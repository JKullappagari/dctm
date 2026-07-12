# Security Key & Password-Hash Migration Plan

Status: **Phase 2 & 3 implemented (server-side)** · Phase 1 deferred · Owner: _TBD_

**Implementation notes (done):**
- Phase 2: `Services/PasswordHasher.cs` (PBKDF2-SHA256, `v2:` prefix, legacy verify → `SuccessRehashNeeded`).
  `TokenController` verifies both formats and, when `Security:UpgradePasswordHashesOnLogin` is `true`
  (**default false**), upgrades legacy hashes on successful login. The rehash uses a **raw parameterized
  UPDATE**, not `SaveChanges`, because `tblUser` has a trigger (`trgInserttblUser`) and EF Core's
  `UPDATE … OUTPUT` is rejected by SQL Server on trigger tables. Covered by unit + SQL integration tests.
- Phase 3: login treats the password as **plaintext over TLS by default** (no production clients used
  the AES scheme); legacy AES-encrypted clients opt in via `X-Password-Encoding: aes`. The anonymous
  `EncryptString` oracle endpoint was **removed**.
- **Still required before enabling the rehash flag:** the main DCTrack app (which shares `tblUser`)
  must understand the `v2:` format, and old mobile clients must migrate to plaintext-over-TLS before
  `CryptographyUtil` can be deleted.

**Retiring `CryptographyUtil` (and the hard-coded key) — remaining preconditions:**
With plaintext-over-TLS now the login default, `CryptographyUtil.Encrypt/Decrypt` has only two
remaining reachable call paths:
1. The **opt-in `X-Password-Encoding: aes`** login header (`TokenController.Create`).
2. The **connection-string decryption fallback** in `Startup.cs` (only for environments still storing
   an encrypted connection string — i.e. wherever **Phase 1** has not yet been completed).

To delete `CryptographyUtil` and its hard-coded `Key`/`SaltBytes` outright:
- [ ] Confirm via request logs/telemetry that **no client sends `X-Password-Encoding: aes`**, then remove
      the `aes` branch from `TokenController.Create`.
- [ ] Complete **Phase 1** (connection string → plaintext in a secret store) and remove the decrypt
      fallback from `Startup.cs`.
- [ ] Delete `Types/CryptographyUtil.cs` and `EncryptDecrypt/CryptographyUtil.cs`, then update the
      unit/integration tests that reference the legacy hash/cipher (the backward-compat tests become
      obsolete once no legacy value can exist).

Until both call paths above are gone, `CryptographyUtil` must remain.


This plan removes the hard-coded cryptographic key and the fixed password-hash salt from
`CryptographyUtil` and replaces the weak password hashing, **without breaking existing encrypted
values, stored password hashes, or deployed mobile clients**.

---

## 1. Current state (what we're migrating away from)

`DCTMRestAPI/Types/CryptographyUtil.cs` (and an identical copy in `EncryptDecrypt/`) contains:

```csharp
public static string Key = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ@#$%"; // hard-coded, in source + binary
public static byte[] SaltBytes = { 0x64, ... };                       // hard-coded IV source
```

That single key is used for **three different jobs**, which is the core problem — they must be
separated before anything can be rotated:

| # | Use | Call site | Trust boundary | Can rotate server-side alone? |
|---|-----|-----------|----------------|-------------------------------|
| A | **At-rest** encryption of the DB connection string | `Startup.cs:55` `UseSqlServer(CryptographyUtil.Decrypt(connString))` | Server only | ✅ Yes |
| B | **Password hashing** (salted SHA-256, **fixed** 4-byte salt `652`) | `TokenController.GetSHA256HashValue` → `ComputeHash(pwd,"SHA256",salt)` | Server only | ✅ Yes (via rehash-on-login) |
| C | **Transport** encryption of the login password | `TokenController.cs:57` `CryptographyUtil.Decrypt(t.Password)` | **Client + server share the key** | ❌ No — breaks mobile clients |

Plus an **encryption oracle**: `GET /api/auth/Token/EncryptString/{Text}` is anonymous and encrypts
arbitrary text with the shared key (`TokenController.cs:93`).

### Why each is a problem
- **A** — The "encryption" is obfuscation only: the key ships in the binary, so anyone with the
  assembly can decrypt the connection string. No real protection.
- **B** — A *single fixed salt* for every user defeats the purpose of salting (rainbow-table-able,
  identical passwords → identical hashes), and raw SHA-256 is far too fast for password storage.
- **C** — A symmetric key embedded in the client is not secret. The scheme adds complexity, not
  security; transport confidentiality is already provided by TLS.

---

## 2. Guiding principles

1. **Backward compatibility first.** Every phase must verify *both* the old and new formats during a
   transition window. No flag day.
2. **Separate the three concerns** before rotating any key (they currently share one).
3. **Never require plaintext we don't have.** Passwords can only be migrated *on successful login*
   (rehash-on-login) — there is no bulk re-hash.
4. **Secrets live in a store, not in source or committed config** — user secrets (dev), environment
   variables / Azure Key Vault (prod). This matches what we already did for the JWT signing key.
5. **Every phase is independently shippable and independently reversible.**

---

## 3. Phase 0 — Preparation (no behavior change)

- [ ] **Inventory environments** and record, per environment, whether `ConnectionStrings:DCTrackDatabase`
      is currently stored **encrypted** or **plaintext** (contains `source`). The app already branches
      on this at `Startup.cs:53`.
- [ ] **Back up** `tblUser` (`Password` column) and every environment's `appsettings*.json`.
- [ ] **Provision a secret store**: confirm Key Vault (prod) / environment-variable strategy. Dev already
      uses user secrets.
- [ ] Add a feature-flag config section, e.g. `Security:PasswordHashV2Enabled`, defaulted `false`, to gate Phase 2.
- [ ] Confirm the integration-test DB (restored `DCTrack`) is available so each phase can be verified
      against real data (see §7).

---

## 4. Phase 1 — Retire connection-string encryption (concern A)  ·  risk: LOW

**Goal:** the connection string lives in the secret store as **plaintext**; the custom encrypt/decrypt
path for it is removed. This eliminates use A of the shared key entirely.

**Steps**
1. For each environment, put the plaintext connection string in the secret store
   (`ConnectionStrings__DCTrackDatabase` env var, or Key Vault secret `ConnectionStrings--DCTrackDatabase`).
2. Keep the `Startup.cs` decrypt-fallback **during transition** so already-encrypted values still load:
   ```csharp
   var connString = _configuration.GetConnectionString("DCTrackDatabase");
   var resolved = connString.ToLower().Contains("source")
       ? connString                      // plaintext (from secret store)
       : CryptographyUtil.Decrypt(connString); // legacy encrypted value — transitional
   options.UseSqlServer(resolved);
   ```
3. Once every environment serves plaintext from the store, **delete** the `else`/decrypt branch and the
   `-pe`/`-pd` connection-string options from the `EncryptDecrypt` tool (or retire the tool).

**Backward-compat:** the fallback keeps old encrypted configs working until removed.
**Rollback:** re-point config at the old encrypted `appsettings` value; the fallback still decrypts it.
**Done when:** no environment stores an encrypted connection string and the decrypt branch is deleted.

---

## 5. Phase 2 — Modernize password hashing (concern B)  ·  risk: MEDIUM · highest security value

**Goal:** replace fixed-salt SHA-256 with a per-user-salted, slow KDF, migrating users transparently on
login. Recommended hasher: **`Microsoft.AspNetCore.Identity.PasswordHasher<T>`** (PBKDF2, versioned,
per-user salt, and it reports `SuccessRehashNeeded` — purpose-built for this).

**Storage marker.** Prefix new hashes so both formats coexist, e.g. store `"v2:" + pbkdf2hash`. Absence
of a prefix ⇒ legacy salted-SHA-256.

**Steps**
1. Add an `IPasswordHasher`-style abstraction with two implementations behind a single `Verify` +
   `Hash` API:
   - **Legacy verify** = current behavior. Note: today the login does a *direct equality* check
     (`inputPassword == dbPassword`) using the fixed salt. Switch it to
     `CryptographyUtil.VerifyHash(password, "SHA256", dbPassword)` — `VerifyHash` extracts the salt
     from the stored value, so it validates existing hashes unchanged.
   - **V2** = `PasswordHasher.HashPassword` / `VerifyHashedPassword`.
2. In `TokenController.IsValidUserAndPasswordCombination` (`TokenController.cs:108`):
   ```csharp
   var result = _passwords.Verify(users[0].Password, password); // tries v2, then legacy
   if (result == PasswordVerification.Failed) return false;
   if (result == PasswordVerification.SuccessRehashNeeded)      // legacy hash matched
   {
       users[0].Password = _passwords.HashV2(password);         // upgrade in place
       await _context.SaveChangesAsync();
   }
   return true;
   ```
3. Update **every password write path** to produce v2 hashes: user creation and password change
   (`UsersController`, `UserPasswordController` — audit for `ComputeHash`/`.Password =`).
4. Gate the write-side change behind `Security:PasswordHashV2Enabled` so verify-both can ship first and
   bake before new hashes start being written.
5. After a chosen window, report on remaining legacy hashes (`Password NOT LIKE 'v2:%'`); force a
   password reset for dormant accounts, then remove the legacy verify path.

**Backward-compat:** verify tries v2 then legacy; every successful legacy login self-upgrades.
**Rollback:** v2 hashes can't be downgraded, so rollback = re-enable legacy verify (kept until the final
step) — v2 users would then need a reset. Keep legacy verify until adoption is ~100%.
**Done when:** no `tblUser.Password` lacks the `v2:` marker and the legacy path is deleted.

---

## 6. Phase 3 — Fix transport & the encryption oracle (concern C)  ·  risk: HIGH (mobile-coordinated)

This is the only phase that touches the **mobile client**, because the client encrypts the login
password with the shared key. Do Phases 1–2 first; by then the shared key is used *only* here.

**Target design:** stop custom-encrypting the password. Rely on **TLS** for transport confidentiality
(the connection is already HTTPS). The password travels as plaintext-inside-TLS, like every standard
login API.

**Steps (server accepts both during transition)**
1. Server: in `TokenController.Create`, detect format instead of always decrypting:
   ```csharp
   string password = LooksEncrypted(t.Password)
       ? CryptographyUtil.Decrypt(t.Password) // old clients
       : t.Password;                          // new clients (plaintext over TLS)
   ```
   Prefer an explicit signal over guessing — e.g. a request header `X-Pwd-Encoding: plain|aes`, or a
   new `v2` login route — rather than a fragile try/catch on `Decrypt`.
2. Ship the mobile update that sends the password without client-side encryption (and sets the header /
   uses the v2 route).
3. **Remove the `EncryptString` oracle endpoint** (`TokenController.cs:88`) — it's anonymous and lets
   anyone encrypt with the shared key. If the tooling still needs it, move it server-side / behind auth.
4. Once client telemetry shows old clients are gone, remove the `Decrypt` branch. At that point
   `CryptographyUtil.Encrypt/Decrypt` has **no remaining callers** and the class (and the hard-coded
   `Key`) can be deleted.

**Backward-compat:** server accepts encrypted (old) and plaintext-over-TLS (new) until old clients age out.
**Rollback:** keep accepting the encrypted form (it's still there) — pure additive change server-side.
**Done when:** all clients send plaintext-over-TLS, the oracle endpoint is gone, and `CryptographyUtil`
is deleted.

---

## 7. Testing strategy (reuse the existing harness)

- **Phase 1:** unit test the `Startup` resolver branch (plaintext passthrough vs. legacy decrypt).
  Integration: the app already boots against the restored DB via the SQL-backed test factory — point it
  at a plaintext secret and confirm `/api/Countries` still returns data.
- **Phase 2:** unit-test `Verify` against a known **legacy** hash (fixed-salt SHA-256) *and* a v2 hash.
  Integration: seed a `tblUser` row with a legacy hash, POST `/api/auth/Token`, assert 200 **and** that
  the stored hash is now `v2:` (rehash-on-login) — a direct extension of the existing `SqlServerWriteTests`
  pattern (transaction/cleanup). The `Decrypt_is_backward_compatible_with_legacy_ciphertext` test is the
  template for pinning old-format compatibility.
- **Phase 3:** integration-test both a plaintext-over-TLS login and an AES-encrypted login returning the
  same result; assert `EncryptString` returns 404 after removal.

Each phase must leave **all existing tests green** before merge.

---

## 8. Sequencing & rollback summary

```
Phase 1 (conn-string) ──► Phase 2 (hashing) ──► Phase 3 (transport + delete CryptographyUtil)
   low risk               medium, high value       high risk, mobile-coordinated
   server-only            server-only              client + server
```

- Phases 1 and 2 are **server-only** and can proceed in parallel; both are independently reversible.
- Phase 3 gates deletion of `CryptographyUtil` and is paced by the **mobile release cycle**.
- Do **not** attempt to "rotate the key" as a shortcut: until Phase 3 completes, the key is shared with
  clients, so rotation breaks logins with no security gain.

## 9. Definition of done
- [ ] No connection string is stored encrypted; `Startup` decrypt branch removed.
- [ ] All `tblUser.Password` values are v2 (PBKDF2); legacy verify path removed.
- [ ] Login accepts only plaintext-over-TLS; `EncryptString` oracle removed.
- [ ] `CryptographyUtil` (both copies) and the hard-coded `Key`/`SaltBytes` are **deleted**.
- [ ] Secrets (connection string, JWT key) live only in the secret store.

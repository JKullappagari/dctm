# DCTrack Web (Modernized) — Administrator & Deployment Guide

Audience: system administrators and DevOps engineers deploying and operating the
modernized DCTrack web application.

The product is not a single app — it is **three Angular bundles + four ASP.NET Core
services + Keycloak + SQL Server**. Deploying it means deploying all of them and wiring
their URLs together.

Covers:

1. [Architecture and components](#1-architecture-and-components)
2. [Prerequisites](#2-prerequisites)
3. [The URL configuration problem — read before deploying](#3-the-url-configuration-problem--read-before-deploying)
4. [Configuration reference](#4-configuration-reference)
5. [Authentication — Keycloak setup (Windows, Forms, MFA/SAML)](#5-authentication--keycloak-setup)
6. [Database](#6-database)
7. [Building](#7-building)
8. [Deployment A — Windows Server + IIS](#8-deployment-a--windows-server--iis)
9. [Deployment B — Containers (on-prem & cloud)](#9-deployment-b--containers-on-prem--cloud)
10. [Authorization model](#10-authorization-model)
11. [Operations](#11-operations)
12. [Troubleshooting](#12-troubleshooting)

---

## 1. Architecture and components

| Component | Tech | Internal port | Default published port | Purpose |
|---|---|---|---|---|
| **shell** | Angular 22 (nginx static) | 8080 | **8090** | Host application — the URL users open. Handles OIDC sign-in, renders the permission-driven menu, loads the remotes. |
| **masterdata-mfe** | Angular 22 (nginx static) | 8080 | 8091 | Remote MFE — master-data, assignment, import, report and admin pages. |
| **assets-mfe** | Angular 22 (nginx static) | 8080 | 8092 | Remote MFE — asset browse, lifecycle, create, import, inventory. |
| **masterdata-api** | .NET 10 minimal API | 8080 | 8081 | Master Data Service. |
| **identity-api** | .NET 10 minimal API | 8080 | 8082 | Identity & Access — menu, permissions, group/user rights admin. |
| **asset-api** | .NET 10 minimal API | 8080 | 8083 | Asset browse, lifecycle state machine, import, inventory. |
| **reporting-api** | .NET 10 minimal API | 8080 | 8084 | Report registry and execution. |
| **keycloak** | Keycloak 26 | 8080 | 8180 | OIDC identity provider, realm `dctrack`. |
| **sqlserver** | SQL Server 2022 | 1433 | 1433 | The legacy `dctrack` database (unchanged schema). |

**Micro-frontend composition.** The shell loads the two remotes at runtime using
[native federation](https://www.npmjs.com/package/@angular-architects/native-federation).
It reads `federation.manifest.json` from its own web root, which maps each remote name to
a **browser-reachable** `remoteEntry.json` URL. Because the browser fetches remote bundles
cross-origin, the remotes must serve permissive CORS headers (the shipped nginx config
sends `Access-Control-Allow-Origin: *`).

**Data access.** All four services use **Dapper against the unchanged legacy stored
procedures**. There are no EF migrations and no schema changes — the application is a
new front end over the existing DCTrack database.

**Authentication.** The shell performs OIDC Authorization Code + PKCE against Keycloak
(public client `dctm-shell`). An HTTP interceptor attaches the resulting bearer token to
requests going to the four service origins listed in its `secureRoutes` configuration.
The services validate the JWT and map `preferred_username` to the legacy
`tblUser.LoginName`.

---

## 2. Prerequisites

### Common

| Requirement | Notes |
|---|---|
| SQL Server 2016+ with the `dctrack` database | Legacy schema, restored from backup. Not modified by this application. |
| Keycloak 26 (or another OIDC provider) | Realm `dctrack`, public client `dctm-shell`. |
| TLS certificates | For the shell, the MFEs and each API in production. |
| DNS names for each component | See §3 — you will need stable hostnames. |

### Build machine

| Requirement | Notes |
|---|---|
| Node.js 22 + npm | Angular 22 workspace. |
| .NET 10 SDK | Four services. |
| Docker (for the container path) | |

### Windows / IIS hosts

| Requirement | Notes |
|---|---|
| Windows Server 2019 / 2022 / 2025 | |
| **ASP.NET Core 10 Hosting Bundle** | For the four APIs. Install after IIS. |
| **IIS URL Rewrite Module 2.1** | Required for Angular SPA deep-link fallback. |
| IIS Static Content role service | |

### Container hosts

Docker/containerd or Kubernetes 1.27+, an OCI registry, and a secret store.

---

## 3. The URL configuration problem — read before deploying

**This is the single biggest gotcha in deploying this application.**

The Angular sources currently **hard-code `http://localhost:<port>` API base URLs** at
build time. They are compiled into the bundle; there is no runtime `environment.json` to
edit after the fact. The affected locations are:

| File | Constant | Points at |
|---|---|---|
| `frontend/projects/shell/src/app/app.config.ts` | `authority` | Keycloak `http://localhost:8180/realms/dctrack` |
| `frontend/projects/shell/src/app/app.config.ts` | `secureRoutes` | `http://localhost:8081/8082/8083/8084` |
| `frontend/projects/shell/src/app/menu.service.ts` | menu URL | identity `http://localhost:8082/api/v1` |
| `frontend/projects/master-data/src/app/lookup/lookup.model.ts` | API base | master-data `http://localhost:8081/api/v1` |
| `frontend/projects/master-data/src/app/lookup/permissions.service.ts` | permissions URL | identity `http://localhost:8082/api/v1` |
| `frontend/projects/master-data/src/app/admin/group-rights-page.ts` | `IDENTITY_BASE` | identity `http://localhost:8082/api/v1` |
| `frontend/projects/master-data/src/app/admin/user-groups-page.ts` | `IDENTITY_BASE` | identity `http://localhost:8082/api/v1` |
| `frontend/projects/master-data/src/app/report/report-page.ts` | `REPORTS` | reporting `http://localhost:8084/api/v1/reports` |
| `frontend/projects/assets/src/app/asset.service.ts` | `BASE` | asset `http://localhost:8083/api/v1` |
| `frontend/projects/assets/src/app/asset-form-page.ts` | `MASTER`, `ASSETS` | master-data 8081, asset 8083 |
| `frontend/projects/assets/src/app/asset-import-page.ts` | `BASE` | asset `http://localhost:8083/api/v1` |
| `frontend/projects/assets/src/app/inventory-page.ts` | `ASSETS` | asset `http://localhost:8083/api/v1` |

Only the **federation manifest** is externalised — `Dockerfile.shell` rewrites
`federation.manifest.json` at build time from the `MASTER_DATA_URL` / `ASSETS_URL` build
arguments.

Choose one of three strategies before you deploy anywhere except a developer laptop:

### Strategy 1 — Single origin behind a reverse proxy (recommended)

Publish everything on one hostname and give each API a path prefix, then change the
hard-coded constants to **relative paths** once:

| Public path | Backend |
|---|---|
| `/` | shell |
| `/mfe/master-data/` | masterdata-mfe |
| `/mfe/assets/` | assets-mfe |
| `/api/master/` | masterdata-api |
| `/api/identity/` | identity-api |
| `/api/asset/` | asset-api |
| `/api/reporting/` | reporting-api |
| `/auth/` | Keycloak |

Benefits: no CORS at all, no per-environment rebuild for the APIs, one certificate, one
DNS name, and `secureRoutes` becomes a single relative entry. This is the least
operational work in the long run and is what a production deployment should look like.

### Strategy 2 — Per-environment build

Keep separate origins and rebuild the Angular bundles per environment, replacing the
constants (via `fileReplacements` in `angular.json`, an `environment.ts`, or a build-time
`sed`). Workable, but every environment needs its own image, and images are no longer
promotable from test to production unchanged.

### Strategy 3 — Runtime configuration (best long-term fix)

Introduce an `assets/config.json` fetched via `APP_INITIALIZER` and have every service
read its base URL from it. Then one image serves every environment and the config is a
mounted file or ConfigMap. This is a small code change and is worth doing before the first
production rollout.

**The rest of this guide assumes Strategy 1 or 2.** Where a step depends on the choice,
it is called out.

---

## 4. Configuration reference

### 4.1 Service settings (all four APIs)

Environment-variable form uses `__` for `:`.

| Key | Env var | Required | Purpose |
|---|---|---|---|
| `ConnectionStrings:MasterData` | `ConnectionStrings__MasterData` | masterdata-api | SQL connection string. |
| `ConnectionStrings:IdentityAccess` | `ConnectionStrings__IdentityAccess` | identity-api | SQL connection string. |
| `ConnectionStrings:Asset` | `ConnectionStrings__Asset` | asset-api | SQL connection string. |
| `ConnectionStrings:Reporting` | `ConnectionStrings__Reporting` | reporting-api | SQL connection string. |
| `Keycloak:MetadataAddress` | `Keycloak__MetadataAddress` | Yes | OIDC discovery document, e.g. `https://sso.contoso.com/realms/dctrack/.well-known/openid-configuration`. |
| `Keycloak:ValidIssuers:0..n` | `Keycloak__ValidIssuers__0` | Yes | Accepted `iss` values. List **every** URL the token may be issued under — browser-facing and internal both, if they differ. |
| `Cors:Origins:0..n` | `Cors__Origins__0` | Unless single-origin | Origins allowed to call the API. Must include the shell's public origin. Not needed under Strategy 1. |
| `Authorization:Enforce` | `Authorization__Enforce` | **Yes — set `true`** | Turns on JWT + legacy-permission enforcement. Ships as `false` in `appsettings.json` for developer convenience. |
| `ASPNETCORE_ENVIRONMENT` | same | Yes | `Production` outside development. |
| `ASPNETCORE_URLS` | same | Containers | `http://+:8080` in the images. |

> **`Authorization:Enforce` must be `true` in every non-development deployment.** With it
> `false` the APIs accept an unauthenticated `X-User-Id` header as the caller's identity —
> that is a development shortcut, not an authentication mechanism. When `true`, the JWT is
> the only accepted identity, the `X-User-Id` fallback is disabled, and a fallback
> authorization policy requires a valid token on *every* endpoint except `/healthz` and the
> OpenAPI document.

### 4.2 Frontend build inputs

| Input | Where | Purpose |
|---|---|---|
| `PROJECT` (build arg) | `Dockerfile.mfe` | Which Angular project to build: `master-data` or `assets`. |
| `MASTER_DATA_URL` (build arg) | `Dockerfile.shell` | Browser-reachable base URL of the master-data remote. Default `http://localhost:8091`. |
| `ASSETS_URL` (build arg) | `Dockerfile.shell` | Browser-reachable base URL of the assets remote. Default `http://localhost:8092`. |
| `federation.manifest.json` | shell web root | Generated from the two build args; maps remote name → `remoteEntry.json`. Can also be edited post-build. |

`federation.manifest.json` is a plain file in the shell's web root, so for an IIS
deployment you can simply write it after copying the bundle — no rebuild needed:

```json
{
  "master-data": "https://dctrack.contoso.com/mfe/master-data/remoteEntry.json",
  "assets":      "https://dctrack.contoso.com/mfe/assets/remoteEntry.json"
}
```

### 4.3 Compose environment file

`.env` at the repository root:

```ini
MSSQL_SA_PASSWORD=<strong password>
KEYCLOAK_ADMIN_PASSWORD=<strong password>
```

Both are referenced with `:?` in `docker-compose.yml`, so compose fails fast if unset.

---

## 5. Authentication — Keycloak setup

### 5.0 Current state — read this first

**The shipped realm is a development stub.** `identity/dctrack-realm.json` is 1 KB and
contains only:

- realm `dctrack`, `registrationAllowed: false`
- one public client **`dctm-shell`** (PKCE `S256`, standard flow **and** direct access
  grants enabled, redirect URIs `http://localhost:4200/*` and `http://localhost:8090/*`)
- **two hard-coded users**, `admin` and `droy`, with non-temporary plaintext passwords
  (`DevOnly!Admin1`, `DevOnly!Droy1`)

It defines **no user federation, no identity providers, no authentication flows, no OTP
policy, and no password policy**. Every item in §5.2–§5.5 below is work you must do; none
of it is pre-configured.

#### Is Forms authentication already handled using the existing DB objects?

**No.** Nothing in `dctm-web-mfe` authenticates anyone against the legacy database.

| Question | Finding |
|---|---|
| Does the new stack verify passwords against `tblUser.Password`? | **No.** No component reads that column. |
| Does the identity service read `tblUser`? | **Yes — but only `LoginName`.** `PermissionsRepository` queries `WHERE U.LoginName = @loginName AND U.Status = 1` to resolve *permissions*. It assumes the caller is **already authenticated** by Keycloak. |
| Is there a Keycloak user-storage provider pointing at `tblUser`? | **No.** The realm has no `userFederationProviders` and no custom SPI. |
| Are the legacy lockout / expiry / audit rules enforced? | **No.** See the gap table below. |

**What this means in practice:** today, a user can only sign in if they exist *in
Keycloak's own user store*. Their DCTrack account governs only what they can then see and
do. The two stores are joined by exactly one string — `preferred_username` must equal
`tblUser.LoginName` (§5.1).

So the legacy Forms-authentication database objects are all still present and populated,
but **orphaned** in the new stack:

| Legacy DB object / rule | Legacy use | Status in the new stack |
|---|---|---|
| `tblUser.Password` | Password hash compared on login | **Unused.** |
| `tblUser.UserType` | Distinguishes corporate (AD) from internal (local password) users | **Unused.** |
| `tblUser.FailedLoginAttempts` + `MaxNoOfLoginAttempts` (3) | Lockout counter | **Unused** — use Keycloak brute-force detection instead (§5.5). |
| `iAssetTrack_Sp_Manage_Users_LoginAttempt_Update` / `_Lock_User` / `_Reset_User` | Increment, lock, unlock | **Never called.** |
| `tblUser.ExpiryDate` | Password expiry, warning countdown | **Unused** — Keycloak password policy replaces it. |
| `tblUser.IsFirstLogin` | Force password change on first login | **Unused** — Keycloak *Update Password* required action replaces it. |
| `tblUser.Status` | Account active flag | **Partly honoured** — permissions queries filter `Status = 1`, so a disabled DCTrack user authenticates successfully but gets an empty menu and 403s. Disable the account in Keycloak too. |
| `iAssetTrack_Sp_AuditLoginLogout` | Login/logout audit trail | **Not wired.** No login auditing exists in the new stack — a compliance gap to close (Keycloak event logging is the natural replacement). |

> **A disabled or deleted DCTrack user is not blocked from signing in.** They will reach
> the shell and see an empty menu. Account lifecycle must be managed in Keycloak (or the
> federated directory), not only in `tblUser`.

#### The legacy password format, if you need to migrate it

`Login.aspx.cs` computes `CommonBAL.GetSHA256HashValue(password)` and compares it for
**exact string equality** with `tblUser.Password`. That value is:

```
Base64( SHA256( utf8(password) || salt ) || salt )
      where salt = the 4 big-endian bytes of the integer 652  →  0x00 0x00 0x02 0x8C
```

The salt is a **compile-time constant shared by every user** — it is not a per-user salt,
so the hashes are effectively unsalted, fast, and vulnerable to rainbow tables. Treat any
extract of that column as compromised material.

Two notes that will otherwise cost you time:

- `web.config` registers `CSqlMembershipProvider` with `passwordFormat="Clear"`, which
  suggests cleartext storage. It is misleading — no `ValidateUser` call exists anywhere in
  the legacy codebase, and `Login.aspx.cs`, `MobileLogin.aspx.cs` and
  `iAssetTrackCorporate.aspx.cs` all bypass the membership provider entirely. **Passwords
  are hashed as above, not cleartext**, and the configured provider appears to be dead
  configuration.
- The separate **mobile REST API** (`dctm-api`) *does* still verify these hashes, via its
  `PasswordHasher` (PBKDF2 "v2:" for new hashes, the fixed-salt SHA-256 above for legacy).
  If you migrate or rotate `tblUser.Password`, you will affect the mobile API's logins —
  coordinate the two.

---

### 5.1 The one hard integration constraint

> **`preferred_username` in the Keycloak token MUST equal `tblUser.LoginName`.**

Every service resolves the caller this way. `IdentityAccess.Api/Program.cs` sets
`NameClaimType = "preferred_username"`, and `PermissionsRepository` looks that value up
against `tblUser.LoginName`. The master-data API does the same to fill the `CreatedBy` /
`LastModifiedBy` audit columns.

The legacy corporate page (`iAssetTrackCorporate.aspx.cs`) received `DOMAIN\samAccountName`
from IIS and **stripped the domain** before the lookup — so `LoginName` holds bare
**sAMAccountName** values, not UPNs and not emails.

Consequences for every scenario below:

- LDAP/AD federation must map the Keycloak username from **`sAMAccountName`**, not
  `userPrincipalName` and not `mail`.
- A SAML/OIDC brokered IdP must have a username mapper producing the same bare value.
- Getting this wrong produces the signature failure mode: **sign-in succeeds, the sidenav
  is empty, and every API call returns 403** — because the login resolves to no `tblUser`
  row. That is not a permissions bug; it is a username-format mismatch.

Verify the mapping for any new user before rollout:

```sql
SELECT UserID, LoginName, Status, UserType FROM tblUser WHERE LoginName = '<preferred_username>';
```

---

### 5.2 Baseline production hardening (do this regardless of scenario)

1. **Run Keycloak in production mode.** The compose file uses `start-dev`, which runs an
   **in-memory H2 database — every realm change is lost on restart.** Switch to `start`
   with `KC_DB` (PostgreSQL), `KC_HOSTNAME`, and TLS:

   ```yaml
   keycloak:
     image: quay.io/keycloak/keycloak:26.0
     command: ["start", "--optimized"]
     environment:
       KC_DB: postgres
       KC_DB_URL: jdbc:postgresql://pg:5432/keycloak
       KC_DB_USERNAME: keycloak
       KC_DB_PASSWORD: ${KC_DB_PASSWORD:?}
       KC_HOSTNAME: https://sso.contoso.com
       KC_PROXY_HEADERS: xforwarded
       KC_HTTP_ENABLED: "false"
   ```

2. **Delete the development users** `admin` and `droy` and their credentials.
3. **Change the bootstrap admin password** and restrict the admin console to an
   administrative network.
4. **Fix the client redirect URIs.** Replace the localhost entries on `dctm-shell` with
   your production shell origin. Use a specific path (`https://dctrack.contoso.com/*`),
   never `*` — a wildcard redirect URI on a public client is a token-theft vector.
5. **Disable direct access grants** on `dctm-shell` after the §5.6 smoke test. The shell
   only uses the authorization-code flow; the password grant is a dev convenience.
6. **Set `RequireHttpsMetadata = true`.** All four services currently hard-code
   `options.RequireHttpsMetadata = false` with the comment *"dev only — Keycloak runs
   plain HTTP locally."* This must be `true` in production — leaving it `false` allows the
   OIDC discovery document and signing keys to be fetched over plain HTTP, which is
   spoofable. **This is a code change**, not a setting.
7. **Enable brute-force detection** (Realm Settings → Security defenses) — this is what
   replaces the legacy `FailedLoginAttempts` / `MaxNoOfLoginAttempts=3` lockout.
8. **Set a password policy** (Authentication → Policies) — length, complexity, history,
   and *Expire Password* if you want to keep the legacy `ExpiryDate` behaviour.
9. **Enable event logging and persistence** (Realm Settings → Events): turn on Save Events
   and Admin Events, set an expiry, and ship them to your SIEM. This is the replacement for
   `iAssetTrack_Sp_AuditLoginLogout`.

---

### 5.3 Scenario A — Windows / Active Directory authentication

Replaces the legacy `ActiveDirectoryMembershipProvider` and the
`iAssetTrackCorporate.aspx` corporate-login page (`UserType` = corporate).

There are two layers; **A1 is required, A2 is optional** and adds desktop single sign-on.

#### A1 — LDAP user federation (username + password against AD)

Admin console → **User federation** → **Add LDAP provider**:

| Setting | Value |
|---|---|
| Vendor | Active Directory |
| Connection URL | `ldaps://dc01.contoso.com:636` (use LDAPS — `ldap://` sends the bind password in the clear) |
| Bind type | `simple` |
| Bind DN | `CN=svc-keycloak,OU=Service Accounts,DC=contoso,DC=com` |
| Bind credential | service-account password |
| Edit mode | `READ_ONLY` (recommended — Keycloak never writes to AD) |
| Users DN | `OU=Users,DC=contoso,DC=com` |
| **Username LDAP attribute** | **`sAMAccountName`** ← see §5.1 |
| RDN LDAP attribute | `cn` |
| UUID LDAP attribute | `objectGUID` |
| User object classes | `person, organizationalPerson, user` |
| User LDAP filter | e.g. `(memberOf=CN=DCTrack-Users,OU=Groups,DC=contoso,DC=com)` to restrict who can sign in |
| Import users | `On` |
| Sync registrations | `Off` |
| Trust email | `On` |

Then **Mappers** on the provider — confirm the `username` mapper reads
`sAMAccountName`. Keycloak's AD defaults sometimes map username to `cn` or
`userPrincipalName`; either will break the `tblUser.LoginName` join.

Set periodic full and changed-users sync so deactivated AD accounts stop authenticating.

Verify the claim before rolling out:

```bash
# decode the token payload and check preferred_username is a bare sAMAccountName
echo "$TOKEN" | cut -d. -f2 | base64 -d 2>/dev/null | jq .preferred_username
# → "jsmith"   ✓        → "jsmith@contoso.com"  ✗ fix the mapper
```

#### A2 — Kerberos / SPNEGO desktop SSO (no password prompt)

This reproduces what IIS Windows Authentication did for the legacy corporate page:
domain-joined users are signed in silently.

1. Create a service principal and keytab on a domain controller:

   ```
   setspn -S HTTP/sso.contoso.com CONTOSO\svc-keycloak

   ktpass /princ HTTP/sso.contoso.com@CONTOSO.COM ^
          /mapuser CONTOSO\svc-keycloak ^
          /pass <password> /crypto AES256-SHA1 /ptype KRB5_NT_PRINCIPAL ^
          /out keycloak.keytab
   ```

2. Mount the keytab into the Keycloak container (read-only, e.g. `/etc/keycloak.keytab`)
   and make `/etc/krb5.conf` resolve the `CONTOSO.COM` realm.

3. On the LDAP provider, enable **Kerberos integration**:

   | Setting | Value |
   |---|---|
   | Allow Kerberos authentication | `On` |
   | Kerberos realm | `CONTOSO.COM` |
   | Server principal | `HTTP/sso.contoso.com@CONTOSO.COM` |
   | Key tab | `/etc/keycloak.keytab` |
   | Debug | `On` initially |

4. **Client browsers must be configured** or SPNEGO silently falls back to a password
   prompt:
   - Edge/Chrome (Windows): add `https://sso.contoso.com` to the **Local Intranet** zone
     via Group Policy.
   - Firefox: add the host to `network.negotiate-auth.trusted-uris`.

5. Kerberos requires the browser to reach Keycloak on the **exact SPN hostname** over
   HTTPS. A mismatch between `KC_HOSTNAME`, the SPN, and the URL users type is the usual
   cause of silent fallback.

> **Users still need a `tblUser` row.** AD authentication proves identity; DCTrack
> permissions come from `tblUser` → `tblGroupMember` → `tblGroupModuleRight`. Provisioning
> an AD user does **not** create the DCTrack row — create it on `/master/users` and assign
> groups on `/master/admin/user-groups`, or the user signs in to an empty application.
> This matches the legacy `UserNotFound.aspx` behaviour, minus the explicit error page.

---

### 5.4 Scenario B — Forms authentication (local DCTrack accounts)

For users who are **not** in the corporate directory — legacy `UserType` = internal,
external subcontractors, service accounts. The legacy `Login.aspx` served exactly this
population.

As established in §5.0, this is **not implemented**. Pick one of three approaches.

#### Option B1 — Migrate users into Keycloak with a forced password reset (recommended)

Simplest, safest, and it retires the weak hash format permanently. Passwords are *not*
carried over — users set a new one on first sign-in.

1. Extract the local accounts:

   ```sql
   SELECT LoginName, FirstName, LastName, Email, Status
   FROM   tblUser
   WHERE  Status = 1 AND UserType = 0;   -- confirm the flag's polarity in your data first
   ```

2. Create each user in Keycloak with `username` = `LoginName` (§5.1), `enabled = true`,
   **no credential**, and the `UPDATE_PASSWORD` required action:

   ```bash
   kcadm.sh create users -r dctrack \
     -s username=jsmith -s enabled=true \
     -s email=jsmith@contoso.com -s firstName=John -s lastName=Smith \
     -s 'requiredActions=["UPDATE_PASSWORD"]'
   ```

   Add `VERIFY_EMAIL` and configure SMTP if you want self-service reset to work.

3. Communicate the cutover — every local user must set a new password on first sign-in.
4. After cutover, **null out `tblUser.Password`** for migrated users so the weak hashes
   stop being a liability. Check the mobile API (`dctm-api`) first — if those users also
   use the handheld clients, their logins go through the same column and will break.

**Trade-off:** a coordinated password reset for every local user. For a typical DCTrack
deployment that is a handful of accounts, which is why this is the recommendation.

#### Option B2 — Custom User Storage SPI against `tblUser` (seamless cutover)

Keycloak can delegate credential validation to your database via a Java **User Storage
SPI** provider. Users sign in with their existing DCTrack password on day one, with no
reset.

The provider implements `UserStorageProvider`, `UserLookupProvider` and
`CredentialInputValidator`, and its `isValid` must reproduce the legacy algorithm exactly:

```java
// salt = big-endian bytes of the int 652 → { 0x00, 0x00, 0x02, 0x8C }
byte[] salt = ByteBuffer.allocate(4).putInt(652).array();
MessageDigest sha256 = MessageDigest.getInstance("SHA-256");
sha256.update(password.getBytes(StandardCharsets.UTF_8));
sha256.update(salt);
byte[] hash = sha256.digest();

byte[] stored = new byte[hash.length + salt.length];       // hash ‖ salt
System.arraycopy(hash, 0, stored, 0, hash.length);
System.arraycopy(salt, 0, stored, hash.length, salt.length);

boolean ok = MessageDigest.isEqual(
        Base64.getEncoder().encode(stored),
        dbPasswordValue.getBytes(StandardCharsets.US_ASCII));   // constant-time compare
```

Set the provider to **import mode** so that on each successful login Keycloak stores its
own modern hash and stops consulting the legacy column — the same rehash-on-login pattern
the mobile API already implements behind
`Security:UpgradePasswordHashesOnLogin`. Once the population has drained, remove the
provider.

**Trade-off:** a Java artifact to build, deploy into `providers/`, and maintain across
Keycloak upgrades — and for a transition period Keycloak is validating against a weak,
globally-salted hash. Worth it only if a coordinated reset is genuinely impractical.

#### Option B3 — Keep local users on the legacy app

Leave `Login.aspx` serving local accounts during the migration window and put only
corporate/AD users on the new stack. Both applications read the same `tblUser` table, so
permissions stay consistent. This is the lowest-effort path if the local-user population
is being retired anyway.

#### Regardless of option — replace the legacy Forms rules

The behaviours `Login.aspx.cs` implemented must be re-created as Keycloak configuration,
or they are silently lost:

| Legacy behaviour | Keycloak replacement |
|---|---|
| `FailedLoginAttempts` ≥ 3 → lock | Realm Settings → Security defenses → **Brute force detection** (permanent or temporary lockout) |
| `ExpiryDate` password expiry + warning | Authentication → Policies → Password policy → **Expire Password (days)** |
| `IsFirstLogin` → force change | **`UPDATE_PASSWORD`** required action on the user |
| `Status` inactive → block | Set the Keycloak user `enabled = false` (do **not** rely on `tblUser.Status` alone) |
| `iAssetTrack_Sp_AuditLoginLogout` | Realm Settings → Events → Save Events + SIEM export |

---

### 5.5 Scenario C — MFA and SAML federation

These are two different things and are frequently conflated. Both are pure Keycloak
configuration — **neither requires an application change**, because the shell always
speaks OIDC to Keycloak no matter how Keycloak established the identity.

```
                    ┌──────── OIDC (always) ────────┐
Angular shell ──────┤                               ├────── Keycloak
                    └───────────────────────────────┘          │
                                                               │ upstream, pluggable:
                                                               ├─ LDAP / Kerberos  (§5.3)
                                                               ├─ local users      (§5.4)
                                                               └─ SAML 2.0 / OIDC broker (below)
```

#### C1 — MFA (second factor at Keycloak)

**OTP / TOTP** (Google Authenticator, Authy, Microsoft Authenticator):

1. Authentication → **Policies → OTP Policy** — type `Time-based`, algorithm `SHA1` or
   better, 6 digits, 30-second window.
2. Enrolment, one of:
   - **Everyone:** Authentication → Required Actions → enable *Configure OTP* as
     **Default action** — every user enrols at next sign-in.
   - **Per user:** add the `CONFIGURE_TOTP` required action to selected users.
   - **Conditional (recommended):** duplicate the `browser` flow, add a
     **Condition – User Role** (or group) sub-flow, and make OTP `Required` only inside it.
     This lets you require MFA for administrators — anyone holding rights on
     *Group Module Access Rights Assignment* — without imposing it on every warehouse
     operator.

**WebAuthn / passkeys** (phishing-resistant, preferred where hardware allows): register a
WebAuthn policy and use the *WebAuthn Register* required action with the
`webauthn-authenticator` execution in the flow. WebAuthn requires HTTPS and the relying
party ID must match your Keycloak hostname.

> With Kerberos SSO (§5.3-A2), the desktop login is already a domain-authenticated factor.
> Applying conditional OTP only to privileged roles avoids prompting domain users twice.

#### C2 — SAML 2.0 identity-provider brokering (SSO to a corporate IdP)

This is **federation, not MFA** — it delegates authentication to Entra ID, ADFS, Okta,
PingFederate and so on. When you broker, MFA is normally enforced *at the upstream IdP*,
and Keycloak simply consumes the result. Configure MFA in one place, not both.

Admin console → **Identity providers** → **SAML v2.0**:

| Setting | Value |
|---|---|
| Alias | `corp-saml` (appears in the login URL — keep it stable) |
| Service provider entity ID | `https://sso.contoso.com/realms/dctrack` |
| Import from URL | your IdP's federation metadata URL (fills the endpoints and certificate) |
| Single Sign-On service URL | from IdP metadata |
| NameID policy format | `persistent`, or `Unspecified` if you map username from an attribute |
| Principal type | `Attribute [Name]` when the username comes from a claim rather than NameID |
| Want AuthnRequests signed | `On` |
| Want Assertions signed / encrypted | `On` / per policy |
| Validate signatures | `On`, with the IdP signing certificate |
| Sync mode | `Force` (re-apply mappers on every login, so directory changes propagate) |

Give your IdP administrator the SP metadata from:

```
https://sso.contoso.com/realms/dctrack/broker/corp-saml/endpoint/descriptor
```

**Then add the username mapper — this is the step that breaks deployments.** Identity
provider → *corp-saml* → **Mappers** → Add:

| Setting | Value |
|---|---|
| Mapper type | `Username Template Importer` |
| Template | `${ATTRIBUTE.sAMAccountName}` (or `${NAMEID}` / `${ATTRIBUTE.uid}` — whatever yields the **bare** account name) |
| Target | `LOCAL` |

Entra ID commonly emits `user.userprincipalname` (`jsmith@contoso.com`) as NameID. That
will **not** match `tblUser.LoginName` (§5.1). Either have the IdP release
`sAMAccountName` (or `onPremisesSamAccountName`) as a claim and map from it, or use a
template that strips the domain: `${ALIAS.jsmith}` style transformations are not
supported, so prefer releasing the correct attribute at the IdP.

Add further mappers for `email`, `firstName`, `lastName` for a usable user profile.

**Optional:** to send users straight to the corporate IdP without a Keycloak login page,
set `kc_idp_hint=corp-saml` — either as an extra authorization request parameter in the
shell's `provideAuth` config, or by marking the IdP as the default for the browser flow.

**OIDC brokering** (Entra ID, Okta via OIDC) is configured the same way and is simpler
than SAML — prefer it when your IdP supports both.

---

### 5.6 Verification

Run these after any authentication change.

```bash
# 1. Discovery document reachable over HTTPS
curl -s https://sso.contoso.com/realms/dctrack/.well-known/openid-configuration | jq .issuer

# 2. Obtain a token (only while direct access grants are enabled — see §5.2 item 5)
TOKEN=$(curl -s -X POST https://sso.contoso.com/realms/dctrack/protocol/openid-connect/token \
  -d grant_type=password -d client_id=dctm-shell \
  -d username=<user> -d password='<pwd>' | jq -r .access_token)

# 3. THE critical check — must be a bare sAMAccountName matching tblUser.LoginName
echo "$TOKEN" | cut -d. -f2 | base64 -d 2>/dev/null | jq '.preferred_username, .iss'

# 4. End to end: a populated menu proves the username→tblUser join works
curl -s https://dctrack.contoso.com/api/identity/api/v1/me/menu \
  -H "Authorization: Bearer $TOKEN" | jq '.[].mainModule'

# 5. Enforcement is on: no token must be rejected
curl -s -o /dev/null -w '%{http_code}\n' \
  https://dctrack.contoso.com/api/master/api/v1/manufacturers        # → 401
```

For SSO flows that cannot use the password grant, sign in through the shell in a browser
and read the token from devtools (Application → Session Storage), then run steps 3–4.

**Reading the results:** an empty array at step 4 with a `200` at step 3 means the token is
valid but `preferred_username` matched no `tblUser` row — go back to §5.1. A `200` at
step 5 means `Authorization:Enforce` is still `false` (§4.1).

---

## 6. Database

### 6.1 Restore (non-production)

`db/restore-db.sh` restores `dctrack_25Aug2019.bak` into a database named `dctrack`. In
compose this runs as the `db-init` one-shot service; the backup is mounted read-only from
`../../db`.

For a manual restore:

```sql
RESTORE FILELISTONLY FROM DISK = N'/backups/dctrack_25Aug2019.bak';

RESTORE DATABASE [dctrack] FROM DISK = N'/backups/dctrack_25Aug2019.bak'
WITH MOVE N'<logical_data>' TO N'/var/opt/mssql/data/dctrack.mdf',
     MOVE N'<logical_log>'  TO N'/var/opt/mssql/data/dctrack_log.ldf',
     RECOVERY, REPLACE;
```

### 6.2 Service accounts

The default `appsettings.json` files name one login per service (`masterdata_svc`,
`identity_svc`, `asset_svc`, `reporting_svc`). Keep that separation — it lets you scope
each service's access and attribute activity in SQL audit.

```sql
CREATE LOGIN [masterdata_svc] WITH PASSWORD = N'<strong>';
USE [dctrack];
CREATE USER [masterdata_svc] FOR LOGIN [masterdata_svc];
ALTER ROLE db_datareader ADD MEMBER [masterdata_svc];
ALTER ROLE db_datawriter ADD MEMBER [masterdata_svc];
GRANT EXECUTE TO [masterdata_svc];   -- services call legacy stored procedures
```

Repeat per service. Do not grant `db_owner`. The `reporting_svc` account can be
read-only plus `EXECUTE`.

### 6.3 Notes for DBAs

- No schema migrations. The services call the **existing** DCTrack stored procedures.
- Two legacy stored-procedure defects are worked around in application code and should not
  be "fixed" in the database without re-testing: `Tenant_Delete` (filters on a
  non-existent `TechID` column — replaced by a parameterized soft delete in the service)
  and `Host_Delete` (concatenates an id CSV into dynamic SQL — the service parses each id
  as a GUID first, which also closes a SQL-injection hole).
- Permission lookups hit `tblUser`, `tblGroupMember`, `tblGroupModuleRight`,
  `tblModuleRight`, `tblModule` and `tblRight` on a 60-second cache.

---

## 7. Building

### 7.1 Services

```bash
cd services/master-data
dotnet test                                   # unit tests, no DB required
dotnet publish src/MasterData.Api -c Release -o ./publish
```

Repeat for `services/identity-access`, `services/asset`, `services/reporting`.

### 7.2 Frontend

```bash
cd frontend
npm ci
npx ng build shell       --configuration production
npx ng build master-data --configuration production
npx ng build assets      --configuration production
```

Output lands in `frontend/dist/{shell,master-data,assets}/browser`.

Under **Strategy 2** (§3), replace the hard-coded API constants before building.

### 7.3 Container images

```bash
# services
docker build -t registry.contoso.com/dctm/masterdata-api:1.0.0 services/master-data
docker build -t registry.contoso.com/dctm/identity-api:1.0.0   services/identity-access
docker build -t registry.contoso.com/dctm/asset-api:1.0.0      services/asset
docker build -t registry.contoso.com/dctm/reporting-api:1.0.0  services/reporting

# front-ends
docker build -f frontend/Dockerfile.mfe --build-arg PROJECT=master-data \
  -t registry.contoso.com/dctm/masterdata-mfe:1.0.0 frontend
docker build -f frontend/Dockerfile.mfe --build-arg PROJECT=assets \
  -t registry.contoso.com/dctm/assets-mfe:1.0.0 frontend
docker build -f frontend/Dockerfile.shell \
  --build-arg MASTER_DATA_URL=https://dctrack.contoso.com/mfe/master-data \
  --build-arg ASSETS_URL=https://dctrack.contoso.com/mfe/assets \
  -t registry.contoso.com/dctm/shell:1.0.0 frontend
```

Service images run as the non-root `app` user on port 8080 and carry a `/healthz`
HEALTHCHECK. The front-end images are nginx:alpine serving a static bundle on 8080.

---

## 8. Deployment A — Windows Server + IIS

The application was designed for Linux containers, but it deploys to IIS: the three
Angular bundles are static sites, and the four services are standard ASP.NET Core apps
under the Hosting Bundle.

### 8.1 Install prerequisites

1. Enable the IIS role (Web Server + Static Content).
2. Install the **ASP.NET Core 10 Hosting Bundle**.
3. Install the **IIS URL Rewrite Module 2.1**.
4. `net stop was /y && net start w3svc`.

### 8.2 Site layout (Strategy 1 — single origin)

Create one site, `dctrack.contoso.com`, with the shell at the root and everything else as
IIS applications beneath it:

```
Default site root  →  C:\inetpub\dctm\shell            (Angular shell)
  /mfe/master-data →  C:\inetpub\dctm\mfe-master-data  (Angular remote)
  /mfe/assets      →  C:\inetpub\dctm\mfe-assets       (Angular remote)
  /api/master      →  C:\inetpub\dctm\api-masterdata   (ASP.NET Core)
  /api/identity    →  C:\inetpub\dctm\api-identity     (ASP.NET Core)
  /api/asset       →  C:\inetpub\dctm\api-asset        (ASP.NET Core)
  /api/reporting   →  C:\inetpub\dctm\api-reporting    (ASP.NET Core)
```

```powershell
Import-Module WebAdministration

# One "No Managed Code" app pool per service
foreach ($p in 'DctmMasterData','DctmIdentity','DctmAsset','DctmReporting') {
  New-WebAppPool -Name $p
  Set-ItemProperty "IIS:\AppPools\$p" -Name managedRuntimeVersion -Value ''
  Set-ItemProperty "IIS:\AppPools\$p" -Name startMode -Value 'AlwaysRunning'
  Set-ItemProperty "IIS:\AppPools\$p" -Name processModel.idleTimeout -Value '00:00:00'
}
# Static app pool for the Angular bundles
New-WebAppPool -Name 'DctmStatic'
Set-ItemProperty 'IIS:\AppPools\DctmStatic' -Name managedRuntimeVersion -Value ''

New-Website -Name 'DCTrack' -PhysicalPath 'C:\inetpub\dctm\shell' `
  -ApplicationPool 'DctmStatic' -HostHeader 'dctrack.contoso.com' -Port 80

New-WebApplication -Site 'DCTrack' -Name 'mfe/master-data' -PhysicalPath 'C:\inetpub\dctm\mfe-master-data' -ApplicationPool 'DctmStatic'
New-WebApplication -Site 'DCTrack' -Name 'mfe/assets'      -PhysicalPath 'C:\inetpub\dctm\mfe-assets'      -ApplicationPool 'DctmStatic'
New-WebApplication -Site 'DCTrack' -Name 'api/master'      -PhysicalPath 'C:\inetpub\dctm\api-masterdata'  -ApplicationPool 'DctmMasterData'
New-WebApplication -Site 'DCTrack' -Name 'api/identity'    -PhysicalPath 'C:\inetpub\dctm\api-identity'    -ApplicationPool 'DctmIdentity'
New-WebApplication -Site 'DCTrack' -Name 'api/asset'       -PhysicalPath 'C:\inetpub\dctm\api-asset'       -ApplicationPool 'DctmAsset'
New-WebApplication -Site 'DCTrack' -Name 'api/reporting'   -PhysicalPath 'C:\inetpub\dctm\api-reporting'   -ApplicationPool 'DctmReporting'
```

### 8.3 `web.config` for the Angular bundles

Each of the three static folders needs a `web.config` providing SPA fallback and correct
MIME types. **Use this for the shell** (root application):

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <staticContent>
      <remove fileExtension=".json" />
      <mimeMap fileExtension=".json" mimeType="application/json" />
      <remove fileExtension=".webmanifest" />
      <mimeMap fileExtension=".webmanifest" mimeType="application/manifest+json" />
      <remove fileExtension=".woff2" />
      <mimeMap fileExtension=".woff2" mimeType="font/woff2" />
    </staticContent>
    <rewrite>
      <rules>
        <rule name="Angular SPA fallback" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
            <add input="{REQUEST_URI}" pattern="^/(api|mfe)/" negate="true" />
          </conditions>
          <action type="Rewrite" url="/index.html" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
```

For the **two remote MFE folders**, use the same file plus CORS headers — needed only if
the remotes are served from a different origin than the shell (Strategy 2). Under
Strategy 1 they share an origin and no CORS headers are required:

```xml
<httpProtocol>
  <customHeaders>
    <add name="Access-Control-Allow-Origin" value="https://dctrack.contoso.com" />
    <add name="Access-Control-Allow-Methods" value="GET, OPTIONS" />
  </customHeaders>
</httpProtocol>
```

Also disable caching of `index.html` and `remoteEntry.json` so deployments take effect
immediately, while letting the hashed bundle files cache aggressively.

### 8.4 `web.config` for each service

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <handlers>
      <remove name="aspNetCore" />
      <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
    </handlers>
    <aspNetCore processPath="dotnet" arguments=".\MasterData.Api.dll"
                stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout"
                hostingModel="inprocess">
      <environmentVariables>
        <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
        <environmentVariable name="Authorization__Enforce" value="true" />
      </environmentVariables>
    </aspNetCore>
    <security>
      <requestFiltering>
        <requestLimits maxAllowedContentLength="52428800" /> <!-- xlsx imports -->
      </requestFiltering>
    </security>
  </system.webServer>
</configuration>
```

Change `arguments` per service: `MasterData.Api.dll`, `IdentityAccess.Api.dll`,
`Asset.Api.dll`, `Reporting.Api.dll`.

### 8.5 Secrets on IIS

Set the connection string and Keycloak settings as **app-pool environment variables**
(IIS Manager → Application Pools → *pool* → Advanced Settings → Environment Variables),
or reference a secret store. Do not commit them to `appsettings.json`.

```
ConnectionStrings__MasterData = Server=sql01;Database=dctrack;User ID=masterdata_svc;Password=…;Encrypt=True;
Keycloak__MetadataAddress     = https://sso.contoso.com/realms/dctrack/.well-known/openid-configuration
Keycloak__ValidIssuers__0     = https://sso.contoso.com/realms/dctrack
Authorization__Enforce        = true
ASPNETCORE_ENVIRONMENT        = Production
```

Grant each app-pool identity read on its folder and modify on its `logs` subfolder.

### 8.6 Path-base considerations

Hosting a service under an IIS application path (`/api/master`) means the app sees a path
base. ASP.NET Core handles this automatically for in-process hosting, so
`/api/master/api/v1/manufacturers` routes correctly. Verify with:

```powershell
Invoke-RestMethod https://dctrack.contoso.com/api/master/healthz
```

If you prefer cleaner URLs, host each service as its own site on its own hostname
(`api-master.dctrack.contoso.com`) and use Strategy 2 for the frontend.

### 8.7 TLS

Bind the certificate to the site and redirect HTTP → HTTPS with a URL Rewrite rule.
Keycloak, the shell and the APIs must **all** be HTTPS in production — an OIDC flow that
mixes schemes will fail in modern browsers, and tokens must never traverse plain HTTP.

### 8.8 Deploy the federation manifest

After copying the shell bundle, write `federation.manifest.json` into its web root with
the production remote URLs (§4.2). This is the one file you must touch per environment
even if you rebuild nothing else.

### 8.9 Verify

```powershell
# static
Invoke-WebRequest https://dctrack.contoso.com/                                   -UseBasicParsing | % StatusCode
Invoke-WebRequest https://dctrack.contoso.com/federation.manifest.json           -UseBasicParsing | % Content
Invoke-WebRequest https://dctrack.contoso.com/mfe/master-data/remoteEntry.json   -UseBasicParsing | % StatusCode
Invoke-WebRequest https://dctrack.contoso.com/mfe/assets/remoteEntry.json        -UseBasicParsing | % StatusCode

# services — expect 200 on healthz, 401 on a data endpoint without a token
Invoke-WebRequest https://dctrack.contoso.com/api/master/healthz -UseBasicParsing | % StatusCode
try { Invoke-WebRequest https://dctrack.contoso.com/api/master/api/v1/manufacturers -UseBasicParsing }
catch { $_.Exception.Response.StatusCode }   # → Unauthorized  (proves Enforce=true)
```

A `200` on that last call means `Authorization:Enforce` is still `false`. Fix it before
going live.

---

## 9. Deployment B — Containers (on-prem & cloud)

### 9.1 Local / on-prem with Docker Compose

The repository ships a complete `docker-compose.yml` — SQL Server, database restore,
Keycloak, four services, two MFEs and the shell:

```bash
cp .env.example .env       # set MSSQL_SA_PASSWORD (and KEYCLOAK_ADMIN_PASSWORD)
docker compose up --build
# open http://localhost:8090
```

This stack is a **development and demonstration** configuration. Before using anything
like it for real:

| Change | Why |
|---|---|
| Replace the `sqlserver` and `db-init` services with a connection to your managed SQL Server | The containerised SQL Server has no backup, HA or patching story. |
| Run Keycloak with `start` + a real database + TLS | `start-dev` uses in-memory H2 — realm changes vanish on restart. |
| Set `ASPNETCORE_ENVIRONMENT=Production` on all four services | Compose sets `Development`. |
| Move `MSSQL_SA_PASSWORD` and connection strings out of compose into a secret store | They are plain environment variables today. |
| Stop using the `sa` login | Use the per-service logins from §6.2. |
| Put a TLS-terminating proxy in front | Every published port is plain HTTP. |
| Keep `Authorization__Enforce: "true"` | It is already `true` for masterdata, asset and reporting in compose — **identity-api does not set it**; add it. |

### 9.2 Kubernetes

One Deployment + Service per component. Sketch for the master-data service — repeat the
pattern for identity, asset and reporting:

```yaml
apiVersion: v1
kind: Secret
metadata: { name: dctm-db }
type: Opaque
stringData:
  ConnectionStrings__MasterData: "Server=sql01;Database=dctrack;User ID=masterdata_svc;Password=…;Encrypt=True;"
---
apiVersion: apps/v1
kind: Deployment
metadata: { name: masterdata-api }
spec:
  replicas: 2
  selector: { matchLabels: { app: masterdata-api } }
  template:
    metadata: { labels: { app: masterdata-api } }
    spec:
      containers:
        - name: api
          image: registry.contoso.com/dctm/masterdata-api:1.0.0
          ports: [{ containerPort: 8080 }]
          env:
            - { name: ASPNETCORE_ENVIRONMENT,  value: Production }
            - { name: Authorization__Enforce,  value: "true" }
            - { name: Keycloak__MetadataAddress, value: "https://sso.contoso.com/realms/dctrack/.well-known/openid-configuration" }
            - { name: Keycloak__ValidIssuers__0, value: "https://sso.contoso.com/realms/dctrack" }
            - { name: Cors__Origins__0,          value: "https://dctrack.contoso.com" }
          envFrom: [{ secretRef: { name: dctm-db } }]
          readinessProbe: { httpGet: { path: /healthz, port: 8080 }, initialDelaySeconds: 10 }
          livenessProbe:  { httpGet: { path: /healthz, port: 8080 }, periodSeconds: 30 }
          resources:
            requests: { cpu: "100m", memory: "256Mi" }
            limits:   { cpu: "1",    memory: "1Gi"  }
          securityContext:
            runAsNonRoot: true
            allowPrivilegeEscalation: false
---
apiVersion: v1
kind: Service
metadata: { name: masterdata-api }
spec:
  selector: { app: masterdata-api }
  ports: [{ port: 80, targetPort: 8080 }]
```

All four services expose **`/healthz`** — use it for both probes.

Front-end Deployments are the same shape without the database secret, probing `/` on 8080.

Single-origin Ingress (Strategy 1):

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: dctrack
  annotations:
    nginx.ingress.kubernetes.io/proxy-body-size: "50m"
    nginx.ingress.kubernetes.io/use-regex: "true"
    nginx.ingress.kubernetes.io/rewrite-target: /$2
spec:
  ingressClassName: nginx
  tls: [{ hosts: [dctrack.contoso.com], secretName: dctrack-tls }]
  rules:
    - host: dctrack.contoso.com
      http:
        paths:
          - { path: "/api/master(/|$)(.*)",    pathType: ImplementationSpecific, backend: { service: { name: masterdata-api, port: { number: 80 } } } }
          - { path: "/api/identity(/|$)(.*)",  pathType: ImplementationSpecific, backend: { service: { name: identity-api,   port: { number: 80 } } } }
          - { path: "/api/asset(/|$)(.*)",     pathType: ImplementationSpecific, backend: { service: { name: asset-api,      port: { number: 80 } } } }
          - { path: "/api/reporting(/|$)(.*)", pathType: ImplementationSpecific, backend: { service: { name: reporting-api,  port: { number: 80 } } } }
          - { path: "/mfe/master-data(/|$)(.*)", pathType: ImplementationSpecific, backend: { service: { name: masterdata-mfe, port: { number: 80 } } } }
          - { path: "/mfe/assets(/|$)(.*)",      pathType: ImplementationSpecific, backend: { service: { name: assets-mfe,     port: { number: 80 } } } }
          - { path: "/()(.*)",                   pathType: ImplementationSpecific, backend: { service: { name: shell,          port: { number: 80 } } } }
```

Deliver `federation.manifest.json` to the shell pod via a ConfigMap mounted over
`/usr/share/nginx/html/federation.manifest.json` — that avoids rebuilding the shell image
per environment.

Raise the ingress body-size limit: the import pages upload `.xlsx` files.

### 9.3 Cloud-managed container services

| Platform | Notes |
|---|---|
| **Azure Container Apps** | One app per component; ingress `external: true` on the shell and (Strategy 2) on each API. Built-in TLS and forwarded headers. Secrets via Container Apps secrets or Key Vault. |
| **Azure App Service (Linux containers)** | One Web App per component, or a multi-container app. App settings carry the environment variables; Key Vault references for secrets. Enable *Always On*. |
| **AWS ECS / Fargate** | One task definition per component behind an ALB; path-based listener rules implement Strategy 1 directly. Secrets from Secrets Manager. |
| **Google Cloud Run** | One service per component. Cloud Run's default port is 8080, matching the images. Use a Global External Load Balancer with path rules for Strategy 1. Secrets from Secret Manager. |
| **AKS / EKS / GKE / OpenShift** | Manifests in §9.2. |

**Keycloak in the cloud:** run it as a managed container with an external database, or
replace it with a managed OIDC provider. If you swap providers, the only requirement is
that the token's `preferred_username` matches the legacy `tblUser.LoginName`.

**Network:** if the containers run in the cloud but `dctrack` stays on-premises, you need
VPN / ExpressRoute / Direct Connect and a firewall rule for TCP 1433.

---

## 10. Authorization model

Understanding this matters for support calls.

Authorization derives entirely from the **legacy DCTrack security tables** — the same
`(module, right)` pairs that drove the Web Forms security-trimmed menu. There is no new
permission store.

**Two layers:**

1. **Server-side — the real gate.** Each API endpoint carries a
   `RequirePermission(module, right)` filter. CRUD verbs map to legacy rights:
   `GET → View`, `POST → Create`, `PUT → Modify`, `DELETE → Delete`, against the entity's
   `tblModule.Module` name. The asset API maps each lifecycle action to its right on the
   **"Search Asset"** module (write-off → WriteOff, bar → Bar, restrict → Restrict,
   decommission → Decommission, RFID → Assign/De-assign RFID, reads → View). Active only
   when `Authorization:Enforce` is `true`.

   A **fallback policy** requires a valid JWT on every endpoint that has no explicit module
   guard — assignments, ref/geo lookups, and the three entities with no `tblModule` row
   (Business Unit, Location Type, Asset Group). `/healthz` and the OpenAPI document opt out.

2. **Client-side — UX only.** A route guard in the master-data MFE blocks navigation to
   modules the user has no right on and redirects to `/forbidden`. It reads
   `/api/v1/me/permissions` and **fails open** when it cannot reach the identity service.
   It is defence in depth, not a security control — the API is the enforcer.

**Caching.** Both APIs cache permission lookups for **60 seconds**, so rights edits take
effect within a minute without restarting anything.

**Administering rights** is done in the application itself, on `/master/admin/group-rights`
and `/master/admin/user-groups` — both gated on holding a right on the legacy
*Group Module Access Rights Assignment* module.

---

## 11. Operations

### 11.1 Health and monitoring

| Endpoint / signal | Component | Notes |
|---|---|---|
| `GET /healthz` | all four services | Liveness/readiness. Anonymous. |
| `GET /openapi/v1.json` | all four services | OpenAPI document. Anonymous — consider blocking at the edge in production. |
| `GET /` returns `index.html` | shell, both MFEs | Static health. |
| `GET /remoteEntry.json` | both MFEs | If this 404s, the shell renders a blank page. |

Alert on: pod/app-pool restarts, 5xx rate per service, 401/403 rate (a spike in 403 usually
means a rights change went wrong), SQL connection failures, and Keycloak availability —
Keycloak being down blocks **all** sign-ins.

### 11.2 Logging

Services log to stdout via the standard ASP.NET Core logger (`Information` default,
`Microsoft.AspNetCore` at `Warning`). In containers, collect stdout with your log driver;
under IIS, use the `stdoutLogFile` only for startup diagnosis and add a proper sink for
steady state. nginx access logs from the three front-end containers go to stdout as well.

### 11.3 Backup and DR

- **`dctrack` database** — the only stateful component of the application. Covered by the
  existing DCTrack backup policy; this application adds no schema.
- **Keycloak realm** — export `dctrack` realm configuration after any change
  (`kc.sh export`) and store it with your infrastructure code. Its database (users,
  sessions, federated identities) needs its own backup.
- **Application** — stateless. Redeploy from image tags or published artifacts.
- **Configuration** — back up secret-store entries and the per-environment
  `federation.manifest.json`.

### 11.4 Upgrades and rollback

**Containers**

```bash
kubectl set image deployment/masterdata-api api=registry.contoso.com/dctm/masterdata-api:1.1.0
kubectl rollout status deployment/masterdata-api
kubectl rollout undo   deployment/masterdata-api
```

Deploy front-end and service changes together when a release changes an API contract; the
shell and the remotes are versioned independently and a mismatched remote will fail at
runtime.

**IIS** — publish to a parallel folder, drop `app_offline.htm` into the live one, repoint
the application's physical path, remove `app_offline.htm`. Rolling back is repointing the
path back.

Because there are no schema migrations, application rollback is safe.

### 11.5 Cache-busting after a front-end deploy

Angular bundles are content-hashed, but `index.html`, `remoteEntry.json` and
`federation.manifest.json` are not. Serve those three with `Cache-Control: no-store` (or
a very short TTL), or users will keep loading the previous release from cache. Purge any
CDN in front of the shell as part of the deployment.

---

## 12. Troubleshooting

| Symptom | Cause / fix |
|---|---|
| Shell loads, sidenav empty, pages blank | The browser cannot reach an API, or the JWT is missing. Check the browser console for CORS/401 errors and confirm the hard-coded base URLs match the deployment (§3). |
| Sign-in redirects to `localhost:8180` | The shell bundle was built with the default Keycloak authority. Rebuild with the real authority (Strategy 2) or move to relative/runtime config (Strategies 1/3). |
| Keycloak: "Invalid redirect URI" | The shell's origin is not registered on the `dctm-shell` client. Add it as a valid redirect URI and web origin (§5.2). |
| Everything 401s after switching Keycloak hostnames | `Keycloak__ValidIssuers` does not include the new issuer. List every issuer URL a token may carry. |
| **Sign-in succeeds but the sidenav is empty and every call 403s** | The classic username-mapping failure: `preferred_username` does not match any `tblUser.LoginName`. Usually a UPN/email (`jsmith@contoso.com`) where a bare sAMAccountName (`jsmith`) is required. Fix the LDAP username attribute or the SAML username mapper (§5.1). |
| A brand-new AD user signs in to an empty application | AD authentication does not provision DCTrack permissions. Create the user on `/master/users` and assign groups on `/master/admin/user-groups` (§5.3). |
| A user removed from DCTrack can still sign in | Expected — account lifecycle lives in Keycloak/AD, not `tblUser`. Disable the Keycloak user as well (§5.0). |
| Local (non-AD) users cannot sign in at all | Forms authentication against `tblUser.Password` is **not implemented**. Choose an approach from §5.4. |
| Kerberos SSO silently falls back to a password prompt | The browser has not been told to trust the host (Local Intranet zone / `network.negotiate-auth.trusted-uris`), or the SPN does not match `KC_HOSTNAME` and the URL users type (§5.3-A2). |
| Realm/user changes vanish after a Keycloak restart | Keycloak is running `start-dev` on in-memory H2. Move to `start` with a real database (§5.2 item 1). |
| Account lockout no longer happens after 3 bad passwords | The legacy `FailedLoginAttempts` counter is not used. Enable Keycloak brute-force detection (§5.2 item 7). |
| No record of who signed in and when | `iAssetTrack_Sp_AuditLoginLogout` is not wired in the new stack. Enable Keycloak event logging and export it (§5.2 item 9). |
| A page returns 403 for one user but not another | Working as designed — legacy `(module, right)`. Grant the right on `/master/admin/group-rights`. Effective within ~60s. |
| **Every** call succeeds without a token | `Authorization:Enforce` is `false`. Set `Authorization__Enforce=true` on **all four** services — note that compose does not set it for identity-api. |
| Shell shows a blank page; console reports a federation error | `federation.manifest.json` points at unreachable remote URLs, or `remoteEntry.json` 404s. Fetch both by hand (§8.9). |
| Remote bundle blocked by CORS | The remotes are on a different origin than the shell and are not sending `Access-Control-Allow-Origin`. Add the header (§8.3), or move to a single origin. |
| Deep link (e.g. `/master/sites`) returns IIS 404 | The SPA fallback rewrite rule is missing, or URL Rewrite is not installed (§8.1, §8.3). |
| `.json` / `.woff2` requests 404 on IIS | Missing MIME map. Add the `staticContent` entries in §8.3. |
| Users see the previous release after a deploy | `index.html` / `remoteEntry.json` / `federation.manifest.json` are cached. Set `no-store` on them and purge the CDN (§11.5). |
| Import upload fails with 413 | Raise `maxAllowedContentLength` (IIS) or `proxy-body-size` (ingress/nginx) to ~50 MB. |
| Service starts then exits under IIS (500.30/500.31) | Missing connection string or Keycloak metadata. Enable `stdoutLogEnabled` temporarily and read `logs\stdout_*.log`. |
| HTTP 500.19 under IIS | `web.config` references the wrong module. Use `AspNetCoreModuleV2` (§8.4). |
| Deleting a tenant used to fail in the legacy app and now works | Expected — the legacy `Tenant_Delete` stored procedure is broken and the service uses a corrected soft delete (§6.3). |
| Keycloak realm changes disappear after a restart | Keycloak is running `start-dev` with an in-memory database. Move to `start` with a real database (§5.2). |
| Inventory **Commit** produced unexpected asset changes | Commit runs the legacy RFID `InventoryUpdate` stored procedure and is destructive and irreversible from the UI. Restore from database backup if needed; restrict the Inventory Update right. |

---

## 13. Related documents

- [User Guide](USER-GUIDE.md) — pages, modules and end-user workflows.
- Project [README](../README.md) — migration status, architecture decisions,
  per-service detail, and the list of deferred legacy pages.

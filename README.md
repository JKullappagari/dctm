# DCTrack Mobile REST API (DCTMRestAPI)

A REST API over the **DCTrack** data-center asset-management database. It exposes asset, location,
and reference (master-data) endpoints for mobile/stock-take clients, secured with JWT bearer tokens.

- **Runtime:** .NET 10 (ASP.NET Core)
- **Data:** SQL Server (`DCTrack`) via EF Core (database-first)
- **Auth:** JWT bearer
- **Docs:** Swagger / OpenAPI at `/api-docs`

---

## Solution layout

| Project | Type | Purpose |
|---------|------|---------|
| `DCTMRestAPI` | ASP.NET Core Web API | The REST API |
| `EncryptDecrypt` (`Cryptosettings.exe`) | Console tool | Encrypt/decrypt the connection string in `appsettings.json` |
| `DCTMRestAPI.UnitTests` | xUnit + Moq | Unit tests (logic classes + representative controllers) |
| `DCTMRestAPI.IntegrationTests` | xUnit + `WebApplicationFactory` | Integration tests (EF InMemory + real SQL Server) |
| `EncryptDecrypt.UnitTests` | xUnit | Tests for the console tool |

---

## Prerequisites

- **.NET 10 SDK**
- **SQL Server** (LocalDB, Express, or full) with the `DCTrack` database
- (optional) `dotnet dev-certs https --trust` to trust the local HTTPS dev certificate

### Restoring the database (local dev)
If you have a `.bak`, restore it into your instance, e.g. against SQL Server Express:

```sql
RESTORE DATABASE [DCTrack] FROM DISK = N'path\to\dctrack.bak'
WITH MOVE N'DCTrack'     TO N'<data-path>\DCTrack.mdf',
     MOVE N'DCTrack_log' TO N'<data-path>\DCTrack_log.ldf',
     RECOVERY, REPLACE;
```
(Use `RESTORE FILELISTONLY FROM DISK = N'...'` to see the logical file names, and
`SELECT SERVERPROPERTY('InstanceDefaultDataPath')` for the data path.)

---

## Configuration

Settings are read from `appsettings.json`, `appsettings.{Environment}.json`, user secrets (Development),
environment variables, then command line — later sources win.

| Key | Env-var form | Required | Notes |
|-----|--------------|----------|-------|
| `Jwt:SigningKey` | `Jwt__SigningKey` | **Yes** | ≥ 32 bytes. App **fails fast at startup** if missing/short. |
| `ConnectionStrings:DCTrackDatabase` | `ConnectionStrings__DCTrackDatabase` | **Yes** | Plaintext (contains `source`) or AES-encrypted (see the `EncryptDecrypt` tool). |
| `Security:UpgradePasswordHashesOnLogin` | `Security__UpgradePasswordHashesOnLogin` | No (default `false`) | Rehash legacy password hashes to PBKDF2 on successful login. Keep **off** until the main DCTrack app understands the new hash format (shared `tblUser`). |
| `PageConfiguration:PageSize` | — | No | Default page size for paged endpoints. |
| `Serilog:*` | — | No | Logging (console + rolling file under `./logs`). |

### Where to put secrets
- **Never** commit real secrets to `appsettings.json`.
- **Local dev:** user secrets (loaded automatically in the Development environment):
  ```powershell
  cd DCTMRestAPI
  dotnet user-secrets set "Jwt:SigningKey" "<32+ byte random value>"
  dotnet user-secrets set "ConnectionStrings:DCTrackDatabase" "data source=.\SQLEXPRESS;initial catalog=DCTrack;Integrated Security=True;TrustServerCertificate=True;"
  ```
  For convenience the `DCTMRestAPI` launch profile also carries **dev-only throwaway** values so the
  app runs out of the box; these are never used in Production.
- **Production:** environment variables or a secret store (e.g. Azure Key Vault). The Development-only
  fallbacks above are not consulted.

---

## Running

```powershell
cd DCTMRestAPI
dotnet run
```

Default local endpoints (from the launch profile):

- **HTTPS:** `https://localhost:7113`
- **HTTP:**  `http://localhost:13163`
- **Swagger UI:** `https://localhost:7113/api-docs`
- **OpenAPI JSON:** `https://localhost:7113/swagger/v1.2/swagger.json`

> In **Development**, both HTTP and HTTPS serve directly. In other environments the API **enforces
> HTTPS** (redirect + `RequireHttps`). The dev certificate is untrusted by default — either accept the
> browser warning or run `dotnet dev-certs https --trust`.

---

## Authentication

1. **Get a token** — `POST /api/auth/token` with a JSON body. The password is sent **plaintext over
   TLS** by default:

   ```bash
   curl -k -X POST https://localhost:7113/api/auth/token \
     -H "Content-Type: application/json" \
     -d '{"UserName":"Admin","Password":"<password>","DeviceID":""}'
   ```

   Response:
   ```json
   { "tokentype": "Bearer", "token": "<jwt>", "expiration": "...", "roles": "Public|Mobile" }
   ```

   Legacy clients that AES-encrypt the password may opt in with the header
   `X-Password-Encoding: aes` (the password value must then be the AES-encrypted string).

2. **Call protected endpoints** with the token:
   ```bash
   curl -k https://localhost:7113/api/countries -H "Authorization: Bearer <jwt>"
   ```

- Tokens are HS256, valid ~1 day, `roles` claim is `Public` or `Mobile` (device-registered).
- Some write endpoints require the `Mobile` role.
- Passwords are stored as PBKDF2 (new) or legacy salted-SHA256 (verified for backward compatibility).

---

## API surface

~68 controllers. Broadly:

- **Reference / master data** (read-mostly): countries, cities, sites, divisions, business units,
  manufacturers, asset models, statuses, orientations, connector types, etc.
  `GET /api/{resource}`, `GET /api/{resource}/{id}`, `GET /api/{resource}/updated/{lastUpdatedTime}`.
- **Assets:** `GET /api/assets` (stored-proc export), `GET /api/assets/pages` (paged, `X-Pagination`
  header), `GET /api/assets/{id}`, `GET /api/assets/tag/{tag}`, `GET /api/assets/search`,
  `GET /api/assets/search/csv`, `PATCH /api/assets/{id}` (JSON Patch → stored procedure).
- **Locations:** CRUD + `PATCH /api/locations/{id}`.
- **Mobile workflows:** check-out sessions/items/purposes, stock-take sessions/items, status history,
  asset transaction logs, log upload (`multipart/form-data`).
- **Auth:** `POST /api/auth/token`.

Full, always-current reference: **Swagger UI at `/api-docs`.**

---

## Database & mapping

- **EF Core, database-first.** The `DCTrackContext` (`Models/DCTrackContext.cs`) is scaffolded from the
  `DCTrack` schema (~87 tables). Read queries use `AsNoTracking` + async.
- Some tables have **triggers** (e.g. `tblUser`); writes to those use raw parameterized SQL because
  EF Core's `UPDATE … OUTPUT` is rejected by SQL Server on trigger tables.
- **Object mapping:** [Mapperly](https://github.com/riok/mapperly) (source-generated `DctmMapper`).
- **JSON:** Newtonsoft.Json, camelCase.

### Connection-string encryption tool (`EncryptDecrypt`)
Encrypts/decrypts the `DCTrackDatabase` value in `appsettings.json` (run from the folder containing it):

```powershell
Cryptosettings -pe ConnectionStrings -key DCTrackDatabase   # encrypt
Cryptosettings -pd ConnectionStrings -key DCTrackDatabase   # decrypt
```
At startup the API decrypts the value automatically if it isn't plaintext.
> The encryption uses an in-source key and is being retired — see
> [`docs/security-key-migration-plan.md`](docs/security-key-migration-plan.md). Prefer plaintext in a
> secret store.

---

## Testing

```powershell
dotnet test                      # whole solution
dotnet test DCTMRestAPI.UnitTests\DCTMRestAPI.UnitTests.csproj
```

- **Unit tests** — hashing, JWT config, paging, mapping, and representative controllers (EF InMemory,
  Moq for `ILogger`/`IConfiguration`/`IWebHostEnvironment`).
- **Integration tests** — boot the app via `WebApplicationFactory`:
  - Always-on suite uses the **EF InMemory** provider (routing, auth, async reads).
  - SQL-Server-backed tests (`[SkippableFact]`) run against a **real `DCTrack`** database
    (SQL translation, stored procedures, `IDENTITY_INSERT`, and login/rehash). They **skip
    automatically** when the DB isn't reachable, and clean up any rows they create.
  - Point them at a specific server with `DCTM_TEST_SQL` (defaults to `.\SQLEXPRESS`).
    **Use a throwaway database — never a shared/production one.**

---

## Conventions & operational notes

- **Startup:** `Program.cs` (minimal hosting) wires in the classic `Startup` class.
- **Logging:** Serilog to console + daily rolling files in `./logs`.
- **Security headers:** `NetEscapades.AspNetCore.SecurityHeaders`.
- **Reverse proxy / IIS:** `UseForwardedHeaders` (X-Forwarded-For/Proto) is enabled so HTTPS detection
  and client IP are correct behind a TLS-terminating proxy.
- **Secrets & keys:** JWT signing key, connection string, and password hashing are covered by the
  security migration plan in [`docs/security-key-migration-plan.md`](docs/security-key-migration-plan.md).

---

## Troubleshooting

| Symptom | Cause / fix |
|---------|-------------|
| Startup crash: *"JWT signing key is missing or too short"* | `Jwt:SigningKey` not provided. Set it via user secrets, `Jwt__SigningKey` env var, or the launch profile. This fail-fast is intentional. |
| `POST /api/auth/token` → *"Token validation failed"* with a Base-64 error | You sent a plaintext password **with** the `X-Password-Encoding: aes` header (which expects an AES-encrypted value). Omit the header for plaintext, or send an AES-encrypted password with it. |
| Swagger unreachable / HTTP redirects to an unbound port | Use the **HTTPS** URL, or run in the Development environment where both schemes serve directly. |
| Browser "Not secure" warning on `https://localhost:7113` | Untrusted dev cert — run `dotnet dev-certs https --trust`. |
| SQL integration tests all skipped | The `DCTrack` database isn't reachable at `.\SQLEXPRESS` (or `DCTM_TEST_SQL`). |

# DCTrack Mobile REST API — User Guide

Audience: developers and integrators calling the API (mobile stock-take clients,
scripts, third-party systems).

- **Product:** DCTMRestAPI — REST API over the **DCTrack** data-centre asset database
- **Version:** 1.2 (assembly `1.2.0.0`, OpenAPI doc `v1.2`)
- **Runtime:** .NET 10 / ASP.NET Core
- **Auth:** JWT bearer (HS256)
- **Serialization:** JSON (Newtonsoft.Json, camelCase)
- **Interactive reference:** Swagger UI at `/api-docs`

---

## 1. Quick start

### 1.1 Base URLs

| Environment | Base URL |
|---|---|
| Local dev (HTTPS) | `https://localhost:7113` |
| Local dev (HTTP) | `http://localhost:13163` |
| IIS-hosted (typical) | `https://<host>/dctmrest` |
| Container | `http://<host>:8080` (behind a TLS-terminating proxy) |

> Outside the `Development` environment the API **requires HTTPS** — plain HTTP is
> redirected and `RequireHttps` rejects non-TLS requests. Always call the HTTPS URL.

### 1.2 Get a token

```bash
curl -X POST https://localhost:7113/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{ "UserName": "Admin", "Password": "<password>", "DeviceID": "" }'
```

Request body (`TokenModel`):

| Field | Type | Required | Notes |
|---|---|---|---|
| `UserName` | string | yes | Matches `tblUser.LoginName`; the account must be active (`Status = true`). |
| `Password` | string | yes | Plaintext over TLS by default. |
| `DeviceID` | string | no | A device id already registered in DCTrack. Supplying a valid one grants the `Mobile` role. |

Response `200 OK`:

```json
{
  "tokentype": "Bearer",
  "token": "eyJhbGciOiJIUzI1NiIs…",
  "expiration": "2026-07-20T09:14:22",
  "roles": "Mobile"
}
```

Failure responses: `400 Bad Request` with `"Invalid user name or password"`
(bad credentials) or `"Token validation failed"` (malformed request / decryption error).

**Legacy AES-encrypted passwords.** Older clients that encrypt the password with the
shared symmetric key opt in per request with a header:

```
X-Password-Encoding: aes
```

With that header the `Password` value **must** be the AES-encrypted string. Sending a
plaintext password together with the header produces `"Token validation failed"` (a
Base-64 decode error). Omit the header for plaintext. New clients should not use it —
the shared key is being retired.

### 1.3 Call a protected endpoint

Every endpoint except `POST /api/auth/token` requires the bearer token:

```bash
curl https://localhost:7113/api/Country \
  -H "Authorization: Bearer <jwt>"
```

---

## 2. Authentication and authorisation

| Property | Value |
|---|---|
| Algorithm | HS256, symmetric signing key (`Jwt:SigningKey`) |
| Lifetime | ~1 day from issue |
| Clock skew allowed | 5 minutes |
| Issuer / audience validation | **Disabled** (signature + lifetime only) |
| `roles` claim | `Public` or `Mobile` |

**Roles**

- **`Public`** — issued when no `DeviceID` (or an unrecognised one) is supplied.
  Grants read access across the API.
- **`Mobile`** — issued when `DeviceID` matches a registered device. Required by the
  write endpoints that record field activity (asset writes, check-out, stock-take,
  status history, transaction logs, log upload, enclosure/rack positions, deleted rows,
  mobile-device queries).

Calling a `Mobile`-only endpoint with a `Public` token returns `403 Forbidden`. Calling
any protected endpoint without a token (or with an expired one) returns `401 Unauthorized`.

**Passwords at rest** are PBKDF2 (current) or legacy salted SHA-256 (still verified, for
backward compatibility). Optional rehash-on-login is a server setting — see the Admin Guide.

---

## 3. Conventions shared by all endpoints

### 3.1 Route shape

Routes are `/api/{ControllerName}` — the controller name is case-insensitive in URLs
(`/api/country` and `/api/Country` both work). The canonical names are listed in §4.

### 3.2 The three standard read operations

Most reference/master-data resources expose the same trio:

| Operation | Route | Description |
|---|---|---|
| List all | `GET /api/{resource}` | Every non-deleted row. |
| Get one | `GET /api/{resource}/{id}` | A single row by primary key. Returns an empty collection when not found. |
| Delta sync | `GET /api/{resource}/updated/{lastUpdatedTime}` | Rows modified **after** the given time. |

**`lastUpdatedTime` is a Unix timestamp in seconds** (a `long`), not an ISO date. This
is the mechanism mobile clients use to sync incrementally: store the server time from
`GET /api/ServerProperties/currentdatetime/unixtimestamp` before a sync, then pass it
on the next one.

```bash
# everything changed since 2026-01-01T00:00:00Z
curl https://…/api/Manufacturers/updated/1767225600 -H "Authorization: Bearer $JWT"
```

### 3.3 Writes

- `POST /api/{resource}` — create (many controllers accept a **list** of entities in one call).
- `PUT /api/{resource}/{id}` — full replace.
- `DELETE /api/{resource}/{id}` — delete (soft delete on most tables).
- `PATCH` — only on `Assets` and `Locations`; JSON Patch, see §5.

Not every resource supports every verb. §4 lists what each one exposes.

### 3.4 Status codes

| Code | Meaning |
|---|---|
| `200` | Success (body returned). |
| `204` | Success, no content — common for `PUT`/`PATCH`. |
| `400` | Invalid request body, failed model validation, or a stored-procedure rejection. |
| `401` | Missing, expired, or invalid token. |
| `403` | Valid token, but the endpoint requires the `Mobile` role. |
| `404` | Route or record not found. |
| `500` | Unhandled server error (see server logs). |

### 3.5 Security headers

Responses carry a standard hardening header set (`X-Content-Type-Options`,
`X-Frame-Options`, `Referrer-Policy`, `Strict-Transport-Security`, …). Clients need no
special handling; browser-based callers should expect framing to be denied.

---

## 4. Endpoint reference

Legend for the **Verbs** column: `L` = list all, `G` = get by id,
`U` = `updated/{unixTime}` delta, `P` = POST, `T` = PUT, `D` = DELETE, `X` = PATCH.
🔒 marks endpoints (or verbs) that require the **`Mobile`** role.

### 4.1 Authentication

| Endpoint | Verb | Auth | Description |
|---|---|---|---|
| `/api/auth/token` | `POST` | anonymous | Exchange username + password (+ optional device id) for a JWT. §1.2. |

> A previously available `EncryptString` helper has been **removed** — it exposed the
> shared symmetric key to any caller. Do not expect it to come back.

### 4.2 Assets

| Endpoint | Description |
|---|---|
| `GET /api/Assets` | Full asset export via stored procedure. Large payload — prefer `/pages` or `/search`. |
| `GET /api/Assets/pages?pageNumber=&pageSize=` | Paged list. Defaults `pageNumber=1`, `pageSize=10` (server default configurable). Paging metadata is returned in the **`X-Pagination`** response header as JSON: `{ "totalItems", "pageNumber", "pageSize", "totalPages" }`. The body also carries `paging`, `links` (first/prev/next/last) and `items`. |
| `GET /api/Assets/{assetId}` | One asset by numeric id. |
| `GET /api/Assets/tag/{assetTag}` | Look up by asset tag — the primary scanner lookup. |
| `GET /api/Assets/updated/{unixTime}` | Assets changed since the given Unix timestamp. |
| `GET /api/Assets/search?…` | Filtered search returning the flattened export shape. Query fields: `site`, `location`, `custodian`, `assettype`, `tagno`, `manufacturer`, `assetmodel`, `hostname`, `serialno`, `assetname`. All optional; supplied values are combined. |
| `GET /api/Assets/search/csv?…` | Same filters, returned as a CSV download. |
| `PUT /api/Assets` 🔒 | Bulk update — body is a **list** of asset objects. |
| `PATCH /api/Assets/{assetId}` | Partial update, JSON Patch (RFC 6902). See §5. |

Example paged call:

```bash
curl -i "https://…/api/Assets/pages?pageNumber=2&pageSize=50" \
  -H "Authorization: Bearer $JWT"
# → X-Pagination: {"totalItems":8412,"pageNumber":2,"pageSize":50,"totalPages":169}
```

### 4.3 Locations

| Endpoint | Description |
|---|---|
| `GET /api/Locations` | All locations. |
| `GET /api/Locations/{locationId}` | One location. |
| `GET /api/Locations/updated/{unixTime}` | Delta. |
| `PUT /api/Locations` 🔒 | Bulk update (list body). |
| `PATCH /api/Locations/{locationId}` | Partial update, JSON Patch. |

### 4.4 Mobile field workflows

These are the endpoints a handheld/stock-take client drives. Writes require `Mobile`.

| Resource | Verbs | Description |
|---|---|---|
| `/api/CheckOutSessions` | L G U P🔒 | Check-out sessions opened on a device. |
| `/api/CheckOutItems` | L G U P🔒 | Items scanned into a check-out session. `GET /{checkOutSessionId}` returns a session's items. |
| `/api/CheckOutPurposes` | L G U P🔒 T🔒 | Reasons available for a check-out. |
| `/api/StockTakeSessions` | L G U P🔒 | Stock-take (inventory) sessions. |
| `/api/StockTakeItems` | L G U P🔒 | Items scanned in a stock-take session. `GET /{stockTakeSessionId}` returns a session's items. |
| `/api/StatusHistory` | L G U P🔒 | Asset status-change history entries. |
| `/api/AssetTransLogs` | L G U P🔒 | Asset transaction log — the audit trail of field movements. |
| `/api/BarredHistory` | L G U P D | History of barred/unbarred assets. |
| `/api/DeletedRows` | L G U P🔒 | Tombstones so offline clients can purge locally-cached rows. |
| `/api/MobileDevices` | L🔒 G🔒 U🔒 P D | Registered devices. Reads require `Mobile`. |
| `/api/EnclPositions` | L G U T🔒 | Blade positions inside an enclosure. |
| `/api/RackPositions` | L G U T🔒 | U-positions inside a rack. |
| `/api/LogUpload` | P🔒 | Upload a device log file. `multipart/form-data` with fields `LogFile` (the file), `FileName`, `DeviceId`. |
| `/api/AuditCycles` | L G U P D | Audit-cycle definitions (site/location + start/end dates). |

Log upload example:

```bash
curl -X POST https://…/api/LogUpload \
  -H "Authorization: Bearer $MOBILE_JWT" \
  -F "LogFile=@device-2026-07-19.log" \
  -F "FileName=device-2026-07-19.log" \
  -F "DeviceId=HH-0042"
```

### 4.5 Reference / master data

All follow the standard trio (§3.2) plus the write verbs shown.

**Geography & organisation**

| Resource | Verbs | Description |
|---|---|---|
| `/api/WorldRegions` | L G U P T D | World regions. |
| `/api/Country` | L G U P T D | Countries. |
| `/api/City` | L G U P T D | Cities. |
| `/api/Sites` | L G U P T D | Data-centre sites. |
| `/api/BusinessUnit` | L G U P D | Business units. |
| `/api/Division` | L G U P T D | Divisions. |
| `/api/Owners` | L G U P T D | Owners / custodians. |
| `/api/Tenants` | L G U | Tenants (read-only here). |
| `/api/Users` | L G U P T D | User accounts. |
| `/api/UserPassword` | L G U P T D | User password records. |
| `/api/UserBusinessUnti` | L G U P T D | User → business-unit links. *(Route name carries a legacy spelling — `UserBusinessUnti`, not `…Unit`.)* |
| `/api/Groups`-related rights are **not** exposed by this API. | | |

**Asset catalogue**

| Resource | Verbs | Description |
|---|---|---|
| `/api/Manufacturers` | L G U P D | Manufacturers. |
| `/api/AssetModels` | L G U P D | Asset models. |
| `/api/BladeModels` | L G U P T D | Blade-specific model details. |
| `/api/EnclModels` | L G U P D | Enclosure-specific model details. |
| `/api/AssetGroups` | L G U P D | Asset groups / types. |
| `/api/TechCategories` | L G U P T D | Technology categories. |
| `/api/EntityTypes` | L G U P D | Entity types. |
| `/api/MountTypes` | L G U P D | Mount types. |
| `/api/AFDirections` | L G U | Air-flow directions (read-only). |
| `/api/Orientations` | L G U P D | Asset orientations. |
| `/api/InputConnectorTypes` | L G U P D | Input connector types. |
| `/api/OutputConnectorTypes` | L G U P T D | Output connector types. |
| `/api/UOM` | L U P T D | Units of measure. *(No get-by-id.)* |
| `/api/LocationTypes` | L G U P D | Location types. |
| `/api/Hosts` | L G U P D | Hosts. |
| `/api/StatusMaster` | L G U P T D | Asset status master list. |
| `/api/TransactionTypes` | L G U P T D | Transaction types. |
| `/api/MusterReasons` | L G U P D | Muster reasons. |
| `/api/Purposes` | L G U P T D | Purposes. |
| `/api/Messages` | L G U P D | Message catalogue (codes → text). |
| `/api/CheckDelete` | L G U P D | Referential-integrity check metadata for deletes. |

**Applications**

| Resource | Verbs | Description |
|---|---|---|
| `/api/Applications` | L G U P D | Applications. |
| `/api/ApplTypes` | L G U P T D | Application types. |
| `/api/AppCriticalities` | L G U P T D | Application criticalities. |
| `/api/AppStatuses` | L G U P D | Application statuses. |
| `/api/ApplicationMaps` | L G U | Application → asset/host map (read-only). |

**Assignments**

| Resource | Verbs | Description |
|---|---|---|
| `/api/BuDivAssignment` | L G U P D | Business unit ↔ division. |
| `/api/BuSiteAssignment` | L G U P D | Business unit ↔ site. |
| `/api/SiteLocAssignment` | L G U P T D | Site ↔ location. |
| `/api/OwnerDivAssignment` | L G U P T D | Owner ↔ division. |
| `/api/AssetHostAssignments` | L G U | Asset ↔ host (read-only). |
| `/api/TenantAssetAssignments` | L G U | Tenant ↔ asset (read-only). |
| `/api/TenantApplicationAssignments` | L G U | Tenant ↔ application (read-only). |
| `/api/TenantDivisionAssignments` | L G U | Tenant ↔ division (read-only). |
| `/api/TenantGroupAssignments` | L G U | Tenant ↔ group (read-only). |
| `/api/TenantHostAssignments` | L G U | Tenant ↔ host (read-only). |
| `/api/TenantLocationAssignments` | L G U | Tenant ↔ location (read-only). |
| `/api/TenantOwnerAssignments` | L G U | Tenant ↔ owner (read-only). |

### 4.6 Server utilities

| Endpoint | Description |
|---|---|
| `GET /api/ServerProperties/currentdatetime/utc` | Server time, UTC. |
| `GET /api/ServerProperties/currentdatetime/localzone` | Server time in the host's local zone. |
| `GET /api/ServerProperties/currentdatetime/unixtimestamp` | Server time as a Unix timestamp — **use this as the watermark for `updated/{lastUpdatedTime}` calls.** |

> `/api/Downloads` exists as a controller but has **no active routes** in this version.

---

## 5. Partial updates with JSON Patch

`PATCH /api/Assets/{assetId}` and `PATCH /api/Locations/{locationId}` follow
[RFC 6902](https://datatracker.ietf.org/doc/html/rfc6902). Only the `replace`
operation is supported.

Patchable asset properties: `assetname`, `hostname`, `assettag`, `serialnumber`,
`parentassetid`, `uposition`, `orientation`, `locationid`, `externalid`.

```bash
curl -X PATCH https://…/api/Assets/10432 \
  -H "Authorization: Bearer $JWT" \
  -H "Content-Type: application/json" \
  -d '[{ "op": "replace", "path": "/serialnumber", "value": "TestSerialNo" }]'
```

**Moving an asset** is a compound change — a location update must supply
`orientation`, the new `locationid` **and** `uposition` in the same patch document.
For a blade, also include `parentassetid`:

```json
[
  { "op": "replace", "path": "/locationid",    "value": 88 },
  { "op": "replace", "path": "/uposition",     "value": 14 },
  { "op": "replace", "path": "/orientation",   "value": 1  },
  { "op": "replace", "path": "/parentassetid", "value": 10399 }
]
```

The patch is applied through a stored procedure, so DCTrack's own validation rules
(slot availability, uniqueness, orientation compatibility) still apply — a rejection
comes back as `400` with the message text.

---

## 6. Typical mobile sync flow

1. `POST /api/auth/token` with `UserName`, `Password`, `DeviceID` → `Mobile` token.
2. `GET /api/ServerProperties/currentdatetime/unixtimestamp` → record as `T`.
3. First run: pull each reference resource with `GET /api/{resource}`.
   Subsequent runs: `GET /api/{resource}/updated/{T_previous}`.
4. `GET /api/DeletedRows/updated/{T_previous}` → purge locally cached rows.
5. Work offline; look assets up by tag with `GET /api/Assets/tag/{tag}`.
6. Push field activity: `POST /api/StockTakeSessions`, `POST /api/StockTakeItems`,
   `POST /api/AssetTransLogs`, `POST /api/StatusHistory`, `PATCH /api/Assets/{id}`.
7. `POST /api/LogUpload` for device diagnostics.
8. Persist `T` as the new watermark.

Because the token lives ~1 day, long-running clients should re-authenticate on `401`
rather than caching the token indefinitely.

---

## 7. Interactive documentation

Swagger UI is served at **`/api-docs`**; the OpenAPI document is at
`/swagger/v1.2/swagger.json` (`/dctmrest/swagger/v1.2/swagger.json` for the
IIS-hosted release build).

To try secured endpoints from the UI: call `POST /api/auth/token`, copy the `token`
value, click **Authorize**, and enter `Bearer <token>`.

The Swagger document is generated from the code and XML comments, so it is always the
authoritative reference for request/response schemas — this guide describes the shape
and the workflow; Swagger describes the fields.

---

## 8. Troubleshooting

| Symptom | Cause / fix |
|---|---|
| `401` on every call | Missing/expired token, or `Authorization` header not formatted `Bearer <jwt>`. Tokens last ~1 day. |
| `403` on a write | The token has role `Public`. Re-authenticate supplying a registered `DeviceID` to obtain the `Mobile` role. |
| `POST /api/auth/token` → `"Token validation failed"` with a Base-64 error | Plaintext password sent **with** `X-Password-Encoding: aes`. Drop the header, or send an AES-encrypted value. |
| `POST /api/auth/token` → `"Invalid user name or password"` | Wrong credentials, or the `tblUser` row has `Status = false`. |
| Request redirected / connection refused on HTTP | Outside Development the API enforces HTTPS. Use the HTTPS URL. |
| Browser "Not secure" warning on `https://localhost:7113` | Untrusted dev certificate — run `dotnet dev-certs https --trust`. |
| `updated/{…}` returns everything | The parameter is a **Unix timestamp in seconds**, not an ISO date. Passing `0` returns all rows. |
| `GET /api/Assets` times out | It exports the whole asset table. Use `/api/Assets/pages` or `/api/Assets/search`. |
| Paging metadata missing | It is in the `X-Pagination` **response header** — many HTTP clients hide headers by default. |

---

## 9. Related documents

- [Admin Guide](ADMIN-GUIDE.md) — IIS and container deployment, configuration, operations.
- [Security key migration plan](security-key-migration-plan.md) — JWT signing key,
  connection-string encryption, and password-hash migration.
- Project [README](../README.md) — developer setup and testing.

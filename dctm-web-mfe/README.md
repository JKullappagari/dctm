# DCTrack Modernization

Strangler-fig migration of the legacy `DCTrackMobileWeb` ASP.NET Web Forms app
(see `../migration-inventory.md` for the full page-by-page plan) to
Angular micro-frontends + ASP.NET Core microservices on Linux containers.

## Fully containerized stack

`docker compose up --build` brings up everything on Linux containers:

| Service | Port | What |
|---|---|---|
| sqlserver | 1433 | legacy DB (restored from `../db`) |
| keycloak | 8180 | OIDC realm `dctrack` |
| identity-api | 8082 | Identity & Access (permissions/menu) |
| masterdata-api | 8081 | Master Data (13 pages) |
| asset-api | 8083 | Asset browse + lifecycle |
| masterdata-mfe | 8091 | Angular remote (nginx) |
| assets-mfe | 8092 | Angular remote (nginx) |
| **shell** | **8090** | **Angular host — open this** |

The MFEs are nginx static-bundle images (`frontend/Dockerfile.mfe` per project,
`Dockerfile.shell` for the host). Remotes send `Access-Control-Allow-Origin: *` so the
shell can load them cross-origin (module federation). The shell's federation manifest is
rewritten at build time to the browser-reachable remote ports (build args in compose).
Verified: all three serve 200, manifest + CORS correct, and the shell at :8090 completes
its OIDC redirect to Keycloak (the `:8090` redirect URI is registered in the realm).

## Layout

```
modern/
├── docker-compose.yml          # SQL Server + DB restore + services
├── db/restore-db.sh            # restores ../db/dctrack_25Aug2019.bak → [dctrack]
├── services/
│   └── master-data/            # Master Data Service (.NET 10) — FIRST MIGRATED MODULE
│       ├── src/MasterData.Api/
│       ├── tests/MasterData.Api.Tests/
│       └── Dockerfile
└── frontend/                   # (next) Nx workspace: shell + master-data MFE
```

## Master Data Service (first vertical slice)

Ports the legacy "Pattern A" CRUD pages by calling the **unchanged** legacy stored
procedures via Dapper. Entities migrated so far (legacy page → API):

| Legacy page | API base | Legacy SPs reused |
|---|---|---|
| `Manufacturer.aspx` | `/api/v1/manufacturers` | `iAssetTrack_Sp_Manufacturer_{List,Update,Delete,DoesExist}` |
| `LocationType.aspx` | `/api/v1/location-types` | `iAssetTrack_Sp_LocationType_{…}` |
| `Purpose.aspx` | `/api/v1/purposes` | `iAssetTrack_Sp_Purpose_{…}` |
| `MusterReason.aspx` | `/api/v1/muster-reasons` | `iAssetTrack_Sp_MusterReason_{…}` |
| `BusinessUnit.aspx` | `/api/v1/business-units` | `iAssetTrack_Sp_BusinessUnit_{…}` (Update needs non-defaulted `@pbitFromIA`) |
| `Division.aspx` | `/api/v1/divisions` | `iAssetTrack_Sp_Division_{…}` |
| `Owner.aspx` | `/api/v1/owners` | `iAssetTrack_Sp_Owner_{…}` (DoesExist checks first+last+email; list joins Division) |
| `TechnologyCategory.aspx` | `/api/v1/technology-categories` | `iAssetTrack_Sp_TechCat_{…}` |
| `AssetType.aspx` ("Asset Group") | `/api/v1/asset-groups` | `iAssetTrack_Sp_AssetGroup_{…}` |

| `Sites.aspx` | `/api/v1/sites` | `iAssetTrack_Sp_Site_{…}` + geo lookups below |
| `Location.aspx` | `/api/v1/locations` | `iAssetTrack_Sp_Location_{…}` (LocationType FK + self-referential Parent; list SP returns hierarchical Path) |
| `Host.aspx` / `HostPopup.aspx` | `/api/v1/hosts` | `iAssetTrack_Sp_Host_{…}` — **GUID-keyed**, see below |
| `AssetModel.aspx` | `/api/v1/asset-models` | `iAssetTrack_Sp_AssetModel_{…}` — 6 FK selects (mfg, model-type, tech, BU, mount, airflow) + numeric fields; ~25 physical params default. FK options come from the entity endpoints plus read-only `/api/v1/ref/{mount-types,airflow-directions,orientations,input-connector-types,output-connector-types}`. |

Geo lookups for the cascading Region → Country → City site form:
`GET /geo/regions` (no legacy SP exists — direct read on tblCountry),
`GET /geo/countries?region=`, `GET /geo/cities?countryId=` (legacy SPs), and
`GET /sites/{id}/geo` (country/city ids the list SP omits, needed to preselect on edit).

**Host is GUID-keyed** so it can't ride the int-based generic machinery — it has a
dedicated `Features/Hosts.cs` (still reuses `ExcelExport` and `Audit`). Two DB-specific
gotchas, both verified: `Host_Update`'s id param is an nvarchar output GUID, and
`Host_Delete` concatenates the id CSV into dynamic SQL, so ids must be **quoted GUIDs**
(`'guid','guid'`) — the endpoint parses each to `Guid` first, which also closes the
SQL-injection hole in that SP.

| `Tenant.aspx` | `/api/v1/tenants` | `iAssetTrack_Sp_Tenant_{List,Update,DoesExist}` — core CRUD (the location-tree / permissions sub-system is deferred) |

**Dual-list assignment pages** (`Assignments.cs`): `GET/PUT /api/v1/assignments/{bu-divisions|bu-sites|site-locations}/{parentId}`
returns assigned + available and replaces the assignment via the legacy delimited-list
Update SP. Frontend: one generic `AssignmentPage` (parent picker + two transfer lists),
verified end-to-end (unassign a site → save → DB reflects it → restore). `site-locations`
resolves the site's BU server-side (`ParentContextSql`) since its Update SP needs both;
verified the save preserves all 36 of Colo's location links.

Two **legacy defects surfaced and worked around**, both verified:
- `Tenant_Delete` filters `WHERE TechID` — a column that doesn't exist on tblTenant, so
  it always errored (in the legacy app too). Replaced with a correct parameterized
  soft-delete via the new `LookupSpMap.DeleteSqlOverride`.
- (earlier) `Host_Delete` needed quoted GUIDs / had an injection hole.

Deferred: `DeviceReg.aspx` / `Department` (no SPs in the DB), `GlobalParameters.aspx`
(SPC-specific, `Sno` key), `SiteLocationAssignment` (same dual-list UI but its Update SP
also needs the site's BU — a small backend extension), and the Tenant location-tree /
permissions sub-system. The generic machinery now supports composite DoesExist checks
(`AddExistsParams` — Owner, Location, Tenant), self-referential FK selects, and delete
overrides.

Each entity exposes: `GET /`, `GET /{id}`, `POST /` (409 on duplicate name — legacy
DoesExist SP), `PUT /{id}`, `DELETE /?ids=1,2,3` (legacy CSV soft delete), and
`GET /export` (xlsx via ClosedXML — replaces Infragistics WebExcelExporter).

> **Legacy DoesExist SP semantics** (verified against the restored DB, not what the
> BAL code suggests): returns `-1` when an active record with the name exists (block),
> `0` when the name is free, and the **soft-deleted record's id** when a deleted record
> has that name — in which case `POST` reactivates that row (legacy page behavior) and
> responds `{"id": <original id>, "reactivated": true}`. All four verified end-to-end
> against `dctrack_25Aug2019.bak` in the SQL Server 2022 Linux container.

Adding the next lookup entity (Sites, BusinessUnit, Division, Owner, TechnologyCategory,
AssetType, …) = one `Features/<Entity>.cs` file (row class + write DTO + `LookupSpMap`)
plus two lines in `Program.cs`.

### Run locally (no Docker)

```bash
cd services/master-data
dotnet test                                    # 10 tests, no DB needed
dotnet run --project src/MasterData.Api        # needs ConnectionStrings__MasterData
```

### Run the full stack (Docker)

```bash
cp .env.example .env       # set MSSQL_SA_PASSWORD
docker compose up --build  # restores the .bak, starts the API on :8081
curl http://localhost:8081/api/v1/manufacturers
```

## Conventions for subsequent services

- .NET 10 minimal APIs, Dapper against legacy SPs first (rewrite SQL later, not during migration).
- `X-User-Id` header stands in for the authenticated user until Keycloak JWT auth lands
  (single TODO in `LookupEndpoints.CurrentUserId`).
- `/healthz` liveness endpoint; OpenAPI served at `/openapi/v1.json`.
- One Dockerfile per service, non-root `app` user, port 8080 internally.

## Identity & Access (Keycloak + permissions API)

- **Keycloak** (`:8180`, realm `dctrack` auto-imported from `identity/dctrack-realm.json`):
  public PKCE client `dctm-shell` for the Angular shell; test users `admin` / `droy`
  matching legacy `tblUser.LoginName` values. Production: federate to AD/LDAP
  (replaces `ActiveDirectoryMembershipProvider`) and disable direct access grants.
- **Identity & Access Service** (`services/identity-access`, `:8082`): validates
  Keycloak JWTs (`preferred_username` = legacy `LoginName`) and reads the legacy
  security model (tblUser → tblGroupMember → tblGroupModuleRight → tblModuleRight →
  tblModule/tblRight — the data behind the Web Forms security-trimmed menu):
  - `GET /api/v1/me` — identity echo
  - `GET /api/v1/me/menu` — main-module → module tree the user may see
    (replaces `SqlSiteMapProvider`); includes each module's legacy PageURL so the
    shell can map it to the new route.
  - `GET /api/v1/me/permissions` — flat (module, right) pairs for route guards and
    API authorization policies. Legacy rights are fine-grained (Create/Modify/Delete/
    View/Bar/WriteOff/… — 31 kinds).

**Admin editors** (retire `GroupModuleRightsAssignment.aspx` + the group slice of
`ManageUsers.aspx`): `GET /api/v1/groups/{id}/module-rights` (full main-module → module →
rights matrix with granted flags), `PUT /groups/{id}/modules/{moduleId}/rights`
`{rightIds}` (legacy per-module delimited SP), `GET/PUT /users/{id}/groups` (legacy
`Sp_User_AssignRights`). Gated on holding a right on the legacy
"Group Module Access Rights Assignment" module (401/403 verified). Frontend pages:
`/master/admin/group-rights` (rights matrix with per-module dirty tracking) and
`/master/admin/user-groups`. The APIs' permission caches now expire after **60s**, so
rights edits take effect without restarts — verified live: granting Users→Manufacturer/View
flipped droy's API access 403→200, revoking flipped it back.

Token smoke test:

```bash
TOKEN=$(curl -s -X POST http://localhost:8180/realms/dctrack/protocol/openid-connect/token \
  -d grant_type=password -d client_id=dctm-shell -d username=admin -d 'password=DevOnly!Admin1' \
  | jq -r .access_token)
curl -s http://localhost:8082/api/v1/me/menu -H "Authorization: Bearer $TOKEN"
```

**Shell integration (done):** the shell uses `angular-auth-oidc-client` — unauthenticated
visits redirect to Keycloak (no local login page; retires `Login.aspx`), `authInterceptor`
attaches Bearer tokens to both APIs (`secureRoutes`), and the sidenav renders from
`/me/menu` with `LEGACY_ROUTE_MAP` translating tblModule PageURLs to migrated routes
(unmigrated modules show disabled). The master-data API accepts JWTs and resolves
`preferred_username` → `tblUser.UserID` for CreatedBy/LastModifiedBy audit columns
(`LegacyUserResolver`); the `X-User-Id` header remains only as an unauthenticated dev
fallback until `Auth` is enforced.

**MFA / SAML (later stage):** both are Keycloak configuration, no app changes —
enable OTP/TOTP or WebAuthn/passkeys per realm (or conditionally per group/role) for
MFA, and add the corporate IdP (Entra ID, ADFS, Okta, …) as a **SAML 2.0 identity
provider** in the realm for brokered SSO. Apps keep receiving the same OIDC JWTs.

**Authorization (done):** two layers off the legacy (module, right) model.
- **Server-side (the real gate)** — the master-data API has a `RequirePermission(module, right)`
  endpoint filter; each CRUD verb requires the matching legacy right (GET→View,
  POST→Create, PUT→Modify, DELETE→Delete) on the entity's `tblModule.Module`.
  Gated by `Authorization:Enforce` (env `Authorization__Enforce`), **now `true` in the
  containerized stack** for both master-data-api and asset-api. The asset API maps each
  lifecycle action to its legacy right on the **"Search Asset"** module (writeoff→WriteOff,
  bar→Bar, restrict→Restrict, decommission→Decommission, RFID→Assign/De-assign RFID,
  reads→View). The JWT is the only accepted identity — the `X-User-Id` dev fallback is
  disabled when enforcing. Verified end-to-end with real Keycloak tokens: no token→401 on
  both APIs, `admin`→200, `droy` (lacking the right)→403 (both a master-data delete and an
  asset restrict). Permissions read from the legacy tables via `PermissionRepository`
  (cached); centralize behind the identity service when the DBs split. Entities with no
  `tblModule` row (Business Unit, Location Type, Asset Group) are unguarded.
  **Consequence:** the tokenless standalone MFEs on :4201/:4202 now need a local API run
  with `Authorization:Enforce=false`; the containerized stack expects the shell (JWTs) as
  the entry point. A **fallback authorization policy** (when enforcing) requires a valid JWT
  on *every* endpoint — closing the gap for those without a module guard (assignments,
  Business Unit / Location Type / Asset Group, ref/geo). `/healthz` and OpenAPI opt out.
  Verified: assignments and business-units now 401 without a token.
- **Client-side (UX defense-in-depth)** — a `permissionGuard` on each master-data route
  blocks hand-typed URLs to modules the user has no right on, redirecting to
  `/forbidden`. It reads `/me/permissions` (cached) and **fails open** when the call
  can't be made (standalone :4201 with no token) — the API is the real enforcer.
  Each `LookupConfig` carries its legacy `module` name.

## Asset Service (lifecycle state machine)

`services/asset` (.NET 10, `:8083`) — asset browse + the lifecycle state machine.
- `GET /api/v1/assets?q=&page=&size=` — paged list (direct join; the 25-param legacy
  search SP is deferred), `GET /assets/{id}`, `GET /assets/{id}/status`.
- Lifecycle transitions (each → a legacy SP): `POST /{id}/{writeoff|reinstate|restrict|
  de-restrict|bar|un-bar|decommission|recommission}`, `POST/DELETE /{id}/rfid-card`.
- The **state machine** (`AssetLifecycle`) treats writeoff/restrict/bar/decommission/RFID
  as independent guarded toggles derived from tblAsset flags (IsWriteOff, IsMustered,
  IsPermRestrict, Barred date window, CurrentRFIDCardNumber). `/status` returns the
  active states + currently-valid actions; an invalid transition (e.g. re-restrict) → 409.
  Verified against real data: restrict→409-on-repeat→de-restrict, bar→un-bar, all flipping
  available actions correctly. 5 unit tests on the guard logic.

The **Assets MFE** (`:4202`, federated remote `assets`) is a grid + a lifecycle side
panel: select a row → see its states as chips and one button per available action; each
opens a reason/date dialog, POSTs the transition, and refreshes. Verified end-to-end in
the browser (restrict via the panel → chips/buttons update → de-restrict).

**Create Asset** (retires the core of `CreateAsset.aspx`): `POST /api/v1/assets` /
`PUT /assets/{id}` via legacy `iAssetTrack_Sp_Asset_UpdateNew` (34 params; hardware
detail params default). The API derives the asset group from the model and the BU from
the site (legacy page behavior), and surfaces the SP's `Result`/`MessageCode` outputs as
error text resolved from `tblMessage`. Uniqueness is whatever the legacy configurable
rule (`Udf_Get_CheckUniqueAssetCondition`) says — behavior preserved, not re-invented.
Gated on Create/Modify rights of the legacy **"Create Asset"** module. Frontend:
`/assets/new` — manufacturer→model cascade, site→location cascade (locations assigned
to the site), owner/orientation/rack fields. Verified: API create round-trip against the
real SP (asset created, names resolved, lifecycle active, then removed), form renders
with all 11 fields.

**Import Asset / Import Blades** (`ImportAsset.aspx`, `ImportBlades.aspx`): xlsx wizards
sharing `Asset_UpdateNew` via a reusable `UpsertCoreAsync`. Resolve name columns to ids
(Site, Rack-within-site, Manufacturer+Model, Owner; blades resolve the parent enclosure
by tag and inherit its site/location), insert per row, return per-row results. Import into
EXISTING locations — the legacy Racks-sheet location-creation step is out of scope.
Gated on the "Import Asset" / "Import Blades" rights. Verified end-to-end: valid rows
imported (incl. an HP BL460C blade into an HP C7000 enclosure), unknown site/rack/parent
and invalid blade/enclosure combos each reported per-row. Frontend: `/assets/import/{assets,blades}`
(one generic upload+results component), template download per type.

**Inventory Update** (`InventoryUpdate.aspx`): RFID stock-take reconciliation. `GET
/api/v1/inventory?locations=` lists stock-take sessions (from `tblStockTakeSession`,
populated by handheld RFID scans) with asset/scanned/missing/over-scanned counts;
`POST /inventory/sessions/{guid}/commit` reconciles a session's un-processed scanned
assets via the legacy `InventoryUpdate` SP (destructive — moves/marks assets; gated on
the Inventory Update right). Verified the read path against real sessions; the commit is
wired but **not exercised in verification** since it mutates asset records via the
RFID-coupled SP. Frontend: `/assets/inventory` (location picker → sessions grid → per-row
Commit). Note: sessions only appear where RFID scans exist; the scanning pipeline itself
is the mobile-device side, out of this scope.

## Frontend (Angular micro-frontends)

`frontend/` is an Angular 22 workspace with two applications wired via
**native federation** (`@angular-architects/native-federation`):

- **`shell`** (:4200) — host app: sidenav layout, loads remote routes from the
  federation manifest (`projects/shell/public/federation.manifest.json`). The nav
  is static for now; it becomes permission-driven when the Identity Service lands.
- **`master-data`** (:4201) — remote MFE exposing `./routes`. One generic
  `LookupPage` (AG Grid Community + Material dialog) renders every legacy
  "Pattern A" page from a `LookupConfig` entry in
  `projects/master-data/src/app/entities.ts`. Four configs currently retire
  `Manufacturer.aspx`, `LocationType.aspx`, `Purpose.aspx`, `MusterReason.aspx`.
  Adding the next page = one config entry (no new components).

```bash
cd frontend
npx ng serve master-data --port 4201   # remote (also runs standalone)
npx ng serve shell --port 4200         # host — open http://localhost:4200
```

Notes:
- Grid: AG Grid Community (sorting/filtering/paging/multi-select included) —
  replaces Infragistics WebDataGrid. Export button streams the API's ClosedXML xlsx.
- `@angular/animations` is deprecated in Angular 20.2+ and has no v22 release —
  Material runs on native CSS animations; do not add `provideAnimationsAsync`.
- The API allows CORS from :4200/:4201 via `Cors:Origins` config.

## Next increments

1. Remaining master-data entities (Sites, BusinessUnit, Division, Owner, …) —
   one API feature file + one `entities.ts` config each.
2. Keycloak + permissions API (replaces `SqlSiteMapProvider` security trimming);
   swap the `X-User-Id` header for JWT claims.
3. Asset Service (lifecycle state machine) + Assets MFE.
4. Dockerize the MFEs (nginx static bundles) and add them to docker-compose.

# DCTrack Web (Modernized) — User Guide

Audience: end users of the modernized DCTrack web application — asset administrators,
data-centre operators, and application owners.

This application replaces the legacy `DCTrackMobileWeb` ASP.NET Web Forms site. It is
built as an **Angular micro-frontend (MFE)** shell hosting two remote applications
(**Master Data** and **Assets**) backed by four ASP.NET Core microservices. Sign-in is
handled by **Keycloak** (OIDC single sign-on).

---

## 1. Getting started

### 1.1 Opening the application

Open the **shell** URL supplied by your administrator — the default local address is:

```
http://localhost:8090
```

Everything is reached through the shell. The individual micro-frontends (ports 8091 /
8092, or 4201 / 4202 in development) are internal build artifacts and are not meant to be
opened directly — they have no sign-in and will show empty pages against a secured API.

### 1.2 Signing in

There is **no login form in the application**. Visiting the shell while unauthenticated
redirects you to the central sign-in page (Keycloak) for the `dctrack` realm. Sign in with
the account your administrator has given you — your sign-in name is the same `LoginName`
you used in the legacy DCTrack application. After a successful sign-in you are returned to
the page you requested.

Depending on how your organisation has configured sign-in, you may see a corporate
single-sign-on page instead, or be signed in automatically on a domain-joined PC.

Notes:

- The session refreshes silently in the background; you should not be interrupted while
  working.
- Multi-factor authentication and corporate SSO (Entra ID, ADFS, Okta) can be enabled by
  your administrator without any change to the application — if your organisation has
  them turned on, you will see those prompts on the Keycloak page.
- Signing out returns you to the shell's start page and clears the session.

### 1.3 The layout

| Area | What it is |
|---|---|
| **Side navigation** | Built from *your* permissions. Only modules you hold a right on are listed; modules that have not yet been migrated appear disabled. |
| **Main area** | The page you selected. |

Because the menu is permission-driven, two users may see different menus. If a page you
expect is missing, you lack the right on that legacy module — ask an administrator (see
§7).

### 1.4 If you land on "Forbidden"

Typing a URL directly for a module you have no right on redirects you to a **Forbidden**
page. This is expected — the server enforces the same rule, so there is nothing to work
around; request the right instead.

---

## 2. Working with master-data pages

Twenty-one master-data pages share the same screen design, so learning one teaches you
all of them. They live under `/master/…`.

### 2.1 The list screen

Every master-data page shows a data grid with:

- **Sorting** — click a column header; click again to reverse.
- **Filtering** — per-column filters in the header menu.
- **Paging** — page size and navigation at the bottom.
- **Multi-select** — checkboxes for selecting several rows.
- **New** — opens the create dialog.
- **Edit** — opens the edit dialog for the selected row (or double-click the row).
- **Delete** — deletes the selected row(s). This is a *soft* delete: the record is
  retired, not physically removed.
- **Export** — downloads the current entity as an **`.xlsx`** file.

### 2.2 Create and edit dialog

Fields are rendered from the entity definition and include text boxes, multi-line
descriptions, numbers, dates, checkboxes, and dropdowns. Dropdowns are populated from the
related master data (for example, *Asset Model* offers Manufacturer, Model Type,
Technology Category, Business Unit, Mount Type and Air Flow Direction). Some forms
**cascade**: on the *Sites* page, choosing a Region filters Countries, and choosing a
Country filters Cities.

Required fields are marked and validated before the dialog will save. Maximum lengths and
numeric minimums are enforced client-side and again by the server.

### 2.3 Duplicate names and reactivation

Names must be unique per entity. Three outcomes are possible when you save a new record:

| Situation | What happens |
|---|---|
| The name is free | The record is created. |
| An **active** record already has that name | Save is rejected with a duplicate-name error. Choose another name. |
| A **soft-deleted** record has that name | The existing record is **reactivated** rather than a duplicate created. The screen reports the record as reactivated and it reappears in the list with its original identifier. |

The third case is deliberate — it preserves the legacy application's behaviour and keeps
historical references to that record intact.

### 2.4 Master-data pages

| Page | Route | Replaces (legacy page) | Notes |
|---|---|---|---|
| Manufacturers | `/master/manufacturers` | `Manufacturer.aspx` | Name (max 25 chars) + description. |
| Asset Models | `/master/asset-models` | `AssetModel.aspx` | Six lookups (manufacturer, model type, technology category, business unit, mount type, air flow), U height, max power (W), blade/enclosure flags. |
| Asset Groups | `/master/asset-groups` | `AssetType.aspx` | Called "Asset Type" in the legacy app. |
| Technology Categories | `/master/technology-categories` | `TechnologyCategory.aspx` | |
| Sites | `/master/sites` | `Sites.aspx` | Cascading Region → Country → City. |
| Locations | `/master/locations` | `Location.aspx` | Location type, self-referencing parent location (the grid shows the full hierarchical path), tag id, floor, exit-door and check-out-location flags. |
| Location Types | `/master/location-types` | `LocationType.aspx` | Storage-type and RFID-location flags. |
| Hosts | `/master/hosts` | `Host.aspx` | Identified by GUID rather than a number. |
| Business Units | `/master/business-units` | `BusinessUnit.aspx` | Name (max 50) + company prefix (max 5). |
| Divisions | `/master/divisions` | `Division.aspx` | |
| Owners / Custodians | `/master/owners` | `Owner.aspx` | Uniqueness is first name + last name + email; the list shows the owner's division. |
| Tenants | `/master/tenants` | `Tenant.aspx` | Core details (names, type, contacts, user count). The tenant location-tree / permissions sub-system is **not** migrated. |
| Users | `/master/users` | `UserSearch.aspx` | |
| Groups | `/master/groups` | `Group.aspx` | Security groups. |
| Devices | `/master/devices` | `DeviceReg.aspx` | Handheld device registration — device id, name, site. |
| Audit Cycles | `/master/audit-cycles` | `AuditCycle.aspx` | Site + room, start and end date. |
| Purposes | `/master/purposes` | `Purpose.aspx` | |
| Muster Reasons | `/master/muster-reasons` | `MusterReason.aspx` | |
| Applications | `/master/applications` | `CreateApplication.aspx` | Business unit, application type, criticality, owner, status. |
| Application Types | `/master/application-types` | `ApplicationType.aspx` | |
| Application Criticalities | `/master/application-criticalities` | `ApplicationCriticality.aspx` | Includes background/foreground colour codes used to highlight rows. |

---

## 3. Assignment pages

Assignment pages link two sets of records using a **dual-list transfer** screen: pick the
parent record at the top, then move items between *Available* and *Assigned* with the
transfer buttons and click **Save**. Saving replaces the whole assignment set for that
parent.

| Page | Route | Replaces | What it assigns |
|---|---|---|---|
| Business Unit → Divisions | `/master/assign/bu-divisions` | `BUDivAssignment.aspx` | Divisions belonging to a business unit. |
| Business Unit → Sites | `/master/assign/bu-sites` | `BUSiteAssignment.aspx` | Sites belonging to a business unit. |
| Site → Locations | `/master/assign/site-locations` | `SiteLocationAssignment.aspx` | Locations belonging to a site. |
| Application Map | `/master/assign/app-map` | `ApplicationMap.aspx` | Applications mapped to a host. |

Nothing is written until you click Save, so you can move items back and forth freely
first.

---

## 4. Bulk import (spreadsheets)

Import pages let you create many records at once from an Excel workbook. They all follow
the same three steps:

1. **Download the template** — click the template link on the page. It contains the exact
   column headers expected.
2. **Fill one record per row.** Related records are matched **by name**, not by id — type
   the manufacturer name, the site name, and so on exactly as they exist.
3. **Upload the file.** Each row is processed independently: valid rows are imported and
   invalid rows are reported individually with the reason. A bad row never blocks the rest
   of the file.

| Import page | Route | Replaces | Notes |
|---|---|---|---|
| Import Asset Models | `/master/import/asset-models` | `ImportAssetModels.aspx` | Manufacturer, Model Type, Mount Type and Air Flow Direction matched by name. |
| Import Applications | `/master/import/applications` | `ImportApplications.aspx` | Business Unit, Application Type, Criticality, Owner and Status matched by name. |
| Import Asset-App Map | `/master/import/app-asset-map` | `ImportAppAssetMap.aspx` | One Application + HostName per row. Importing only **adds** mappings — it never removes existing ones. |
| Import Assets | `/assets/import/assets` | `ImportAsset.aspx` | Site and Rack must **already exist**; Manufacturer/Model/Owner matched by name. |
| Import Blades | `/assets/import/blades` | `ImportBlades.aspx` | Each blade is placed into an existing enclosure identified by `ParentAssetTag`. The model must be a blade and the parent an enclosure with a free slot. |

> **Locations are never created by an import.** If a site or rack named in your
> spreadsheet does not exist, those rows are rejected — create the location first on
> `/master/locations`.

---

## 5. Assets

### 5.1 Asset browser and lifecycle — `/assets`

Replaces `AssetSearchPage.aspx`.

The page is a paged, searchable grid of assets. Selecting a row opens a **lifecycle side
panel** showing:

- **State chips** — the states currently active on the asset (written off, restricted,
  barred, decommissioned, RFID card assigned).
- **Action buttons** — only the transitions that are *valid right now*.

Each action opens a small dialog for a reason and/or date, posts the change, and refreshes
the panel.

| Action | Available when | Effect |
|---|---|---|
| Write off / Reinstate | asset is not / is written off | Marks the asset written off, or reverses it. |
| Restrict / De-restrict | asset is not / is restricted | Applies or removes a permanent restriction. |
| Bar / Un-bar | asset is not / is barred | Applies or clears a barred date window. |
| Decommission / Recommission | asset is not / is decommissioned | Retires or returns the asset to service. |
| Assign / De-assign RFID card | card absent / present | Links or unlinks an RFID card number. |

The five state families are independent — an asset can be, say, both restricted and
barred. If you attempt a transition that is no longer valid (for example, restricting an
already-restricted asset because someone else changed it in another tab), the application
reports a conflict; refresh the panel and try again.

### 5.2 Create asset — `/assets/new`

Replaces the core of `CreateAsset.aspx`. The form cascades:

- **Manufacturer → Model** — choosing a manufacturer filters the model list.
- **Site → Location** — choosing a site filters locations to those assigned to it.

Plus owner, orientation and rack position fields. Two values are derived for you and are
not on the form:

- the **asset group** comes from the selected model;
- the **business unit** comes from the selected site.

Uniqueness (asset tag, serial number, host name, and so on) follows your organisation's
configured DCTrack rule — the application preserves the existing rule rather than
imposing a new one. If a save is rejected, the DCTrack message text is shown directly.

### 5.3 Inventory update — `/assets/inventory`

Replaces `InventoryUpdate.aspx`. This page reconciles **RFID stock-take sessions**
recorded by handheld scanners.

1. Pick one or more locations.
2. The grid lists stock-take sessions for those locations with counts of assets,
   scanned, missing and over-scanned.
3. **Commit** on a row reconciles that session's unprocessed scanned assets.

> **Commit is destructive.** It moves and marks asset records according to what was
> scanned, and it cannot be undone from the application. Review the counts first.

Sessions only appear where RFID scans exist — the scanning itself happens on the handheld
devices, outside this application.

---

## 6. Reports — `/master/reports/{report}`

Every report page works the same way: fill in the parameters at the top, run the report,
review the results in the grid, and use **Export** to download them.

Parameters are dropdowns populated from live master data (Site, Business Unit, Location,
Application, Criticality, Asset Type) plus date ranges and free-text/flag fields. Leaving
a parameter empty generally means "all".

| Report | Route | Replaces |
|---|---|---|
| Rack Tag Details | `/master/reports/rack-tag-details` | `RptRackTagDetails.aspx` |
| Last Printed Tag Details | `/master/reports/last-printed-tags` | `RptLastTagDetails.aspx` |
| Capacity Report | `/master/reports/capacity` | `RptCapacityReport.aspx` |
| Rack Inventory Report | `/master/reports/rack-inventory` | `RptRackInventory.aspx` |
| Rack Details | `/master/reports/rack-details` | *(rack drill-down)* |
| Inventory Report | `/master/reports/inventory` | `RptInventoryReport.aspx` |
| Asset Transaction Report | `/master/reports/transaction-list` | `RptTransactionsList.aspx` |
| Asset History Report | `/master/reports/asset-history` | `RptTransactionHistory.aspx` |
| Asset Movement Report | `/master/reports/asset-movement` | `RptAssetMovement.aspx` |
| App Summary by Rack | `/master/reports/app-summary` | `RptAppSummaryByRack.aspx` |
| Asset Data – Excel | `/master/reports/asset-data-excel` | `AssetDataExcel.aspx` |

The **Asset Movement Report** takes the most parameters: a date range plus source and
destination business unit / site / location, asset type, and an "in-transit only" flag.

---

## 7. Administration pages

These two pages replace the legacy security-administration screens. They require a right
on the legacy *Group Module Access Rights Assignment* module — most users will not see
them.

### 7.1 Group rights — `/master/admin/group-rights`

Replaces `GroupModuleRightsAssignment.aspx`. Pick a group and you get the full
**main module → module → rights** matrix with a checkbox per granted right (Create,
Modify, Delete, View, Bar, Write-off and the rest of the 31 legacy right types). Changes
are tracked per module, so you can adjust several modules and save.

### 7.2 User groups — `/master/admin/user-groups`

Replaces the group-membership part of `ManageUsers.aspx`. Select a user, then assign or
remove groups.

> **Rights changes take effect within about a minute** — the services cache permissions
> for 60 seconds. No restart or re-login is needed; if a user says a change has not
> applied, wait a minute and have them reload.

---

## 8. What is not migrated yet

If you cannot find one of these, that is expected — use the legacy application:

- `GlobalParameters.aspx` (global parameter maintenance)
- `Department` maintenance
- The **tenant location-tree and tenant permissions** sub-system (basic tenant CRUD *is*
  available on `/master/tenants`)
- Creating racks/locations as part of an asset import (the legacy Racks sheet)
- The handheld RFID scanning pipeline itself (device-side)

Menu entries for unmigrated modules appear **disabled** in the side navigation.

---

## 9. Troubleshooting

| Symptom | Cause / what to do |
|---|---|
| Redirected to a sign-in page you did not expect | Your session expired. Sign in again; unsaved dialog content is lost. |
| Grids are empty and everything looks broken | Usually a lost or unauthorised session. Reload the page — you will be sent to sign-in if needed. If it persists, contact your administrator. |
| You sign in successfully but the menu is completely empty | Your sign-in account is not linked to a DCTrack user record, so you have no rights at all. Contact your administrator — this is a configuration issue, not something you can fix. |
| Your old DCTrack password is not accepted | Sign-in has moved to the central identity system. Depending on how your organisation configured it, use your Windows/corporate password, or the new password your administrator issued. |
| "Forbidden" page after clicking a link or typing a URL | You do not hold a right on that module. Request it via §7. |
| A menu item is greyed out | That legacy module has not been migrated yet (§8). |
| Saving a new record says the name already exists | An active record has that name. If the name belonged to a deleted record, saving reactivates it instead — check the list. |
| A lifecycle action button disappeared | Someone else changed the asset. Re-select the row to refresh its state. |
| A lifecycle action returns a conflict | The transition is no longer valid for the asset's current state. Refresh and retry. |
| Import rows rejected with "unknown site/rack/parent" | Those records are matched **by name** and must already exist. Create them first (§4). |
| Blade import rejected | The model must be a blade, the parent must be an enclosure, and it must have a free slot. |
| A rights change has not taken effect | Permissions are cached for ~60 seconds. Wait and reload. |
| Export downloads nothing | Check the browser's pop-up/download blocker. |

---

## 10. Related documents

- [Admin Guide](ADMIN-GUIDE.md) — deployment to IIS and to containers, configuration,
  Keycloak setup, operations.
- Project [README](../README.md) — architecture, migration status, developer setup.

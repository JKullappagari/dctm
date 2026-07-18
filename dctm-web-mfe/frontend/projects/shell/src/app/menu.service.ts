import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

export interface MenuModuleItem {
  moduleID: number;
  label: string;
  legacyUrl: string | null;
}

export interface MenuSection {
  title: string;
  items: MenuModuleItem[];
}

/**
 * Maps legacy page URLs (as stored in tblModule.PageURL) to migrated routes.
 * Modules not listed here have not been migrated yet and render as disabled.
 * This table shrinks the legacy app one line at a time.
 */
export const LEGACY_ROUTE_MAP: Record<string, string> = {
  'Sites.aspx': '/master/sites',
  'Location.aspx': '/master/locations',
  'Host.aspx': '/master/hosts',
  'AssetModel.aspx': '/master/asset-models',
  'DeviceReg.aspx': '/master/devices',
  'AuditCycle.aspx': '/master/audit-cycles',
  'ImportAssetModels.aspx': '/master/import/asset-models',
  // Asset search + lifecycle (writeoff/restrict/bar/decommission/RFID) consolidated
  // into the single Assets page with its lifecycle panel.
  'AssetSearchPage.aspx': '/assets',
  'CreateAsset.aspx': '/assets/new',
  'ImportAsset.aspx': '/assets/import/assets',
  'ImportBlades.aspx': '/assets/import/blades',
  'InventoryUpdate.aspx': '/assets/inventory',
  'Tenant.aspx': '/master/tenants',
  'ApplicationType.aspx': '/master/application-types',
  'ApplicationCriticality.aspx': '/master/application-criticalities',
  'Group.aspx': '/master/groups',
  'UserSearch.aspx': '/master/users',
  'CreateApplication.aspx': '/master/applications',
  'ImportApplications.aspx': '/master/import/applications',
  'ImportAppAssetMap.aspx': '/master/import/app-asset-map',
  'ApplicationMap.aspx': '/master/assign/app-map',
  'AssetDataExcel.aspx': '/master/reports/asset-data-excel',
  // Reports — SSRS .rdl replaced by the reporting service + generic report page.
  'RptAssetMovement.aspx': '/master/reports/asset-movement',
  'RptInventoryReport.aspx': '/master/reports/inventory',
  'RptTransactionsList.aspx': '/master/reports/transaction-list',
  'RptTransactionHistory.aspx': '/master/reports/asset-history',
  'RptRackTagDetails.aspx': '/master/reports/rack-tag-details',
  'RptRackInventory.aspx': '/master/reports/rack-inventory',
  'RptAppSummaryByRack.aspx': '/master/reports/app-summary',
  'RptLastTagDetails.aspx': '/master/reports/last-printed-tags',
  'RptCapacityReport.aspx': '/master/reports/capacity',
  'GroupModuleRightsAssignment.aspx': '/master/admin/group-rights',
  // ManageUsers.aspx → its group-membership slice; identity lifecycle lives in Keycloak.
  'ManageUsers.aspx': '/master/admin/user-groups',
  'BUDivAssignment.aspx': '/master/assign/bu-divisions',
  'BUSiteAssignment.aspx': '/master/assign/bu-sites',
  'SiteLocationAssignment.aspx': '/master/assign/site-locations',
  'Manufacturer.aspx': '/master/manufacturers',
  'BusinessUnit.aspx': '/master/business-units',
  'Division.aspx': '/master/divisions',
  'Owner.aspx': '/master/owners',
  'AssetType.aspx': '/master/asset-groups',
  'TechnologyCategory.aspx': '/master/technology-categories',
  'LocationType.aspx': '/master/location-types',
  'Purpose.aspx': '/master/purposes',
  'MusterReason.aspx': '/master/muster-reasons',
};

@Injectable({ providedIn: 'root' })
export class MenuService {
  private readonly http = inject(HttpClient);

  /** Security-trimmed menu from the Identity & Access Service (replaces SqlSiteMapProvider). */
  load(): Observable<MenuSection[]> {
    return this.http.get<MenuSection[]>('http://localhost:8082/api/v1/me/menu');
  }
}

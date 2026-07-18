export interface AssignmentItem {
  id: number;
  name: string;
}

export interface AssignmentView {
  assigned: AssignmentItem[];
  available: AssignmentItem[];
}

/** One legacy dual-list assignment page (assign child items to a parent). */
export interface AssignmentConfig {
  /** Route segment + API path under /assignments, e.g. 'bu-divisions'. */
  path: string;
  title: string;
  /** Parent picker: which entity owns the assignment (e.g. business-units). */
  parentPath: string;
  parentIdField: string;
  parentNameField: string;
  parentLabel: string;
  itemLabel: string;
}

export const ASSIGNMENT_CONFIGS: AssignmentConfig[] = [
  {
    // retires BUDivAssignment.aspx
    path: 'bu-divisions',
    title: 'Business Unit → Divisions',
    parentPath: 'business-units',
    parentIdField: 'businessUnitID',
    parentNameField: 'businessUnit',
    parentLabel: 'Business Unit',
    itemLabel: 'Divisions',
  },
  {
    // retires BUSiteAssignment.aspx
    path: 'bu-sites',
    title: 'Business Unit → Sites',
    parentPath: 'business-units',
    parentIdField: 'businessUnitID',
    parentNameField: 'businessUnit',
    parentLabel: 'Business Unit',
    itemLabel: 'Sites',
  },
  {
    // retires SiteLocationAssignment.aspx
    path: 'site-locations',
    title: 'Site → Locations',
    parentPath: 'sites',
    parentIdField: 'siteID',
    parentNameField: 'site',
    parentLabel: 'Site',
    itemLabel: 'Locations',
  },
  {
    // retires ApplicationMap.aspx — Host (GUID-keyed) → Applications
    path: 'app-map',
    title: 'Application Map',
    parentPath: 'hosts',
    parentIdField: 'hostID',
    parentNameField: 'hostName',
    parentLabel: 'Host',
    itemLabel: 'Applications',
  },
];

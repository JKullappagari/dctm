import { LookupConfig, LookupRow } from './lookup/lookup.model';

/** Resolves the selected Location Type id in the form to its name via the loaded options. */
function typeNameOf(
  form: Record<string, unknown>,
  rows: Record<string, LookupRow[]>,
): string | null {
  const selected = rows['locationTypeId']?.find(
    (t) => t['locationTypeID'] === form['locationTypeId'],
  );
  return (selected?.['locationType'] as string) ?? null;
}

/**
 * One config per retired legacy page. Adding the next master-data entity
 * (Sites, BusinessUnit, Division, …) is one entry here + one API feature file.
 */
export const LOOKUP_ENTITIES: LookupConfig[] = [
  {
    // retires ApplicationType.aspx
    path: 'application-types',
    title: 'Application Types',
    idField: 'applTypeID',
    nameField: 'applType',
    module: 'Application Type',
    columns: [
      { field: 'applType', headerName: 'Application Type' },
      { field: 'applTypeDesc', headerName: 'Description' },
    ],
    fields: [
      { key: 'applType', label: 'Application Type', type: 'text', required: true },
      { key: 'description', label: 'Description', type: 'textarea' },
    ],
  },
  {
    // retires ApplicationCriticality.aspx
    path: 'application-criticalities',
    title: 'Application Criticalities',
    idField: 'applCriticalityID',
    nameField: 'applCriticality',
    module: 'Application Criticality',
    columns: [
      { field: 'applCriticality', headerName: 'Criticality' },
      { field: 'applCriticalityDesc', headerName: 'Description' },
    ],
    fields: [
      { key: 'applCriticality', label: 'Criticality', type: 'text', required: true },
      { key: 'description', label: 'Description', type: 'textarea' },
      {
        key: 'backColor', rowKey: 'backColorCOde', label: 'Background Colour (hex)', type: 'text',
        pattern: '^#?[0-9A-Fa-f]{6}$', patternError: 'Enter a 6-digit hex colour, e.g. #3366FF',
      },
      {
        key: 'foreColor', rowKey: 'foreColorCode', label: 'Foreground Colour (hex)', type: 'text',
        pattern: '^#?[0-9A-Fa-f]{6}$', patternError: 'Enter a 6-digit hex colour, e.g. #3366FF',
      },
    ],
  },
  {
    // retires Group.aspx
    path: 'groups',
    title: 'Groups',
    idField: 'groupID',
    nameField: 'group',
    module: 'Group',
    columns: [
      { field: 'group', headerName: 'Group' },
      { field: 'description', headerName: 'Description' },
    ],
    fields: [
      { key: 'group', label: 'Group', type: 'text', required: true },
      { key: 'description', label: 'Description', type: 'textarea' },
    ],
  },
  {
    // retires CreateApplication.aspx — multi-FK form (BU, type, criticality, owner, status)
    path: 'applications',
    title: 'Applications',
    idField: 'applID',
    nameField: 'applName',
    module: 'Create Application',
    columns: [
      { field: 'applName', headerName: 'Application' },
      { field: 'applType', headerName: 'Type' },
      { field: 'applStatus', headerName: 'Status' },
      { field: 'businessUnit', headerName: 'Business Unit' },
      { field: 'owner', headerName: 'Owner' },
    ],
    fields: [
      { key: 'applName', label: 'Application Name', type: 'text', required: true },
      { key: 'description', rowKey: 'applDesc', label: 'Description', type: 'textarea' },
      {
        key: 'buId',
        rowKey: 'buid',
        label: 'Business Unit',
        type: 'select',
        optionsPath: 'business-units',
        optionValue: 'businessUnitID',
        optionLabel: 'businessUnit',
      },
      {
        key: 'applTypeId',
        rowKey: 'applTypeID',
        label: 'Application Type',
        type: 'select',
        optionsPath: 'application-types',
        optionValue: 'applTypeID',
        optionLabel: 'applType',
      },
      {
        key: 'criticalityId',
        rowKey: 'applCriticalityID',
        label: 'Criticality',
        type: 'select',
        optionsPath: 'application-criticalities',
        optionValue: 'applCriticalityID',
        optionLabel: 'applCriticality',
      },
      {
        key: 'ownerId',
        rowKey: 'ownerID',
        label: 'Owner',
        type: 'select',
        optionsPath: 'owners',
        optionValue: 'ownerID',
        optionLabel: 'ownerFirstName',
      },
      {
        key: 'appStatusId',
        rowKey: 'appStatusID',
        label: 'Status',
        type: 'select',
        optionsPath: 'ref/app-statuses',
        optionValue: 'appStatusID',
        optionLabel: 'appStatus',
      },
    ],
  },
  {
    // retires UserSearch.aspx — read-only; users are managed in Keycloak
    path: 'users',
    title: 'Users',
    idField: 'userID',
    nameField: 'loginName',
    module: 'User Search',
    readOnly: true,
    columns: [
      { field: 'loginName', headerName: 'Login' },
      { field: 'name', headerName: 'Name' },
      { field: 'siteRestriction', headerName: 'Site Restriction', boolean: true },
      { field: 'status', headerName: 'Status' },
    ],
    fields: [],
  },
  {
    // retires DeviceReg.aspx — RFID reader / mobile device registration
    path: 'devices',
    title: 'Devices',
    idField: 'id',
    nameField: 'deviceName',
    module: 'Register Device',
    columns: [
      { field: 'deviceName', headerName: 'Device Name' },
      { field: 'deviceID', headerName: 'Device ID' },
      { field: 'site', headerName: 'Site' },
      { field: 'statusValue', headerName: 'Status' },
    ],
    fields: [
      { key: 'deviceID', label: 'Device ID', type: 'text', required: true },
      { key: 'deviceName', label: 'Device Name', type: 'text', required: true },
      {
        key: 'siteId',
        rowKey: 'siteID',
        label: 'Site',
        type: 'select',
        required: true,
        optionsPath: 'sites',
        optionValue: 'siteID',
        optionLabel: 'site',
      },
    ],
  },
  {
    // retires AuditCycle.aspx — a location (room) + audit date range; insert-only
    path: 'audit-cycles',
    title: 'Audit Cycles',
    idField: 'id',
    nameField: 'room',
    module: 'Audit Cycle',
    noEdit: true,
    columns: [
      { field: 'site', headerName: 'Site' },
      { field: 'room', headerName: 'Room' },
      { field: 'startDate', headerName: 'Start' },
      { field: 'endDate', headerName: 'End' },
      { field: 'auditCount', headerName: 'Audits' },
    ],
    fields: [
      {
        key: 'locationId',
        rowKey: 'locationID',
        label: 'Location (Room)',
        type: 'select',
        required: true,
        optionsPath: 'locations',
        optionValue: 'locationID',
        optionLabel: 'path',
        // Audits run at Room level.
        optionsFilter: (row) => row['locationType'] === 'Room',
      },
      { key: 'startDate', label: 'Start Date', type: 'date', required: true },
      { key: 'endDate', label: 'End Date', type: 'date', required: true },
    ],
  },
  {
    // retires Manufacturer.aspx
    path: 'manufacturers',
    title: 'Manufacturers',
    idField: 'mfgID',
    nameField: 'mfgName',
    module: 'Manufacturer',
    columns: [
      { field: 'mfgName', headerName: 'Manufacturer' },
      { field: 'description', headerName: 'Description' },
    ],
    fields: [
      { key: 'mfgName', label: 'Manufacturer', type: 'text', required: true, maxLength: 25 },
      { key: 'description', label: 'Description', type: 'textarea' },
    ],
  },
  {
    // retires LocationType.aspx
    path: 'location-types',
    title: 'Location Types',
    idField: 'locationTypeID',
    nameField: 'locationType',
    columns: [
      { field: 'locationType', headerName: 'Location Type' },
      { field: 'description', headerName: 'Description' },
      { field: 'isStorageType', headerName: 'Storage Type', boolean: true },
      { field: 'isRfidLocation', headerName: 'RFID Location', boolean: true },
    ],
    fields: [
      { key: 'locationType', label: 'Location Type', type: 'text', required: true },
      { key: 'description', label: 'Description', type: 'textarea' },
      { key: 'isStorageType', label: 'Is Storage Type', type: 'checkbox' },
      { key: 'isRfidLocation', label: 'Is RFID Location', type: 'checkbox' },
    ],
  },
  {
    // retires Purpose.aspx
    path: 'purposes',
    title: 'Purposes',
    idField: 'purposeID',
    nameField: 'purpose',
    module: 'Purpose',
    columns: [
      { field: 'purpose', headerName: 'Purpose' },
      { field: 'description', headerName: 'Description' },
    ],
    fields: [
      { key: 'purpose', label: 'Purpose', type: 'text', required: true },
      { key: 'description', label: 'Description', type: 'textarea' },
    ],
  },
  {
    // retires Tenant.aspx (core CRUD; the location-tree / permissions sub-system is deferred)
    path: 'tenants',
    title: 'Tenants',
    idField: 'tenantId',
    nameField: 'tenantFullName',
    module: 'Tenant',
    columns: [
      { field: 'tenantFullName', headerName: 'Full Name' },
      { field: 'tenantShortName', headerName: 'Short Name' },
      { field: 'tenantType', headerName: 'Type' },
      { field: 'contactEmail', headerName: 'Contact Email' },
      { field: 'userCount', headerName: 'Users' },
    ],
    fields: [
      { key: 'tenantFullName', label: 'Full Name', type: 'text', required: true },
      { key: 'tenantShortName', label: 'Short Name', type: 'text', required: true },
      { key: 'tenantType', label: 'Type', type: 'text' },
      { key: 'tenantTypeSize', label: 'Type Size', type: 'number', min: 0 },
      { key: 'contactFName', rowKey: 'contactFirstName', label: 'Contact First Name', type: 'text' },
      { key: 'contactLName', rowKey: 'contactLastName', label: 'Contact Last Name', type: 'text' },
      { key: 'contactEmail', label: 'Contact Email', type: 'text', email: true },
      { key: 'userCount', label: 'User Count', type: 'number', min: 0 },
    ],
  },
  {
    // retires AssetModel.aspx — multi-FK form (manufacturer, model-type, tech, BU,
    // mount type, airflow) + numeric fields; the ~25 physical/enclosure params default.
    path: 'asset-models',
    title: 'Asset Models',
    idField: 'modelID',
    nameField: 'modelName',
    module: 'Asset Model',
    columns: [
      { field: 'modelName', headerName: 'Model' },
      { field: 'mfgName', headerName: 'Manufacturer' },
      { field: 'modelType', headerName: 'Model Type' },
      { field: 'mountType', headerName: 'Mount Type' },
      { field: 'assetCount', headerName: 'Assets' },
    ],
    fields: [
      { key: 'modelName', label: 'Model Name', type: 'text', required: true },
      { key: 'description', label: 'Description', type: 'textarea' },
      {
        key: 'mfgId',
        rowKey: 'mfgID',
        label: 'Manufacturer',
        type: 'select',
        required: true,
        optionsPath: 'manufacturers',
        optionValue: 'mfgID',
        optionLabel: 'mfgName',
      },
      {
        key: 'modelTypeId',
        rowKey: 'assetTypeID',
        label: 'Model Type',
        type: 'select',
        optionsPath: 'asset-groups',
        optionValue: 'assetGroupID',
        optionLabel: 'assetGroup',
      },
      {
        key: 'techId',
        rowKey: 'techID',
        label: 'Technology Category',
        type: 'select',
        optionsPath: 'technology-categories',
        optionValue: 'techID',
        optionLabel: 'techName',
      },
      {
        key: 'buId',
        rowKey: 'businessUnitID',
        label: 'Business Unit',
        type: 'select',
        required: true,
        optionsPath: 'business-units',
        optionValue: 'businessUnitID',
        optionLabel: 'businessUnit',
      },
      {
        key: 'mountTypeId',
        rowKey: 'mountTypeID',
        label: 'Mount Type',
        type: 'select',
        optionsPath: 'ref/mount-types',
        optionValue: 'mountTypeID',
        optionLabel: 'mountType',
      },
      {
        key: 'afDirectionId',
        rowKey: 'airFlowDirectionID',
        label: 'Air Flow Direction',
        type: 'select',
        optionsPath: 'ref/airflow-directions',
        optionValue: 'id',
        optionLabel: 'airFlowDirection',
      },
      { key: 'uHeight', label: 'U Height', type: 'number', min: 0 },
      { key: 'maxPower', rowKey: 'maxPower_Watts', label: 'Max Power (W)', type: 'number', min: 0 },
      { key: 'isBlade', label: 'Is Blade', type: 'checkbox' },
      { key: 'isEnclosure', label: 'Is Enclosure', type: 'checkbox' },
      { key: 'comment', label: 'Comment', type: 'textarea' },
    ],
  },
  {
    // retires Location.aspx — LocationType FK + self-referential Parent Location
    path: 'locations',
    title: 'Locations',
    idField: 'locationID',
    nameField: 'location',
    module: 'Location',
    columns: [
      { field: 'location', headerName: 'Name' },
      { field: 'path', headerName: 'Path' },
      { field: 'locationType', headerName: 'Location Type' },
      { field: 'parentLocation', headerName: 'Parent' },
      { field: 'tagID', headerName: 'Tag' },
      { field: 'description', headerName: 'Description' },
    ],
    fields: [
      { key: 'location', label: 'Location', type: 'text', required: true },
      { key: 'description', label: 'Description', type: 'textarea' },
      {
        key: 'locationTypeId',
        rowKey: 'locationTypeID',
        label: 'Location Type',
        type: 'select',
        optionsPath: 'location-types',
        optionValue: 'locationTypeID',
        optionLabel: 'locationType',
      },
      {
        key: 'parentLocationId',
        rowKey: 'parentLocationID',
        label: 'Parent Location',
        type: 'select',
        optionsPath: 'locations',
        optionValue: 'locationID',
        optionLabel: 'path',
        // Legacy hierarchy: Room has no parent; Row goes under a Room;
        // Rack goes under a Row or a Room.
        dependsOn: 'locationTypeId',
        disabledWhen: (form, rows) => typeNameOf(form, rows) === 'Room',
        optionsFilter: (row, form, rows) => {
          const type = typeNameOf(form, rows);
          if (type === 'Row') return row['locationType'] === 'Room';
          if (type === 'Rack') return row['locationType'] === 'Room' || row['locationType'] === 'Row';
          return type != null && type !== 'Room';
        },
      },
      { key: 'tagId', rowKey: 'tagID', label: 'Tag ID', type: 'text' },
      { key: 'floorNo', label: 'Floor No', type: 'text' },
      { key: 'isExitDoor', label: 'Is Exit Door', type: 'checkbox' },
      { key: 'isCheckOutLocation', label: 'Is Check-out Location', type: 'checkbox' },
    ],
  },
  {
    // retires Host.aspx / HostPopup.aspx (GUID-keyed)
    path: 'hosts',
    title: 'Hosts',
    idField: 'hostID',
    nameField: 'hostName',
    module: 'Host',
    columns: [
      { field: 'hostName', headerName: 'Host' },
      { field: 'description', headerName: 'Description' },
    ],
    fields: [
      { key: 'hostName', label: 'Host', type: 'text', required: true },
      { key: 'description', label: 'Description', type: 'textarea' },
    ],
  },
  {
    // retires Sites.aspx — cascading Region → Country → City selects
    path: 'sites',
    title: 'Sites',
    idField: 'siteID',
    nameField: 'site',
    module: 'Site',
    editEnrichPath: 'geo',
    columns: [
      { field: 'site', headerName: 'Site' },
      { field: 'description', headerName: 'Description' },
      { field: 'region', headerName: 'Region' },
      { field: 'country', headerName: 'Country' },
      { field: 'city', headerName: 'City' },
    ],
    fields: [
      { key: 'site', label: 'Site', type: 'text', required: true },
      { key: 'description', label: 'Description', type: 'textarea' },
      {
        key: 'region',
        rowKey: 'region',
        label: 'Region',
        type: 'select',
        optionsPath: 'geo/regions',
        optionValue: 'region',
        optionLabel: 'region',
      },
      {
        key: 'countryId',
        rowKey: 'countryID',
        label: 'Country',
        type: 'select',
        optionsPath: 'geo/countries',
        optionValue: 'countryID',
        optionLabel: 'countryName',
        dependsOn: 'region',
        optionsParam: 'region',
      },
      {
        key: 'cityId',
        rowKey: 'cityID',
        label: 'City',
        type: 'select',
        optionsPath: 'geo/cities',
        optionValue: 'cityID',
        optionLabel: 'cityName',
        dependsOn: 'countryId',
        optionsParam: 'countryId',
      },
    ],
  },
  {
    // retires BusinessUnit.aspx
    path: 'business-units',
    title: 'Business Units',
    idField: 'businessUnitID',
    nameField: 'businessUnit',
    columns: [
      { field: 'businessUnit', headerName: 'Business Unit' },
      { field: 'coPrefix', headerName: 'Company Prefix' },
      { field: 'description', headerName: 'Description' },
    ],
    fields: [
      { key: 'businessUnit', label: 'Business Unit', type: 'text', required: true, maxLength: 50 },
      { key: 'coPrefix', label: 'Company Prefix', type: 'text', maxLength: 5 },
      { key: 'description', label: 'Description', type: 'textarea' },
    ],
  },
  {
    // retires Division.aspx
    path: 'divisions',
    title: 'Divisions',
    idField: 'divisionID',
    nameField: 'division',
    module: 'Division',
    columns: [
      { field: 'division', headerName: 'Division' },
      { field: 'divisionDesc', headerName: 'Description' },
    ],
    fields: [
      { key: 'division', label: 'Division', type: 'text', required: true },
      { key: 'description', rowKey: 'divisionDesc', label: 'Description', type: 'textarea' },
    ],
  },
  {
    // retires Owner.aspx
    path: 'owners',
    title: 'Owners / Custodians',
    idField: 'ownerID',
    nameField: 'ownerFirstName',
    module: 'Owner',
    columns: [
      { field: 'ownerFirstName', headerName: 'First Name' },
      { field: 'ownerLastName', headerName: 'Last Name' },
      { field: 'email', headerName: 'Email' },
      { field: 'division', headerName: 'Division' },
    ],
    fields: [
      { key: 'firstName', rowKey: 'ownerFirstName', label: 'First Name', type: 'text', required: true },
      { key: 'lastName', rowKey: 'ownerLastName', label: 'Last Name', type: 'text' },
      { key: 'email', label: 'Email', type: 'text', email: true },
      {
        key: 'divisionId',
        rowKey: 'divisionID',
        label: 'Division',
        type: 'select',
        optionsPath: 'divisions',
        optionValue: 'divisionID',
        optionLabel: 'division',
      },
    ],
  },
  {
    // retires TechnologyCategory.aspx
    path: 'technology-categories',
    title: 'Technology Categories',
    idField: 'techID',
    nameField: 'techName',
    module: 'Technology Category',
    columns: [
      { field: 'techName', headerName: 'Technology Category' },
      { field: 'description', headerName: 'Description' },
    ],
    fields: [
      { key: 'techName', label: 'Technology Category', type: 'text', required: true },
      { key: 'description', label: 'Description', type: 'textarea' },
    ],
  },
  {
    // retires AssetType.aspx ("Asset Group")
    path: 'asset-groups',
    title: 'Asset Groups',
    idField: 'assetGroupID',
    nameField: 'assetGroup',
    columns: [
      { field: 'assetGroup', headerName: 'Asset Group' },
      { field: 'description', headerName: 'Description' },
    ],
    fields: [
      { key: 'assetGroup', label: 'Asset Group', type: 'text', required: true },
      { key: 'description', label: 'Description', type: 'textarea' },
    ],
  },
  {
    // retires MusterReason.aspx
    path: 'muster-reasons',
    title: 'Muster Reasons',
    idField: 'musterReasonID',
    nameField: 'musterReason',
    module: 'Reason',
    columns: [
      { field: 'musterReason', headerName: 'Muster Reason' },
      { field: 'description', headerName: 'Description' },
    ],
    fields: [
      { key: 'musterReason', label: 'Muster Reason', type: 'text', required: true },
      { key: 'description', label: 'Description', type: 'textarea' },
    ],
  },
];

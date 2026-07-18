namespace Reporting.Api;

public enum ParamType { Int, Date, Text, Bool, LocList }

/// <summary>
/// One report input. SpParam is the stored-proc parameter; OptionsPath (for Int params)
/// is a master-data endpoint the UI loads a dropdown from; ValueField/LabelField pick
/// the option columns. Int params default to 0 (= "all") when blank.
/// </summary>
public sealed record ReportParam(
    string Key, string SpParam, ParamType Type, string Label,
    string? OptionsPath = null, string? ValueField = null, string? LabelField = null,
    // The legacy dynamic-SQL report SPs treat a sentinel value as "no filter" (usually
    // 'NULL' for text/location lists, '0' for ids/types). When the UI leaves a filter
    // blank ("All"), this value is sent instead of an empty string — otherwise the SP
    // builds invalid SQL like `IN ()`. Date filters use a wide range so blank = all dates.
    string? EmptyValue = null);

/// <summary>
/// One report migrated from a legacy .rdl — the SSRS layout is dropped; the report's
/// stored procedure (which did all the work) is kept and rendered in the app. Module is
/// the legacy tblModule name used to permission it.
/// </summary>
public sealed record ReportDef(
    string Key, string Title, string Module, string Sp, IReadOnlyList<ReportParam> Params,
    // Most report SPs filter by the acting user's site access (@pIntLoggedInUserId);
    // a few (tag-detail reports) take no such param.
    bool NeedsUser = true);

public static class ReportRegistry
{
    // Reusable param definitions with dropdowns from the master-data API.
    private static ReportParam Site(string sp = "@pIntSiteID") =>
        new("siteId", sp, ParamType.Int, "Site", "sites", "siteID", "site");
    private static ReportParam Bu(string sp = "@pIntBusinessUnitID") =>
        new("businessUnitId", sp, ParamType.Int, "Business Unit", "business-units", "businessUnitID", "businessUnit");
    private static ReportParam Location(string sp = "@pIntLocationID") =>
        new("locationId", sp, ParamType.Int, "Location", "locations", "locationID", "path");
    // Blank dates widen to an all-inclusive range so "no dates" means "all dates"
    // (the SPs compare 'yyyy-MM-dd' strings, so these bounds cover everything).
    private static ReportParam Start() => new("startDate", "@StartDate", ParamType.Date, "Start Date", EmptyValue: "1900-01-01");
    private static ReportParam End() => new("endDate", "@EndDate", ParamType.Date, "End Date", EmptyValue: "2999-12-31");
    // The transaction/inventory SPs use the literal 'NULL' as the "all locations" sentinel.
    private static ReportParam LocList() => new("locList", "@LocList", ParamType.LocList, "Locations", EmptyValue: "NULL");

    public static readonly IReadOnlyList<ReportDef> All =
    [
        new("rack-tag-details", "Rack Tag Details", "Rack Tag Details",
            "iAssetTrack_sp_Report_RackTagDetails",
            [Site(), new("tagId", "@pVarTagID", ParamType.Text, "Tag ID")], NeedsUser: false),

        new("last-printed-tags", "Last Printed Tag Details", "Last Printed Tag Details",
            "iAssetTrack_sp_Report_LastPrintedAssetTagDetails", [], NeedsUser: false),

        new("capacity", "Capacity Report", "Capacity Report",
            "iAssettrack_sp_REPORT_CAPACITY_RACK",
            [Site(), Location(), Bu()]),

        new("rack-inventory", "Rack Inventory Report", "Rack Inventory Report",
            "iAssetTrack_sp_Report_Location_Details_By_Site_And_Location",
            [Site(), Location(), Bu()]),

        new("inventory", "Inventory Report", "Audit Report",
            "iAssetTrack_Sp_Report_Inventory",
            [Start(), End(), LocList(), new("historicalData", "@HistoricalData", ParamType.Bool, "Historical Data")]),

        new("transaction-list", "Asset Transaction Report", "Asset Transaction Report",
            "iAssetTrack_Sp_Report_TransactionList",
            [Start(), End(), new("sNo", "@SNo", ParamType.Text, "Asset No", EmptyValue: "NULL"), LocList(),
             new("transType", "@TransType", ParamType.Text, "Transaction Type", EmptyValue: "0")]),

        new("asset-history", "Asset History Report", "Asset History Report",
            "iAssetTrack_Sp_Report_TransactionHistory",
            [Start(), End(), new("sNo", "@SNo", ParamType.Text, "Asset No", EmptyValue: "NULL"), LocList(),
             new("transStatus", "@TransStatus", ParamType.Text, "Status", EmptyValue: "0")]),

        // Inter-BU movement report: the SP requires a specific source AND destination BU
        // (there is no "all BUs" path), so pick both to get results. Site/location/asset
        // filters honour 0 = all. Selectors come from the master-data dropdowns.
        new("asset-movement", "Asset Movement Report", "Asset Movement Report",
            "iAssetTrack_Sp_Report_AssetMovement",
            [Start(), End(),
             new("srcBU", "@srcBU", ParamType.Int, "Source BU", "business-units", "businessUnitID", "businessUnit"),
             new("srcSite", "@srcSite", ParamType.Int, "Source Site", "sites", "siteID", "site"),
             new("srcLoc", "@srcLoc", ParamType.Int, "Source Location", "locations", "locationID", "path"),
             new("dstBU", "@dstBU", ParamType.Int, "Dest BU", "business-units", "businessUnitID", "businessUnit"),
             new("dstSite", "@dstSite", ParamType.Int, "Dest Site", "sites", "siteID", "site"),
             new("dstLoc", "@dstLoc", ParamType.Int, "Dest Location", "locations", "locationID", "path"),
             new("assetType", "@AssetType", ParamType.Int, "Asset Type", "asset-groups", "assetGroupID", "assetGroup"),
             new("transitOnly", "@TransitOnly", ParamType.Bool, "In-transit only")]),

        new("app-summary", "App Summary by Rack", "App Summary - Rack",
            "iAssetTrack_sp_Report_Location_Details_For_AppSummary",
            [Site("@SiteID"), Bu("@BusinessUnitID"),
             new("appId", "@AppID", ParamType.Int, "Application", "applications", "applID", "applName"),
             new("appCriticalityId", "@AppCriticalityID", ParamType.Int, "Criticality", "application-criticalities", "applCriticalityID", "applCriticality")]),

        new("rack-details", "Rack Details", "Rack Inventory Report",
            "iAssetTrack_sp_Report_Rack_Details",
            [new("locationId", "@LocationID", ParamType.Int, "Rack", "locations", "locationID", "path")], NeedsUser: false),

        // columnList is echoed by the SP (it returns a fixed asset column set), so it's
        // an optional free-text passthrough.
        new("asset-data-excel", "Asset Data - Excel", "Asset Data - Excel",
            "iAssetTrack_sp_Report_AssetDataExcel",
            [new("columnList", "@columnList", ParamType.Text, "Columns (optional)")]),
    ];

    public static ReportDef? Find(string key) =>
        All.FirstOrDefault(r => r.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
}

using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns from iAssetTrack_Sp_AuditCycle_List (legacy AuditCycle.aspx grid).
public sealed class AuditCycleRow
{
    public int ID { get; set; }
    public int? BusinessUnitID { get; set; }
    public string? Site { get; set; }
    public int? SiteID { get; set; }
    public string? Room { get; set; }
    public int? AuditCount { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? LocationID { get; set; }
}

public sealed record AuditCycleWrite(int LocationId, DateTime StartDate, DateTime EndDate);

public static class AuditCycles
{
    // An audit cycle is a location (room) + date range — no editable name, and the
    // Update SP inserts only (its AuditCycleID is output-only), so the UI is create +
    // delete. DoesExist is composite (location + start + end).
    public static readonly LookupSpMap<AuditCycleWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_AuditCycle_List",
        UpsertSp: "iAssetTrack_Sp_AuditCycle_Update",
        DeleteSp: "iAssetTrack_Sp_AuditCycle_Delete",
        ExistsSp: "iAssetTrack_Sp_AuditCycle_DoesExist",
        IdParam: "@pIntAuditCycleID",
        NameParam: "@pIntLocationId",
        IdsParam: "@pIntAuditCycleIDs",
        NameOf: _ => "audit cycle",
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pIntLocationId", w.LocationId);
            p.Add("@pStartDate", w.StartDate);
            p.Add("@pEndDate", w.EndDate);
        },
        AddExistsParams: (p, w) =>
        {
            p.Add("@pIntLocationId", w.LocationId);
            p.Add("@pStartDate", w.StartDate);
            p.Add("@pEndDate", w.EndDate);
        },
        RequiresName: false,
        IncludeStatusOnUpsert: false,
        DeleteTakesIdsOnly: true);
}

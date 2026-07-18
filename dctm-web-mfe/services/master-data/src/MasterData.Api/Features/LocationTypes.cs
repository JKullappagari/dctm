using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns as returned by iAssetTrack_Sp_LocationType_List (legacy LocationType.aspx grid binding).
public sealed class LocationTypeRow
{
    public int LocationTypeID { get; set; }
    public string LocationType { get; set; } = "";
    public string? Description { get; set; }
    public int IsStorageType { get; set; }
    public int IsRfidLocation { get; set; }
}

public sealed record LocationTypeWrite(
    string LocationType, string? Description, bool IsStorageType, bool IsRfidLocation);

public static class LocationTypes
{
    // Contract ported from iAssetTrackBAL/LocationTypeBAL.cs + Constants.cs.
    public static readonly LookupSpMap<LocationTypeWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_LocationType_List",
        UpsertSp: "iAssetTrack_Sp_LocationType_Update",
        DeleteSp: "iAssetTrack_Sp_LocationType_Delete",
        ExistsSp: "iAssetTrack_Sp_LocationType_DoesExist",
        IdParam: "@pIntLocationTypeID",
        NameParam: "@pVarLocationType",
        IdsParam: "@pVarLocationTypeIDs",
        NameOf: w => w.LocationType,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarLocationType", w.LocationType);
            p.Add("@pVarDescription", w.Description ?? "");
            p.Add("@pIntIsStorageType", w.IsStorageType ? 1 : 0);
            p.Add("@pIntIsRfidLocation", w.IsRfidLocation ? 1 : 0);
        });
}

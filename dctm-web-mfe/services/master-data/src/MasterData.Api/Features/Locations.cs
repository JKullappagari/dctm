using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns from the first result set of iAssetTrack_Sp_Location_List (legacy Location.aspx grid).
public sealed class LocationRow
{
    public int LocationID { get; set; }
    public string Location { get; set; } = "";
    public string? Description { get; set; }
    public int? ParentLocationID { get; set; }
    public string? ParentLocation { get; set; }
    public int? LocationTypeID { get; set; }
    public string? LocationType { get; set; }
    public string? TagID { get; set; }
    public string? FloorNo { get; set; }
    public int IsExitDoor { get; set; }
    public int IsCheckOutLocation { get; set; }
    public string? Path { get; set; }
}

public sealed record LocationWrite(
    string Location,
    string? Description,
    int? LocationTypeId,
    int? ParentLocationId,
    string? TagId,
    string? FloorNo,
    bool IsExitDoor,
    bool IsCheckOutLocation,
    string? IpAddress);

public static class Locations
{
    // Verified against the restored DB. The Update SP has ~16 params (none defaulted),
    // so every one is supplied; fields the modern form doesn't expose pass safe blanks.
    // DoesExist is composite: a location name must be unique within its parent.
    public static readonly LookupSpMap<LocationWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_Location_List",
        UpsertSp: "iAssetTrack_Sp_Location_Update",
        DeleteSp: "iAssetTrack_Sp_Location_Delete",
        ExistsSp: "iAssetTrack_Sp_Location_DoesExist",
        IdParam: "@pIntLocationID",
        NameParam: "@pVarLocation",
        IdsParam: "@pVarLocationIDs",
        NameOf: w => w.Location,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarLocation", w.Location);
            p.Add("@pVarDescription", w.Description ?? "");
            p.Add("@pIntIsExitDoor", w.IsExitDoor ? 1 : 0);
            p.Add("@pIntIsCheckOutLocation", w.IsCheckOutLocation ? 1 : 0);
            p.Add("@pVarIpAddress", w.IpAddress ?? "");
            p.Add("@pIntParentLocationID", w.ParentLocationId ?? 0);
            p.Add("@pIntlocationTypeID", w.LocationTypeId ?? 0);
            p.Add("@pVarTagID", w.TagId ?? "");
            p.Add("@pVarMFG", "");
            p.Add("@pVarModel", "");
            p.Add("@pVarFloorNo", w.FloorNo ?? "");
            p.Add("@pBitIsSpecialRoom", false);
            p.Add("@pVarSerialNumber", "");
            p.Add("@pIntModelID", 0);
            p.Add("@pIntHeight", 0);
            p.Add("@pVarExternalID", "");
        },
        AddExistsParams: (p, w) =>
        {
            p.Add("@pVarLocation", w.Location);
            p.Add("@pIntParentLocationID", w.ParentLocationId ?? 0);
        });
}

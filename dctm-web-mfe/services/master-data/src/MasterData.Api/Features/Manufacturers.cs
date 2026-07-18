using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns as returned by iAssetTrack_Sp_Manufacturer_List (legacy Manufacturer.aspx grid binding).
public sealed class ManufacturerRow
{
    public int MfgID { get; set; }
    public string MfgName { get; set; } = "";
    public string? Description { get; set; }
}

public sealed record ManufacturerWrite(string MfgName, string? Description);

public static class Manufacturers
{
    // Contract ported from iAssetTrackBAL/ManufacturerBAL.cs + Constants.cs.
    public static readonly LookupSpMap<ManufacturerWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_Manufacturer_List",
        UpsertSp: "iAssetTrack_Sp_Manufacturer_Update",
        DeleteSp: "iAssetTrack_Sp_Manufacturer_Delete",
        ExistsSp: "iAssetTrack_Sp_Manufacturer_DoesExist",
        IdParam: "@pIntMfgID",
        NameParam: "@pVarMfgName",
        IdsParam: "@pVarMfgIDs",
        NameOf: w => w.MfgName,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarMfgName", w.MfgName);
            p.Add("@pVarDescription", w.Description ?? "");
        });
}

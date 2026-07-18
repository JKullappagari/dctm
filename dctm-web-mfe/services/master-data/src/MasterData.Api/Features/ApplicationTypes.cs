using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns from iAssetTrack_Sp_ApplType_List (legacy ApplicationType.aspx grid).
public sealed class ApplicationTypeRow
{
    public int ApplTypeID { get; set; }
    public string ApplType { get; set; } = "";
    public string? ApplTypeDesc { get; set; }
}

public sealed record ApplicationTypeWrite(string ApplType, string? Description);

public static class ApplicationTypes
{
    public static readonly LookupSpMap<ApplicationTypeWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_ApplType_List",
        UpsertSp: "iAssetTrack_Sp_ApplType_Update",
        DeleteSp: "iAssetTrack_Sp_ApplType_Delete",
        ExistsSp: "iAssetTrack_Sp_ApplType_DoesExist",
        IdParam: "@pIntApplTypeID",
        NameParam: "@pVarApplType",
        IdsParam: "@pVarApplTypeIDs",
        NameOf: w => w.ApplType,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarApplType", w.ApplType);
            p.Add("@pVarApplTypeDesc", w.Description ?? "");
        });
}

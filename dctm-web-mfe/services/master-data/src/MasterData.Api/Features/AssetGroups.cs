using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns as returned by iAssetTrack_Sp_AssetGroup_List (legacy AssetType.aspx "Asset Group" grid).
public sealed class AssetGroupRow
{
    public int AssetGroupID { get; set; }
    public string AssetGroup { get; set; } = "";
    public string? Description { get; set; }
}

public sealed record AssetGroupWrite(string AssetGroup, string? Description);

public static class AssetGroups
{
    public static readonly LookupSpMap<AssetGroupWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_AssetGroup_List",
        UpsertSp: "iAssetTrack_Sp_AssetGroup_Update",
        DeleteSp: "iAssetTrack_Sp_AssetGroup_Delete",
        ExistsSp: "iAssetTrack_SP_ASSETGROUP_DOESEXIST",
        IdParam: "@pIntAssetGroupID",
        NameParam: "@pVarAssetGroup",
        IdsParam: "@pVarAssetGroupIDs",
        NameOf: w => w.AssetGroup,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarAssetGroup", w.AssetGroup);
            p.Add("@pVarDescription", w.Description ?? "");
        });
}

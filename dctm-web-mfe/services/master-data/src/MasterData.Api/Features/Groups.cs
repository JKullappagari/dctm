using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns from iAssetTrack_Sp_Group_List (legacy Group.aspx grid).
public sealed class GroupRow
{
    public int GroupID { get; set; }
    public string Group { get; set; } = "";
    public string? Description { get; set; }
}

public sealed record GroupWrite(string Group, string? Description);

public static class Groups
{
    public static readonly LookupSpMap<GroupWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_Group_List",
        UpsertSp: "iAssetTrack_Sp_Group_Update",
        DeleteSp: "iAssetTrack_Sp_Group_Delete",
        ExistsSp: "iAssetTrack_Sp_Group_DoesExist",
        IdParam: "@pIntGroupID",
        NameParam: "@pVarGroup",
        IdsParam: "@pVarGroupIDs",
        NameOf: w => w.Group,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarGroup", w.Group);
            p.Add("@pVarDescription", w.Description ?? "");
        });
}

using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns as returned by iAssetTrack_Sp_Purpose_List (legacy Purpose.aspx grid binding).
public sealed class PurposeRow
{
    public int PurposeID { get; set; }
    public string Purpose { get; set; } = "";
    public string? Description { get; set; }
}

public sealed record PurposeWrite(string Purpose, string? Description);

public static class Purposes
{
    // Contract ported from iAssetTrackBAL/PurposeBAL.cs + Constants.cs.
    public static readonly LookupSpMap<PurposeWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_Purpose_List",
        UpsertSp: "iAssetTrack_Sp_Purpose_Update",
        DeleteSp: "iAssetTrack_Sp_Purpose_Delete",
        ExistsSp: "iAssetTrack_Sp_Purpose_DoesExist",
        IdParam: "@pIntPurposeID",
        NameParam: "@pVarPurpose",
        IdsParam: "@pVarPurposeIDs",
        NameOf: w => w.Purpose,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarPurpose", w.Purpose);
            p.Add("@pVarDescription", w.Description ?? "");
        });
}

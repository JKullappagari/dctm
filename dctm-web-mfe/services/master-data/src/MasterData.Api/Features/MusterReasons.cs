using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns as returned by iAssetTrack_Sp_MusterReason_List (legacy MusterReason.aspx grid binding).
public sealed class MusterReasonRow
{
    public int MusterReasonID { get; set; }
    public string MusterReason { get; set; } = "";
    public string? Description { get; set; }
}

public sealed record MusterReasonWrite(string MusterReason, string? Description);

public static class MusterReasons
{
    // Contract ported from iAssetTrackBAL/MusterReasonBAL.cs + Constants.cs.
    public static readonly LookupSpMap<MusterReasonWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_MusterReason_List",
        UpsertSp: "iAssetTrack_Sp_MusterReason_Update",
        DeleteSp: "iAssetTrack_Sp_MusterReason_Delete",
        ExistsSp: "iAssetTrack_Sp_MusterReason_DoesExist",
        IdParam: "@pIntMusterReasonID",
        NameParam: "@pVarMusterReason",
        IdsParam: "@pVarMusterReasonIDs",
        NameOf: w => w.MusterReason,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarMusterReason", w.MusterReason);
            p.Add("@pVarDescription", w.Description ?? "");
        });
}

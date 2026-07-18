using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns from iAssetTrack_Sp_ApplCriticality_List (legacy ApplicationCriticality.aspx grid).
public sealed class ApplicationCriticalityRow
{
    public int ApplCriticalityID { get; set; }
    public string ApplCriticality { get; set; } = "";
    public string? ApplCriticalityDesc { get; set; }
    public string? BackColorCOde { get; set; }
    public string? ForeColorCode { get; set; }
}

public sealed record ApplicationCriticalityWrite(
    string ApplCriticality, string? Description, string? BackColor, string? ForeColor);

public static class ApplicationCriticalities
{
    public static readonly LookupSpMap<ApplicationCriticalityWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_ApplCriticality_List",
        UpsertSp: "iAssetTrack_Sp_ApplCriticality_Update",
        DeleteSp: "iAssetTrack_Sp_ApplCriticality_Delete",
        ExistsSp: "iAssetTrack_Sp_ApplCriticality_DoesExist",
        IdParam: "@pIntApplCriticalityID",
        NameParam: "@pVarApplCriticality",
        IdsParam: "@pVarApplCriticalityIDs",
        NameOf: w => w.ApplCriticality,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarApplCriticality", w.ApplCriticality);
            p.Add("@pVarApplCriticalityDesc", w.Description ?? "");
            p.Add("@pVarBackColorCode", w.BackColor ?? "");
            p.Add("@pVarForeColorCode", w.ForeColor ?? "");
        });
}

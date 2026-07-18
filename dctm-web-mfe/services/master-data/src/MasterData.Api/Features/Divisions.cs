using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns as returned by iAssetTrack_Sp_Division_List (legacy Division.aspx grid binding).
public sealed class DivisionRow
{
    public int DivisionID { get; set; }
    public string Division { get; set; } = "";
    public string? DivisionDesc { get; set; }
}

public sealed record DivisionWrite(string Division, string? Description);

public static class Divisions
{
    public static readonly LookupSpMap<DivisionWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_Division_List",
        UpsertSp: "iAssetTrack_Sp_Division_Update",
        DeleteSp: "iAssetTrack_Sp_Division_Delete",
        ExistsSp: "iAssetTrack_Sp_Division_DoesExist",
        IdParam: "@pIntDivisionID",
        NameParam: "@pVarDivision",
        IdsParam: "@pVarDivisionIDs",
        NameOf: w => w.Division,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarDivision", w.Division);
            p.Add("@pVarDivisionDesc", w.Description ?? "");
        });
}

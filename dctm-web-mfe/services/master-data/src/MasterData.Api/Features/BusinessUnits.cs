using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns as returned by iAssetTrack_Sp_BusinessUnit_List (legacy BusinessUnit.aspx grid binding).
public sealed class BusinessUnitRow
{
    public int BusinessUnitID { get; set; }
    public string BusinessUnit { get; set; } = "";
    public string? CoPrefix { get; set; }
    public string? Description { get; set; }
}

public sealed record BusinessUnitWrite(string BusinessUnit, string? Description, string? CoPrefix);

public static class BusinessUnits
{
    // Contract verified against the restored DB (sys.parameters), not just the BAL:
    // the Update SP has a non-defaulted @pbitFromIA flag that must be supplied.
    public static readonly LookupSpMap<BusinessUnitWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_BusinessUnit_List",
        UpsertSp: "iAssetTrack_Sp_BusinessUnit_Update",
        DeleteSp: "iAssetTrack_Sp_BusinessUnit_Delete",
        ExistsSp: "iAssetTrack_Sp_BusinessUnit_DoesExist",
        IdParam: "@pIntBusinessUnitID",
        NameParam: "@pVarBusinessUnit",
        IdsParam: "@pVarBusinessUnitIDs",
        NameOf: w => w.BusinessUnit,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarBusinessUnit", w.BusinessUnit);
            p.Add("@pVarDescription", w.Description ?? "");
            p.Add("@pVarCoPrefix", w.CoPrefix ?? "");
            p.Add("@pbitFromIA", false);
        });
}

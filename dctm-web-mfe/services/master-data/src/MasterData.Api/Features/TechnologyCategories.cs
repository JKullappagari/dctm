using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns as returned by iAssetTrack_Sp_TechCat_List (legacy TechnologyCategory.aspx grid binding).
public sealed class TechnologyCategoryRow
{
    public int TechID { get; set; }
    public string TechName { get; set; } = "";
    public string? Description { get; set; }
}

public sealed record TechnologyCategoryWrite(string TechName, string? Description);

public static class TechnologyCategories
{
    public static readonly LookupSpMap<TechnologyCategoryWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_TechCat_List",
        UpsertSp: "iAssetTrack_Sp_TechCat_Update",
        DeleteSp: "iAssetTrack_Sp_TechCat_Delete",
        ExistsSp: "iAssetTrack_Sp_TechCat_DoesExist",
        IdParam: "@pIntTechID",
        NameParam: "@pVarTechName",
        IdsParam: "@pVartechIDs",
        NameOf: w => w.TechName,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarTechName", w.TechName);
            p.Add("@pVarDescription", w.Description ?? "");
        });
}

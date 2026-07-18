using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Selected columns from iAssetTrack_Sp_AssetModel_List (legacy AssetModel.aspx grid + FK preselect).
public sealed class AssetModelRow
{
    public int ModelID { get; set; }
    public string ModelName { get; set; } = "";
    public string? Description { get; set; }
    public string? MfgName { get; set; }
    public int MfgID { get; set; }
    public string? ModelType { get; set; }
    public int? AssetTypeID { get; set; }
    public string? TechName { get; set; }
    public int? TechID { get; set; }
    public int? BusinessUnitID { get; set; }
    public string? BusinessUnit { get; set; }
    public string? MountType { get; set; }
    public int? MountTypeID { get; set; }
    public string? AirFlowDirection { get; set; }
    public int? AirFlowDirectionID { get; set; }
    public int? UHeight { get; set; }
    public double? MaxPower_Watts { get; set; }
    public bool IsBlade { get; set; }
    public bool IsEnclosure { get; set; }
    public int AssetCount { get; set; }
}

public sealed record AssetModelWrite(
    string ModelName,
    string? Description,
    int MfgId,
    int? ModelTypeId,
    int? TechId,
    int? BuId,
    int? MountTypeId,
    int? AfDirectionId,
    int? UHeight,
    double? MaxPower,
    bool IsBlade,
    bool IsEnclosure,
    string? Comment);

public static class AssetModels
{
    // AssetModel_Update has 37 non-defaulted params. The modern form exposes the
    // meaningful core (name, mfg, model-type, tech, BU, mount, airflow, U-height,
    // max power, blade/enclosure, comment); the many physical/enclosure/blade/PSU
    // dimension params pass safe defaults until an "advanced" section is added.
    // DoesExist is composite: model name unique per (Mfg, BU).
    public static readonly LookupSpMap<AssetModelWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_AssetModel_List",
        UpsertSp: "iAssetTrack_Sp_AssetModel_Update",
        DeleteSp: "iAssetTrack_Sp_AssetModel_Delete",
        ExistsSp: "iAssetTrack_Sp_AssetModel_DoesExist",
        IdParam: "@pIntModelID",
        NameParam: "@pVarModelName",
        IdsParam: "@pVarModelIDs",
        NameOf: w => w.ModelName,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarModelName", w.ModelName);
            p.Add("@pVarDescription", w.Description ?? "");
            p.Add("@pIntMfgID", w.MfgId);
            p.Add("@pIntSpcID", 0L);
            p.Add("@pIntTechID", w.TechId ?? 0);
            p.Add("@pVarComment", w.Comment ?? "");
            p.Add("@pIntBuID", w.BuId ?? 0);
            p.Add("@pBitIsBlade", w.IsBlade);
            p.Add("@pBitIsEnclosure", w.IsEnclosure);
            p.Add("@pIntModelTypeID", w.ModelTypeId ?? 0);
            p.Add("@pFltWidth", 0.0);
            p.Add("@pFltDepth", 0.0);
            p.Add("@pFltHeight", 0.0);
            p.Add("@pIntUHeight", w.UHeight ?? 0);
            p.Add("@pFltWeight", 0.0);
            p.Add("@pFltMaxPower", w.MaxPower ?? 0.0);
            p.Add("@pFltSSPower", 0.0);
            p.Add("@pVarConnPDUSide", "");
            p.Add("@pVarConnDevSide", "");
            p.Add("@pIntTotalPSUCount", 0);
            p.Add("@pIntReqPSUCount", 0);
            p.Add("@pIntMountTypeID", w.MountTypeId ?? 0);
            p.Add("@pIntAFDirectionID", w.AfDirectionId ?? 0);
            p.Add("@pFltRInternalDepth", 0.0);
            p.Add("@pFltRInternalHeight", 0.0);
            p.Add("@pIntEnclFrontRowCount", 0);
            p.Add("@pIntEnclFrontColCount", 0);
            p.Add("@pIntEnclRearRowCount", 0);
            p.Add("@pIntEnclRearColCount", 0);
            p.Add("@pIntBladeRowCount", 0);
            p.Add("@pIntBladeColCount", 0);
            p.Add("@pFltRInternalWidth", 0.0);
        },
        AddExistsParams: (p, w) =>
        {
            p.Add("@pVarModelName", w.ModelName);
            p.Add("@pIntMfgID", w.MfgId);
            p.Add("@pIntBuID", w.BuId ?? 0);
        });
}

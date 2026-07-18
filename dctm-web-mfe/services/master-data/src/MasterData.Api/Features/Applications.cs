using Dapper;
using MasterData.Api.Common;
using MasterData.Api.Infrastructure;

namespace MasterData.Api.Features;

// Columns from iAssetTrack_Sp_Application_List (legacy CreateApplication.aspx grid).
public sealed class ApplicationRow
{
    public int ApplID { get; set; }
    public string ApplName { get; set; } = "";
    public string? ApplDesc { get; set; }
    public int? BUID { get; set; }
    public string? BusinessUnit { get; set; }
    public int? ApplTypeID { get; set; }
    public string? ApplType { get; set; }
    public int? ApplCriticalityID { get; set; }
    public int? OwnerID { get; set; }
    public string? Owner { get; set; }
    public int? AppStatusID { get; set; }
    public string? ApplStatus { get; set; }
}

public sealed record ApplicationWrite(
    string ApplName, string? Description, int? BuId, int? ApplTypeId,
    int? CriticalityId, int? OwnerId, int? AppStatusId);

public static class Applications
{
    // Contract from iAssetTrack_Sp_Application_{List,Update,Delete,DoesExist}.
    // DoesExist is composite: name + type + owner + status.
    public static readonly LookupSpMap<ApplicationWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_Application_List",
        UpsertSp: "iAssetTrack_Sp_Application_Update",
        DeleteSp: "iAssetTrack_Sp_Application_Delete",
        ExistsSp: "iAssetTrack_Sp_Application_DoesExist",
        IdParam: "@pIntApplID",
        NameParam: "@pVarApplName",
        IdsParam: "@pVarApplIDs",
        NameOf: w => w.ApplName,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarApplName", w.ApplName);
            p.Add("@pVarApplDesc", w.Description ?? "");
            p.Add("@pIntBUID", w.BuId ?? 0);
            p.Add("@pIntApplTypeID", w.ApplTypeId ?? 0);
            p.Add("@pIntApplCriticality", w.CriticalityId ?? 0);
            p.Add("@pIntOwnerID", w.OwnerId ?? 0);
            p.Add("@pIntApplManageID", 0);
            p.Add("@pIntAppStatusID", w.AppStatusId ?? 0);
        },
        AddExistsParams: (p, w) =>
        {
            p.Add("@pVarApplName", w.ApplName);
            p.Add("@pIntApplTypeID", w.ApplTypeId ?? 0);
            p.Add("@pIntOwnerID", w.OwnerId ?? 0);
            p.Add("@pIntAppStatusID", w.AppStatusId ?? 0);
        });

    /// <summary>
    /// App status options. The legacy iAssetTrack_Sp_Application_Status_List is broken
    /// (selects a misspelled 'ApplStatus' column from tblAppStatus), so this reads the
    /// table directly — same values the legacy dropdown was meant to show.
    /// </summary>
    public static void MapApplicationRefs(this IEndpointRouteBuilder api)
    {
        api.MapGet("/ref/app-statuses", async (IDbConnectionFactory factory) =>
        {
            using var connection = factory.Create();
            var rows = await connection.QueryAsync(
                "SELECT AppStatusID, AppStatus FROM tblAppStatus ORDER BY AppStatusID");
            return Results.Ok(rows);
        }).WithTags("Reference");
    }
}

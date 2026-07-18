using System.Data;
using Dapper;
using MasterData.Api.Common;
using MasterData.Api.Infrastructure;

namespace MasterData.Api.Features;

public sealed record AssignmentItem(int Id, string Name);
public sealed record AssignmentView(IReadOnlyList<AssignmentItem> Assigned, IReadOnlyList<AssignmentItem> Available);
public sealed record SaveAssignment(int[] Ids);

/// <summary>
/// One legacy dual-list assignment page (assign a set of child items to a parent).
/// GET returns assigned + available; PUT replaces the assignment via the legacy
/// Update SP, which takes a delimited id list. The parent id is carried as a string so
/// int-keyed parents (business units, sites) and GUID-keyed parents (hosts, for the
/// Application Map) share the same machinery.
/// </summary>
public sealed record AssignmentType(
    string Path,
    string Tag,
    string AllItemsSp, string AllItemsIdCol, string AllItemsNameCol,
    string AssignedSp, string AssignedParentCol, string AssignedIdCol, string AssignedNameCol,
    string UpdateSp,
    // (params, parentId, csv, parentContext) — parentContext is 0 unless ParentContextSql resolves it.
    Action<DynamicParameters, string, string, int> BuildUpdate,
    // Optional SQL resolving an int context from @parentId (e.g. a site's BU for
    // SiteLocationAssignment_Update, which needs the BU alongside the site).
    string? ParentContextSql = null,
    // Optional SQL run (with @parentId) before the Update SP. Used where the legacy
    // Update SP only adds/reactivates rows and never removes them (ApplicationMap):
    // clearing first gives the dual-list true replace semantics.
    string? PreUpdateSql = null,
    // Parameters for the all-items List SP when it has no default-valued parameters
    // (e.g. Application_List requires @pIntApplID).
    object? AllItemsParams = null,
    // Module gating this page's rights (View to read, SaveRight to save), when enforced.
    string? Module = null,
    // The right the save (PUT) requires. Most pages use Modify; Application Map uses "Map".
    string SaveRight = RequirePermissionExtensions.Modify);

public static class Assignments
{
    private static readonly AssignmentType[] Types =
    [
        new("bu-divisions", "Assignments",
            AllItemsSp: "iAssetTrack_Sp_Division_List", AllItemsIdCol: "DivisionID", AllItemsNameCol: "Division",
            AssignedSp: "iAssetTrack_Sp_BUDivAssignment_List", AssignedParentCol: "BusinessUnitID",
            AssignedIdCol: "DivisionID", AssignedNameCol: "Division",
            UpdateSp: "iAssetTrack_Sp_BUDivAssignment_Update",
            BuildUpdate: (p, parentId, csv, _) =>
            {
                p.Add("@pIntBusinessUnitID", int.Parse(parentId));
                p.Add("@pIntDivisionAccessID", 0);
                p.Add("@pVarDelimiter", ",");
                p.Add("@pVarDivisionIDs", csv);
                p.Add("@pBitStatus", true);
            }),
        new("bu-sites", "Assignments",
            AllItemsSp: "iAssetTrack_Sp_Site_List", AllItemsIdCol: "SiteID", AllItemsNameCol: "Site",
            AssignedSp: "iAssetTrack_Sp_BUSiteAssignment_List", AssignedParentCol: "BusinessUnitID",
            AssignedIdCol: "SiteID", AssignedNameCol: "Site",
            UpdateSp: "iAssetTrack_Sp_BUSiteAssignment_Update",
            BuildUpdate: (p, parentId, csv, _) =>
            {
                p.Add("@pIntBusinessUnitID", int.Parse(parentId));
                p.Add("@pIntSiteAccessID", 0);
                p.Add("@pVarDelimiter", ",");
                p.Add("@pVarSiteIDs", csv);
                p.Add("@pBitStatus", true);
            }),
        new("site-locations", "Assignments",
            AllItemsSp: "iAssetTrack_Sp_Location_List", AllItemsIdCol: "LocationID", AllItemsNameCol: "Location",
            AssignedSp: "iAssetTrack_Sp_SiteLocationAssignment_List", AssignedParentCol: "SiteID",
            AssignedIdCol: "LocationID", AssignedNameCol: "Location",
            UpdateSp: "iAssetTrack_Sp_SiteLocationAssignment_Update_New",
            // The Update SP needs the site's BU alongside the site; resolve it here.
            ParentContextSql: "SELECT TOP 1 BusinessUnitID FROM tblBUSiteAssignment WHERE SiteID = @parentId AND Status = 1",
            BuildUpdate: (p, parentId, csv, buId) =>
            {
                p.Add("@pIntBusinessUnitID", buId);
                p.Add("@pIntSiteID", int.Parse(parentId));
                p.Add("@pVarDelimiter", ",");
                p.Add("@pVarLocationIDs", csv);
                p.Add("@pBitStatus", true);
            }),
        // retires ApplicationMap.aspx — assign Applications to a Host (GUID-keyed).
        new("app-map", "Assignments",
            AllItemsSp: "iAssetTrack_Sp_Application_List", AllItemsIdCol: "ApplID", AllItemsNameCol: "ApplName",
            AssignedSp: "iAssetTrack_Sp_ApplMap_List", AssignedParentCol: "ID",
            AssignedIdCol: "ApplID", AssignedNameCol: "ApplName",
            UpdateSp: "iAssetTrack_Sp_ApplicationMap_Update",
            // The legacy Update SP only inserts/reactivates the given ApplIDs and never
            // deactivates removed ones, so clear the host's map first for replace semantics.
            PreUpdateSql: "UPDATE tblApplicationMap SET Status = 0 WHERE ID = @parentId",
            AllItemsParams: new { pIntApplID = 0 },
            Module: "Application Map",
            SaveRight: RequirePermissionExtensions.Map,
            BuildUpdate: (p, parentId, csv, _) =>
            {
                p.Add("@pGuidID", parentId);
                p.Add("@pVarDelimiter", ",");
                p.Add("@pVarApplIDs", csv);
            }),
    ];

    public static void MapAssignments(this IEndpointRouteBuilder api)
    {
        foreach (var type in Types)
        {
            var group = api.MapGroup($"/api/v1/assignments/{type.Path}").WithTags(type.Tag);

            group.MapGet("/{parentId}", async (string parentId, IDbConnectionFactory factory) =>
                Results.Ok(await LoadAsync(type, parentId, factory)))
                .Guard(type.Module, RequirePermissionExtensions.View);

            group.MapPut("/{parentId}", async (string parentId, SaveAssignment body, HttpContext ctx, IDbConnectionFactory factory) =>
            {
                using var connection = factory.Create();
                var csv = string.Join(",", body.Ids.Distinct());

                if (type.PreUpdateSql is not null)
                    await connection.ExecuteAsync(type.PreUpdateSql, new { parentId });

                // With nothing selected the pre-step has already cleared the map, and the
                // legacy delimited-id SPs choke on an empty list — so skip the Update call.
                if (type.PreUpdateSql is not null && csv.Length == 0)
                    return Results.NoContent();

                var context = type.ParentContextSql is null
                    ? 0
                    : await connection.ExecuteScalarAsync<int?>(type.ParentContextSql, new { parentId }) ?? 0;

                var parameters = new DynamicParameters();
                type.BuildUpdate(parameters, parentId, csv, context);
                parameters.Add("@pIntCreatedBy", await Audit.UserAsync(ctx));
                await connection.ExecuteAsync(type.UpdateSp, parameters, commandType: CommandType.StoredProcedure);
                return Results.NoContent();
            }).Guard(type.Module, type.SaveRight);
        }
    }

    // Applies the permission filter only when a module is configured for the page.
    private static RouteHandlerBuilder Guard(this RouteHandlerBuilder builder, string? module, string right) =>
        module is null ? builder : builder.RequirePermission(module, right);

    private static async Task<AssignmentView> LoadAsync(AssignmentType type, string parentId, IDbConnectionFactory factory)
    {
        using var connection = factory.Create();

        // The List SP returns the id/name lists directly, so map through DapperRow by column name.
        var all = (await connection.QueryAsync(type.AllItemsSp, type.AllItemsParams, commandType: CommandType.StoredProcedure))
            .Select(r => new AssignmentItem((int)Row(r, type.AllItemsIdCol), Row(r, type.AllItemsNameCol)?.ToString() ?? ""))
            .ToList();

        // Compare the parent column as a string so int and GUID parents both work.
        var assignedIds = (await connection.QueryAsync(type.AssignedSp, commandType: CommandType.StoredProcedure))
            .Where(r => string.Equals(Row(r, type.AssignedParentCol)?.ToString(), parentId, StringComparison.OrdinalIgnoreCase))
            .Select(r => (int)Row(r, type.AssignedIdCol))
            .ToHashSet();

        var assigned = all.Where(i => assignedIds.Contains(i.Id)).ToList();
        var available = all.Where(i => !assignedIds.Contains(i.Id)).ToList();
        return new AssignmentView(assigned, available);
    }

    private static object Row(dynamic row, string column) => ((IDictionary<string, object>)row)[column];
}

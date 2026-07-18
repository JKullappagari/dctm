using System.Data;
using Dapper;
using MasterData.Api.Common;
using MasterData.Api.Infrastructure;

namespace MasterData.Api.Features;

/// <summary>
/// Import Asset-App Map (ImportAppAssetMap.aspx): upload an .xlsx of Application ↔ Host
/// rows, resolve both names to their ids/GUIDs, and add each application to its host's
/// application map via the same legacy Update SP the Application Map page uses. The SP is
/// additive (it never removes existing mappings), so an import only ever grows the map.
/// Rows are grouped by host so each host's applications are applied in one SP call.
/// </summary>
public static class ImportAppAssetMap
{
    private static readonly string[] Headers = ["Application", "HostName"];

    public static void MapImportAppAssetMap(this IEndpointRouteBuilder api)
    {
        api.MapGet("/import/app-asset-map/template", () =>
            ImportSupport.Template("AppAssetMap", Headers))
            .RequirePermission("Import Asset-App Map", RequirePermissionExtensions.View);

        api.MapPost("/import/app-asset-map", async (
            IFormFile file, HttpContext ctx, IDbConnectionFactory factory) =>
        {
            if (file is null || file.Length == 0)
                return Results.BadRequest(new { error = "No file uploaded." });

            using var connection = factory.Create();
            var apps = await ImportSupport.NameMap(connection, "SELECT ApplID, ApplName FROM tblApplication WHERE Status = 1");
            var hosts = await HostMap(connection);
            var userId = await Audit.UserAsync(ctx);

            // Validate every row first, collecting the resolved (host → app ids) grouping.
            var results = new List<ImportRowResult>();
            var byHost = new Dictionary<Guid, (List<int> AppIds, List<int> RowNums)>();

            foreach (var dataRow in ImportSupport.DataRows(file))
            {
                var num = dataRow.Number;
                var appName = dataRow.Col("Application") ?? "";
                var hostName = dataRow.Col("HostName") ?? "";
                if (string.IsNullOrWhiteSpace(appName) || string.IsNullOrWhiteSpace(hostName))
                {
                    results.Add(new(num, appName, "error", "Application and HostName are required."));
                    continue;
                }
                if (!apps.TryGetValue(appName, out var appId))
                {
                    results.Add(new(num, appName, "error", $"Unknown application '{appName}'."));
                    continue;
                }
                if (!hosts.TryGetValue(hostName, out var hostId))
                {
                    results.Add(new(num, appName, "error", $"Unknown host '{hostName}'."));
                    continue;
                }
                var bucket = byHost.TryGetValue(hostId, out var b) ? b : (byHost[hostId] = (new(), new()));
                bucket.AppIds.Add(appId);
                bucket.RowNums.Add(num);
            }

            var imported = 0;
            foreach (var (hostId, bucket) in byHost)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@pGuidID", hostId.ToString());
                parameters.Add("@pVarDelimiter", ",");
                parameters.Add("@pVarApplIDs", string.Join(",", bucket.AppIds.Distinct()));
                parameters.Add("@pIntCreatedBy", userId);
                try
                {
                    await connection.ExecuteAsync("iAssetTrack_Sp_ApplicationMap_Update", parameters,
                        commandType: CommandType.StoredProcedure);
                    foreach (var (num, i) in bucket.RowNums.Select((n, i) => (n, i)))
                    {
                        imported++;
                        results.Add(new(num, "", "imported", null));
                    }
                }
                catch (Exception ex)
                {
                    foreach (var num in bucket.RowNums)
                        results.Add(new(num, "", "error", ex.Message));
                }
            }

            return Results.Ok(new ImportResult(
                results.Count, imported, results.Count(r => r.Status == "error"),
                results.OrderBy(r => r.Row).ToList()));
        }).RequirePermission("Import Asset-App Map", RequirePermissionExtensions.Create).DisableAntiforgery();
    }

    private static async Task<Dictionary<string, Guid>> HostMap(IDbConnection c)
    {
        var map = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
        foreach (var (id, name) in await c.QueryAsync<(Guid, string)>(
            "SELECT HostID, HostName FROM tblHost WHERE Status = 1"))
            if (!string.IsNullOrWhiteSpace(name)) map[name.Trim()] = id;
        return map;
    }
}

using System.Data;
using Dapper;
using MasterData.Api.Common;
using MasterData.Api.Infrastructure;

namespace MasterData.Api.Features;

// Columns from iAssetTrack_Sp_Host_List (legacy Host.aspx / HostPopup.aspx grid).
public sealed class HostRow
{
    public Guid HostID { get; set; }
    public string HostName { get; set; } = "";
    public string? Description { get; set; }
}

public sealed record HostWrite(string HostName, string? Description);

/// <summary>
/// Host is the one master-data entity keyed by a GUID rather than an int, so it
/// can't ride the int-based generic lookup machinery. It gets a dedicated (but
/// tiny) endpoint set that still reuses the shared ClosedXML export and the
/// same audit-user resolution. Legacy DoesExist reactivation returns the
/// soft-deleted GUID, mirrored here.
/// </summary>
public static class Hosts
{
    private const string Module = "Host";

    public static void MapHosts(this IEndpointRouteBuilder api)
    {
        var group = api.MapGroup("/hosts").WithTags("Hosts");

        group.MapGet("/", async (IDbConnectionFactory factory) =>
        {
            using var connection = factory.Create();
            var rows = await connection.QueryAsync<HostRow>(
                "iAssetTrack_Sp_Host_List",
                new { pGuidHostID = (string?)null },
                commandType: CommandType.StoredProcedure);
            return Results.Ok(rows);
        }).RequirePermission(Module, RequirePermissionExtensions.View);

        group.MapGet("/{id:guid}", async (Guid id, IDbConnectionFactory factory) =>
        {
            using var connection = factory.Create();
            var rows = (await connection.QueryAsync<HostRow>(
                "iAssetTrack_Sp_Host_List",
                new { pGuidHostID = id.ToString() },
                commandType: CommandType.StoredProcedure)).AsList();
            return rows.Count == 0 ? Results.NotFound() : Results.Ok(rows[0]);
        }).RequirePermission(Module, RequirePermissionExtensions.View);

        group.MapPost("/", async (HostWrite dto, HttpContext context, IDbConnectionFactory factory) =>
        {
            if (string.IsNullOrWhiteSpace(dto.HostName))
                return Results.BadRequest(new { error = "Host name is required." });

            using var connection = factory.Create();
            var check = await CheckAsync(connection, null, dto.HostName);
            if (check == "-1")
                return Results.Conflict(new { error = $"Host '{dto.HostName}' already exists." });

            // check is a GUID string when reactivating a soft-deleted host, else "0".
            var reactivateId = check != "0" ? check : null;
            var id = await UpsertAsync(connection, reactivateId, dto, await Audit.UserAsync(context));
            return Results.Created($"{context.Request.Path}/{id}", new { id, reactivated = reactivateId != null });
        }).RequirePermission(Module, RequirePermissionExtensions.Create);

        group.MapPut("/{id:guid}", async (Guid id, HostWrite dto, HttpContext context, IDbConnectionFactory factory) =>
        {
            if (string.IsNullOrWhiteSpace(dto.HostName))
                return Results.BadRequest(new { error = "Host name is required." });

            using var connection = factory.Create();
            if (await CheckAsync(connection, id.ToString(), dto.HostName) == "-1")
                return Results.Conflict(new { error = $"Host '{dto.HostName}' already exists." });

            await UpsertAsync(connection, id.ToString(), dto, await Audit.UserAsync(context));
            return Results.NoContent();
        }).RequirePermission(Module, RequirePermissionExtensions.Modify);

        group.MapDelete("/", async (string? ids, HttpContext context, IDbConnectionFactory factory) =>
        {
            // Host_Delete concatenates @pVarHostIDs straight into dynamic SQL, and
            // HostID is a uniqueidentifier — so ids must be a quoted, comma-separated
            // list ('guid','guid'). Parsing to Guid first also blocks SQL injection
            // through that dynamic exec.
            var parsed = new List<Guid>();
            foreach (var part in (ids ?? "").Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                if (!Guid.TryParse(part, out var g))
                    return Results.BadRequest(new { error = "Query parameter 'ids' must be a comma-separated list of GUIDs." });
                parsed.Add(g);
            }
            if (parsed.Count == 0)
                return Results.BadRequest(new { error = "Query parameter 'ids' is required (comma-separated GUIDs)." });

            using var connection = factory.Create();
            var parameters = new DynamicParameters();
            parameters.Add("@pVarHostIDs", string.Join(",", parsed.Select(g => $"'{g}'")));
            parameters.Add("@pBitStatus", 0);
            parameters.Add("@pIntLastmodifiedBy", await Audit.UserAsync(context));
            await connection.ExecuteAsync(
                "iAssetTrack_Sp_Host_Delete", parameters, commandType: CommandType.StoredProcedure);
            return Results.NoContent();
        }).RequirePermission(Module, RequirePermissionExtensions.Delete);

        group.MapGet("/export", async (IDbConnectionFactory factory) =>
        {
            using var connection = factory.Create();
            var rows = await connection.QueryAsync<HostRow>(
                "iAssetTrack_Sp_Host_List",
                new { pGuidHostID = (string?)null },
                commandType: CommandType.StoredProcedure);
            return ExcelExport.Sheet(rows, "Hosts");
        }).RequirePermission(Module, RequirePermissionExtensions.View);
    }

    // Returns "-1" (active duplicate), "0" (free), or a GUID string (soft-deleted id to reactivate).
    private static async Task<string> CheckAsync(IDbConnection connection, string? id, string name)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@pGuidHostID", id ?? "");
        parameters.Add("@pVarHost", name);
        var result = await connection.ExecuteScalarAsync<object?>(
            "iAssetTrack_Sp_Host_DoesExist", parameters, commandType: CommandType.StoredProcedure);
        return result?.ToString() ?? "0";
    }

    private static async Task<string> UpsertAsync(IDbConnection connection, string? id, HostWrite dto, int userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@pVarHost", dto.HostName);
        parameters.Add("@pVarDescription", dto.Description ?? "");
        parameters.Add("@pBitStatus", 1);
        parameters.Add("@pIntCreatedBy", userId);
        parameters.Add("@pGuidHostID", id, DbType.String, ParameterDirection.InputOutput, size: 40);
        await connection.ExecuteAsync(
            "iAssetTrack_Sp_Host_Update", parameters, commandType: CommandType.StoredProcedure);
        return parameters.Get<string>("@pGuidHostID");
    }
}

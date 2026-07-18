using System.Data;
using Asset.Api.Common;
using Asset.Api.Infrastructure;
using Dapper;

namespace Asset.Api.Features;

// One RFID stock-take session at a location (iAssetTrack_Sp_InventoryUpdate_Data).
// Property names differ only in case from the SP's ALL-CAPS columns, so Dapper's
// case-insensitive matching binds them while the JSON stays clean camelCase.
public sealed class InventorySession
{
    public Guid Id { get; set; }
    public DateTime? StDateTime { get; set; }
    public int LocationId { get; set; }
    public long StSessionId { get; set; }
    public string? Company { get; set; }
    public string? Site { get; set; }
    public string? Location { get; set; }
    public int AssetCount { get; set; }
    public int UntaggedCount { get; set; }
    public int ScannedCount { get; set; }
    public int MissingCount { get; set; }
    public int OverscannedCount { get; set; }
    public int FinalCount { get; set; }
    public string? InvUser { get; set; }
    public string? DeviceName { get; set; }
}

/// <summary>
/// Inventory Update (InventoryUpdate.aspx): RFID stock-take reconciliation. The sessions
/// list comes from the physical RFID scans (tblStockTakeSession, populated by handheld
/// devices). Commit runs the legacy reconcile SP, which moves/marks assets per scan
/// results — it mutates asset data, so it's gated on the Inventory Update right and only
/// commits a session's not-yet-processed scanned items.
/// </summary>
public static class Inventory
{
    private const string Module = "Inventory Update";

    public static void MapInventory(this IEndpointRouteBuilder app)
    {
        var inv = app.MapGroup("/api/v1/inventory").WithTags("Inventory");

        inv.MapGet("/", async (string? locations, int? page, int? size, IDbConnectionFactory factory) =>
        {
            using var connection = factory.Create();
            var rows = await connection.QueryAsync<InventorySession>(
                "iAssetTrack_Sp_InventoryUpdate_Data",
                new { LocList = locations ?? "", PageNum = page ?? 1, PageSize = size ?? 100 },
                commandType: CommandType.StoredProcedure);
            return Results.Ok(rows);
        }).RequirePermission("View", Module);

        // Commit a stock-take session: reconcile its scanned (not-yet-processed) assets
        // through the legacy SP. Destructive — moves/updates asset records.
        inv.MapPost("/sessions/{sessionId:guid}/commit", async (
            Guid sessionId, HttpContext ctx, IDbConnectionFactory factory) =>
        {
            using var connection = factory.Create();
            var stSessionId = await connection.ExecuteScalarAsync<long?>(
                "SELECT StockTakeSessionId FROM tblStockTakeSession WHERE ID = @sessionId",
                new { sessionId = sessionId.ToString() });
            if (stSessionId is null) return Results.NotFound();

            var assetIds = (await connection.QueryAsync<long>(
                "SELECT AssetId FROM tblStockTakeItems WHERE StockTakeSessionId = @stSessionId AND IsUpdated = 0",
                new { stSessionId })).ToList();
            if (assetIds.Count == 0)
                return Results.Ok(new { committed = 0, message = "Nothing to reconcile — session already processed." });

            var p = new DynamicParameters();
            p.Add("@pVarID", sessionId.ToString());
            p.Add("@pStrAssetIDs", string.Join(",", assetIds));
            p.Add("@pIntCreatedBy", await Audit.UserAsync(ctx));
            await connection.ExecuteAsync("iAssetTrack_Sp_InventoryUpdate", p, commandType: CommandType.StoredProcedure);

            return Results.Ok(new { committed = assetIds.Count });
        }).RequirePermission("Modify", Module);
    }
}

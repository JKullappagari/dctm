using Asset.Api.Common;
using Dapper;

namespace Asset.Api.Features;

// Lifecycle request bodies.
public sealed record ReasonRequest(string? Reason);
public sealed record WriteoffRequest(string? Reason, int? MusterReasonId);
public sealed record BarRequest(string? Reason, DateTime FromDate, DateTime ToDate);
public sealed record DecommissionRequest(string? Reason, int? MusterReasonId, DateTime? ExpiryDate);
public sealed record AssignRfidRequest(string CardNumber, string? RefNumber, string? Reason);

public static class AssetEndpoints
{
    public static void MapAssets(this IEndpointRouteBuilder app)
    {
        var assets = app.MapGroup("/api/v1/assets").WithTags("Assets");

        assets.MapGet("/", async (string? q, int? page, int? size, AssetStore store) =>
            Results.Ok(await store.SearchAsync(q, page ?? 1, size ?? 20)))
            .RequirePermission(AssetRights.View);

        assets.MapGet("/{id:int}", async (int id, AssetStore store) =>
            await store.GetAsync(id) is { } asset ? Results.Ok(asset) : Results.NotFound())
            .RequirePermission(AssetRights.View);

        // Current state conditions + the transitions currently permitted.
        assets.MapGet("/{id:int}/status", async (int id, AssetStore store) =>
        {
            var flags = await store.GetFlagsAsync(id);
            if (flags is null) return Results.NotFound();
            var now = DateTime.UtcNow;
            return Results.Ok(new
            {
                assetId = id,
                states = AssetLifecycle.States(flags, now),
                availableActions = AssetLifecycle.AvailableActions(flags, now),
            });
        }).RequirePermission(AssetRights.View);

        assets.MapPost("/{id:int}/writeoff", (int id, WriteoffRequest body, HttpContext ctx, AssetStore store) =>
            Transition(id, "writeoff", store, ctx, "iAssetTrack_Sp_Asset_Writeoff_Update", (p, uid) =>
            {
                p.Add("@pIntAssetID", id);
                p.Add("@pVarReason", body.Reason ?? "");
                p.Add("@pIntMusterReasonID", body.MusterReasonId ?? 0);
                p.Add("@pIntUpdatedBy", uid);
            })).RequirePermission(AssetRights.ForAction["writeoff"]);

        assets.MapPost("/{id:int}/reinstate", (int id, ReasonRequest body, HttpContext ctx, AssetStore store) =>
            Transition(id, "reinstate", store, ctx, "iAssetTrack_Sp_Asset_DeWriteoff", (p, uid) =>
                ReasonParams(p, id, body.Reason, uid))).RequirePermission(AssetRights.ForAction["reinstate"]);

        assets.MapPost("/{id:int}/restrict", (int id, ReasonRequest body, HttpContext ctx, AssetStore store) =>
            Transition(id, "restrict", store, ctx, "iAssetTrack_Sp_Asset_Restriction", (p, uid) =>
                ReasonParams(p, id, body.Reason, uid))).RequirePermission(AssetRights.ForAction["restrict"]);

        assets.MapPost("/{id:int}/de-restrict", (int id, ReasonRequest body, HttpContext ctx, AssetStore store) =>
            Transition(id, "de-restrict", store, ctx, "iAssetTrack_Sp_Asset_DeRestriction", (p, uid) =>
                ReasonParams(p, id, body.Reason, uid))).RequirePermission(AssetRights.ForAction["de-restrict"]);

        assets.MapPost("/{id:int}/bar", (int id, BarRequest body, HttpContext ctx, AssetStore store) =>
            Transition(id, "bar", store, ctx, "iAssetTrack_Sp_Asset_Barring", (p, uid) =>
            {
                p.Add("@pIntAssetID", id);
                p.Add("@pVarBarredReason", body.Reason ?? "");
                p.Add("@pDtBarredFromDate", body.FromDate);
                p.Add("@pDtBarredToDate", body.ToDate);
                p.Add("@pIntUpdatedBy", uid);
            })).RequirePermission(AssetRights.ForAction["bar"]);

        assets.MapPost("/{id:int}/un-bar", (int id, ReasonRequest body, HttpContext ctx, AssetStore store) =>
            Transition(id, "un-bar", store, ctx, "iAssetTrack_Sp_Asset_UnBarring", (p, uid) =>
            {
                p.Add("@pIntAssetID", id);
                p.Add("@pVarUnBarredReason", body.Reason ?? "");
                p.Add("@pIntUpdatedBy", uid);
            })).RequirePermission(AssetRights.ForAction["un-bar"]);

        assets.MapPost("/{id:int}/decommission", (int id, DecommissionRequest body, HttpContext ctx, AssetStore store) =>
            Transition(id, "decommission", store, ctx, "iAssetTrack_Sp_Asset_Muster", (p, uid) =>
                MusterParams(p, id, body.Reason, body.MusterReasonId, body.ExpiryDate, uid, "Decomm")))
            .RequirePermission(AssetRights.ForAction["decommission"]);

        assets.MapPost("/{id:int}/recommission", (int id, DecommissionRequest body, HttpContext ctx, AssetStore store) =>
            Transition(id, "recommission", store, ctx, "iAssetTrack_Sp_Asset_Muster", (p, uid) =>
                MusterParams(p, id, body.Reason, body.MusterReasonId, body.ExpiryDate, uid, "Recomm")))
            .RequirePermission(AssetRights.ForAction["recommission"]);

        assets.MapPost("/{id:int}/rfid-card", (int id, AssignRfidRequest body, HttpContext ctx, AssetStore store) =>
        {
            if (string.IsNullOrWhiteSpace(body.CardNumber))
                return Task.FromResult(Results.BadRequest(new { error = "Card number is required." }));
            return Transition(id, "assign-rfid", store, ctx, "iAssetTrack_Sp_RFIDCardAssignment_Update", (p, uid) =>
                RfidParams(p, id, body.RefNumber, body.CardNumber, body.Reason, uid, "RFID Tag Assigned"));
        }).RequirePermission(AssetRights.ForAction["assign-rfid"]);

        assets.MapDelete("/{id:int}/rfid-card", (int id, string? reason, HttpContext ctx, AssetStore store) =>
            Transition(id, "deassign-rfid", store, ctx, "iAssetTrack_Sp_RFIDCardAssignment_Update", (p, uid) =>
                RfidParams(p, id, null, "", reason, uid, "RFID Tag DeAssigned")))
            .RequirePermission(AssetRights.ForAction["deassign-rfid"]);
    }

    // Loads flags, checks the transition guard (409 on an invalid transition), then runs the SP.
    private static async Task<IResult> Transition(
        int id, string action, AssetStore store, HttpContext ctx,
        string sp, Action<DynamicParameters, int> build)
    {
        var flags = await store.GetFlagsAsync(id);
        if (flags is null) return Results.NotFound();

        var transition = AssetLifecycle.Find(action)!;
        if (!transition.CanApply(flags, DateTime.UtcNow))
            return Results.Conflict(new
            {
                error = $"Cannot '{action}' — not valid for the asset's current state.",
                states = AssetLifecycle.States(flags, DateTime.UtcNow),
            });

        var parameters = new DynamicParameters();
        build(parameters, await Audit.UserAsync(ctx));
        await store.ExecAsync(sp, parameters);

        var after = await store.GetFlagsAsync(id);
        return Results.Ok(new
        {
            assetId = id,
            applied = action,
            states = after is null ? [] : AssetLifecycle.States(after, DateTime.UtcNow),
            availableActions = after is null ? [] : AssetLifecycle.AvailableActions(after, DateTime.UtcNow),
        });
    }

    private static void ReasonParams(DynamicParameters p, int id, string? reason, int uid)
    {
        p.Add("@pIntAssetID", id);
        p.Add("@pVarReason", reason ?? "");
        p.Add("@pIntUpdatedBy", uid);
    }

    private static void MusterParams(DynamicParameters p, int id, string? reason, int? reasonId, DateTime? expiry, int uid, string action)
    {
        p.Add("@pIntAssetID", id);
        p.Add("@pVarMusterReason", reason ?? "");
        p.Add("@pDtExpiryDate", expiry);
        p.Add("@pIntUpdatedBy", uid);
        p.Add("@pIntMusterReasonID", reasonId ?? 0);
        p.Add("@pVarAction", action);
    }

    private static void RfidParams(DynamicParameters p, int id, string? refNo, string card, string? reason, int uid, string action)
    {
        p.Add("@pIntAssetID", id);
        p.Add("@pVarRefNumber", refNo ?? "");
        p.Add("@pVarRFIDCardNumber", card);
        p.Add("@pVarReason", reason ?? "");
        p.Add("@pVarAction", action);
        p.Add("@pIntUpdatedBy", uid);
        p.Add("@pIntIsAssetNoExists", 0, direction: System.Data.ParameterDirection.InputOutput);
    }
}

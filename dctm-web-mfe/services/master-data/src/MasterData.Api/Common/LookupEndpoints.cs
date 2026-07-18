using MasterData.Api.Infrastructure;

namespace MasterData.Api.Common;

public static class LookupEndpoints
{
    public static IServiceCollection AddLookup<TRow, TWrite>(
        this IServiceCollection services, LookupSpMap<TWrite> map)
    {
        services.AddSingleton(map);
        services.AddSingleton<ILookupStore<TRow, TWrite>, DapperLookupStore<TRow, TWrite>>();
        return services;
    }

    /// <summary>
    /// Maps the standard lookup CRUD surface (legacy "Pattern A" page):
    /// GET /, GET /{id}, POST /, PUT /{id}, DELETE /?ids=1,2, GET /export.
    /// When <paramref name="module"/> is given, each verb enforces the matching legacy
    /// right (View/Create/Modify/Delete) — gated by the Authorization:Enforce flag.
    /// </summary>
    public static RouteGroupBuilder MapLookup<TRow, TWrite>(
        this IEndpointRouteBuilder routes, string pattern, string entityName, string? module = null)
    {
        var group = routes.MapGroup(pattern).WithTags(entityName);

        group.MapGet("/", async (ILookupStore<TRow, TWrite> store) =>
            Results.Ok(await store.ListAsync()))
            .Guard(module, RequirePermissionExtensions.View);

        group.MapGet("/{id:int}", async (int id, ILookupStore<TRow, TWrite> store) =>
        {
            var rows = await store.ListAsync(id);
            return rows.Count == 0 ? Results.NotFound() : Results.Ok(rows[0]);
        }).Guard(module, RequirePermissionExtensions.View);

        group.MapPost("/", async (TWrite dto, HttpContext context,
            ILookupStore<TRow, TWrite> store, LookupSpMap<TWrite> map) =>
        {
            var error = map.RequiresName ? ValidateName(map.NameOf(dto)) : null;
            if (error is not null) return Results.BadRequest(new { error });

            var check = await store.CheckNameAsync(0, dto);
            if (check == -1)
                return Results.Conflict(new { error = $"{entityName} '{map.NameOf(dto)}' already exists." });

            // check > 0: a soft-deleted record has this name — legacy pages reactivate it
            // by upserting with the returned id (see Manufacturer.aspx.cs).
            var id = await store.UpsertAsync(check > 0 ? check : 0, dto, await Audit.UserAsync(context));
            return Results.Created($"{context.Request.Path}/{id}", new { id, reactivated = check > 0 });
        }).Guard(module, RequirePermissionExtensions.Create);

        group.MapPut("/{id:int}", async (int id, TWrite dto, HttpContext context,
            ILookupStore<TRow, TWrite> store, LookupSpMap<TWrite> map) =>
        {
            var error = map.RequiresName ? ValidateName(map.NameOf(dto)) : null;
            if (error is not null) return Results.BadRequest(new { error });

            var rows = await store.ListAsync(id);
            if (rows.Count == 0) return Results.NotFound();

            if (await store.CheckNameAsync(id, dto) == -1)
                return Results.Conflict(new { error = $"{entityName} '{map.NameOf(dto)}' already exists." });

            await store.UpsertAsync(id, dto, await Audit.UserAsync(context));
            return Results.NoContent();
        }).Guard(module, RequirePermissionExtensions.Modify);

        group.MapDelete("/", async (string? ids, HttpContext context,
            ILookupStore<TRow, TWrite> store) =>
        {
            var parsed = ParseIds(ids);
            if (parsed is null)
                return Results.BadRequest(new { error = "Query parameter 'ids' must be a comma-separated list of integers." });

            await store.DeleteAsync(parsed, await Audit.UserAsync(context));
            return Results.NoContent();
        }).Guard(module, RequirePermissionExtensions.Delete);

        group.MapGet("/export", async (ILookupStore<TRow, TWrite> store) =>
            ExcelExport.Sheet(await store.ListAsync(), entityName))
            .Guard(module, RequirePermissionExtensions.View);

        return group;
    }

    // Applies the permission filter only when a module is configured for the entity.
    private static RouteHandlerBuilder Guard(this RouteHandlerBuilder builder, string? module, string right) =>
        module is null ? builder : builder.RequirePermission(module, right);

    private static string? ValidateName(string? name) =>
        string.IsNullOrWhiteSpace(name) ? "Name is required."
        : name.Length > 200 ? "Name must be 200 characters or fewer."
        : null;

    private static IReadOnlyList<int>? ParseIds(string? csv)
    {
        if (string.IsNullOrWhiteSpace(csv)) return null;
        var result = new List<int>();
        foreach (var part in csv.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        {
            if (!int.TryParse(part, out var id) || id <= 0) return null;
            result.Add(id);
        }
        return result.Count == 0 ? null : result;
    }
}

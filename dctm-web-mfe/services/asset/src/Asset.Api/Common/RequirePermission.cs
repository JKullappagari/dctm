using Asset.Api.Infrastructure;

namespace Asset.Api.Common;

/// <summary>
/// Enforces a legacy (module, right) for the authenticated user. No-ops when
/// Authorization:Enforce is false (dev). All asset endpoints use module "Search Asset".
/// </summary>
public sealed class RequirePermissionFilter(string right, string module = RequirePermissionFilter.SearchAsset) : IEndpointFilter
{
    public const string SearchAsset = "Search Asset";

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var http = context.HttpContext;
        var config = http.RequestServices.GetRequiredService<IConfiguration>();
        if (!config.GetValue("Authorization:Enforce", false))
            return await next(context);

        var login = http.User.Identity?.IsAuthenticated == true ? http.User.Identity.Name : null;
        if (string.IsNullOrEmpty(login))
            return Results.Unauthorized();

        var permissions = http.RequestServices.GetRequiredService<PermissionRepository>();
        if (!await permissions.HasRightAsync(login, module, right))
            return Results.Problem(
                detail: $"Requires '{right}' right on '{module}'.",
                statusCode: StatusCodes.Status403Forbidden);

        return await next(context);
    }
}

public static class AssetRights
{
    public const string View = "View";

    // Lifecycle action → legacy tblRight.Rights name (all on the "Search Asset" module).
    public static readonly IReadOnlyDictionary<string, string> ForAction = new Dictionary<string, string>
    {
        ["writeoff"] = "WriteOff",
        ["reinstate"] = "Reinstate",
        ["restrict"] = "Restrict",
        ["de-restrict"] = "Revoke Perm Restrict",
        ["bar"] = "Bar",
        ["un-bar"] = "UnBar",
        ["decommission"] = "Decommission",
        ["recommission"] = "Decommission",
        ["assign-rfid"] = "Assign RFID",
        ["deassign-rfid"] = "De-assign RFID",
    };

    public static RouteHandlerBuilder RequirePermission(this RouteHandlerBuilder builder, string right, string? module = null) =>
        builder.AddEndpointFilter(new RequirePermissionFilter(right, module ?? RequirePermissionFilter.SearchAsset));
}

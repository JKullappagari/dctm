using MasterData.Api.Infrastructure;

namespace MasterData.Api.Common;

/// <summary>
/// Endpoint filter enforcing a legacy (module, right) pair for the authenticated
/// user. No-ops when Authorization:Enforce is false (dev / standalone MFE), so the
/// same endpoints stay usable unauthenticated until the shell is the sole entry point.
/// </summary>
public sealed class RequirePermissionFilter(string module, string right) : IEndpointFilter
{
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

public static class RequirePermissionExtensions
{
    /// <summary>Legacy right names (tblRight.Rights) used by the CRUD verbs.</summary>
    public const string View = "View";
    public const string Create = "Create";
    public const string Modify = "Modify";
    public const string Delete = "Delete";
    // ApplicationMap.aspx uses bespoke rights instead of the CRUD set.
    public const string Map = "Map";

    public static RouteHandlerBuilder RequirePermission(
        this RouteHandlerBuilder builder, string module, string right) =>
        builder.AddEndpointFilter(new RequirePermissionFilter(module, right));
}

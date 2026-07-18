using MasterData.Api.Infrastructure;

namespace MasterData.Api.Common;

public static class Audit
{
    /// <summary>
    /// Acting legacy user id for CreatedBy/LastModifiedBy audit columns.
    /// Authenticated requests resolve the Keycloak preferred_username to
    /// tblUser.UserID; unauthenticated (standalone MFE dev, tests) fall back
    /// to the X-User-Id header. Enforcement flips on once the shell is the
    /// only entry point (Auth:Required in config).
    /// </summary>
    public static async Task<int> UserAsync(HttpContext context)
    {
        var login = context.User.Identity?.IsAuthenticated == true ? context.User.Identity.Name : null;
        if (!string.IsNullOrEmpty(login))
        {
            var resolver = context.RequestServices.GetRequiredService<LegacyUserResolver>();
            if (await resolver.ResolveAsync(login) is int userId) return userId;
        }

        // The X-User-Id dev fallback is disabled once auth is enforced — the JWT is
        // then the only accepted identity (guarded endpoints are already 401'd without it).
        var enforce = context.RequestServices.GetRequiredService<IConfiguration>()
            .GetValue("Authorization:Enforce", false);
        if (enforce) return 1; // unreachable while endpoints require auth; safe default
        return int.TryParse(context.Request.Headers["X-User-Id"], out var id) && id > 0 ? id : 1;
    }
}

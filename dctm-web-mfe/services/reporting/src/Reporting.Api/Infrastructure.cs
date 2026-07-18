using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Reporting.Api;

public interface IDbConnectionFactory { IDbConnection Create(); }

public sealed class SqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    public IDbConnection Create() => new SqlConnection(
        configuration.GetConnectionString("Reporting")
        ?? throw new InvalidOperationException("Connection string 'Reporting' is not configured."));
}

/// <summary>Maps the Keycloak preferred_username to the legacy tblUser.UserID the report SPs expect.</summary>
public sealed class LegacyUserResolver(IDbConnectionFactory factory)
{
    private readonly Dictionary<string, int> _cache = new(StringComparer.OrdinalIgnoreCase);

    public async Task<int> ResolveAsync(HttpContext ctx)
    {
        var login = ctx.User.Identity?.IsAuthenticated == true ? ctx.User.Identity.Name : null;
        if (string.IsNullOrEmpty(login)) return 1;
        if (_cache.TryGetValue(login, out var cached)) return cached;
        using var c = factory.Create();
        var id = await c.QuerySingleOrDefaultAsync<int?>(
            "SELECT UserID FROM tblUser WHERE LoginName = @login AND Status = 1", new { login }) ?? 1;
        _cache[login] = id;
        return id;
    }
}

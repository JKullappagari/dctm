using System.Collections.Concurrent;
using Dapper;

namespace MasterData.Api.Infrastructure;

/// <summary>
/// Maps a Keycloak preferred_username to the legacy tblUser.UserID that the
/// stored procedures expect for CreatedBy/LastModifiedBy audit columns.
/// </summary>
public sealed class LegacyUserResolver(IDbConnectionFactory connectionFactory)
{
    private readonly ConcurrentDictionary<string, int> _cache = new(StringComparer.OrdinalIgnoreCase);

    public async Task<int?> ResolveAsync(string loginName)
    {
        if (_cache.TryGetValue(loginName, out var cached)) return cached;

        using var connection = connectionFactory.Create();
        var userId = await connection.QuerySingleOrDefaultAsync<int?>(
            "SELECT UserID FROM tblUser WHERE LoginName = @loginName AND Status = 1",
            new { loginName });
        if (userId is int found) _cache[loginName] = found;
        return userId;
    }
}

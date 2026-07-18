using System.Collections.Concurrent;
using Dapper;

namespace MasterData.Api.Infrastructure;

/// <summary>
/// Checks whether a legacy user holds a given right on a given module, reading the
/// same tables the Identity &amp; Access service exposes as /me/permissions
/// (tblUser → tblGroupMember → tblGroupModuleRight → tblModuleRight → tblModule/tblRight).
/// Duplicated here (rather than an HTTP hop to identity) while both services share
/// one DB; centralize behind the identity service when the DBs split.
/// </summary>
public sealed class PermissionRepository(IDbConnectionFactory connectionFactory)
{
    // (login|module|right) → granted, cached for 60s so edits in the rights admin
    // page take effect without an API restart.
    private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);
    private readonly ConcurrentDictionary<string, (bool Granted, DateTime At)> _cache = new(StringComparer.OrdinalIgnoreCase);

    public async Task<bool> HasRightAsync(string login, string module, string right)
    {
        var key = $"{login}|{module}|{right}";
        if (_cache.TryGetValue(key, out var cached) && DateTime.UtcNow - cached.At < Ttl)
            return cached.Granted;

        using var connection = connectionFactory.Create();
        var count = await connection.ExecuteScalarAsync<int>("""
            SELECT COUNT(1)
            FROM tblUser U
            JOIN tblGroupMember GM ON GM.UserID = U.UserID AND GM.Status = 1
            JOIN tblGroupModuleRight GMR ON GMR.GroupID = GM.GroupID AND GMR.Status = 1
            JOIN tblModuleRight MR ON MR.RightModuleID = GMR.RightModuleID
            JOIN tblModule M ON M.ModuleID = MR.ModuleID AND M.Status = 1
            JOIN tblRight R ON R.RightsID = MR.RightID
            WHERE U.LoginName = @login AND U.Status = 1
              AND M.Module = @module AND R.Rights = @right
            """, new { login, module, right });

        var granted = count > 0;
        _cache[key] = (granted, DateTime.UtcNow);
        return granted;
    }
}

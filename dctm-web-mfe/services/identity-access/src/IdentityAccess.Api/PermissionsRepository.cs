using Dapper;
using Microsoft.Data.SqlClient;

namespace IdentityAccess.Api;

public sealed class MenuModule
{
    public int MainModuleID { get; set; }
    public string MainModule { get; set; } = "";
    public int MainSort { get; set; }
    public int ModuleID { get; set; }
    public string Module { get; set; } = "";
    public string? PageURL { get; set; }
    public int? SortOrder { get; set; }
}

public sealed class ModulePermission
{
    public string Module { get; set; } = "";
    public string Rights { get; set; } = "";
}

/// <summary>
/// Reads the legacy security model (tblUser → tblGroupMember → tblGroupModuleRight →
/// tblModuleRight → tblModule/tblRight) — the data that drove the Web Forms
/// security-trimmed SqlSiteMapProvider menu. Read-only; no legacy SPs exist for
/// these joins, so the queries live here.
/// </summary>
public sealed class PermissionsRepository(IConfiguration configuration)
{
    private SqlConnection Open() => new(
        configuration.GetConnectionString("IdentityAccess")
        ?? throw new InvalidOperationException("Connection string 'IdentityAccess' is not configured."));

    private const string UserJoin = """
        FROM tblUser U
        JOIN tblGroupMember GM ON GM.UserID = U.UserID AND GM.Status = 1
        JOIN tblGroupModuleRight GMR ON GMR.GroupID = GM.GroupID AND GMR.Status = 1
        JOIN tblModuleRight MR ON MR.RightModuleID = GMR.RightModuleID
        JOIN tblModule M ON M.ModuleID = MR.ModuleID AND M.Status = 1
        """;

    public async Task<IReadOnlyList<MenuModule>> GetMenuAsync(string loginName)
    {
        using var connection = Open();
        var rows = await connection.QueryAsync<MenuModule>($"""
            SELECT MM.MainModuleID, MM.MainModule, MM.SortOrder AS MainSort,
                   M.ModuleID, M.Module, M.PageURL, M.SortOrder
            {UserJoin}
            JOIN tblMainModule MM ON MM.MainModuleID = M.MainModuleID AND MM.Status = 1
            WHERE U.LoginName = @loginName AND U.Status = 1
            GROUP BY MM.MainModuleID, MM.MainModule, MM.SortOrder,
                     M.ModuleID, M.Module, M.PageURL, M.SortOrder
            ORDER BY MM.SortOrder, M.SortOrder
            """, new { loginName });
        return rows.AsList();
    }

    public async Task<IReadOnlyList<ModulePermission>> GetPermissionsAsync(string loginName)
    {
        using var connection = Open();
        var rows = await connection.QueryAsync<ModulePermission>($"""
            SELECT DISTINCT M.Module, R.Rights
            {UserJoin}
            JOIN tblRight R ON R.RightsID = MR.RightID
            WHERE U.LoginName = @loginName AND U.Status = 1
            """, new { loginName });
        return rows.AsList();
    }
}

using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace IdentityAccess.Api;

public sealed class ModuleRightRow
{
    public int MainModuleID { get; set; }
    public string MainModule { get; set; } = "";
    public int MainSort { get; set; }
    public int ModuleID { get; set; }
    public string Module { get; set; } = "";
    public int? ModuleSort { get; set; }
    public int RightsID { get; set; }
    public string Rights { get; set; } = "";
    public bool Granted { get; set; }
}

public sealed class UserGroupRow
{
    public int GroupID { get; set; }
    public string Group { get; set; } = "";
    public bool Member { get; set; }
}

/// <summary>
/// Admin editors over the legacy security model: the group ↔ module-rights matrix
/// (GroupModuleRightsAssignment.aspx) and user ↔ group membership (the group part
/// of ManageUsers.aspx). Reads are direct queries on the permission tables; writes
/// go through the legacy SPs.
/// </summary>
public sealed class AdminRepository(IConfiguration configuration)
{
    private SqlConnection Open() => new(
        configuration.GetConnectionString("IdentityAccess")
        ?? throw new InvalidOperationException("Connection string 'IdentityAccess' is not configured."));

    public async Task<int?> ResolveUserIdAsync(string loginName)
    {
        using var connection = Open();
        return await connection.QuerySingleOrDefaultAsync<int?>(
            "SELECT UserID FROM tblUser WHERE LoginName = @loginName AND Status = 1", new { loginName });
    }

    /// <summary>Every module right, flagged with whether the group currently holds it.</summary>
    public async Task<IReadOnlyList<ModuleRightRow>> GetModuleRightsAsync(int groupId)
    {
        using var connection = Open();
        var rows = await connection.QueryAsync<ModuleRightRow>("""
            SELECT MM.MainModuleID, MM.MainModule, MM.SortOrder AS MainSort,
                   M.ModuleID, M.Module, M.SortOrder AS ModuleSort,
                   R.RightsID, R.Rights,
                   CAST(CASE WHEN GMR.GroupModuleRightID IS NOT NULL THEN 1 ELSE 0 END AS bit) AS Granted
            FROM tblModule M
            JOIN tblMainModule MM ON MM.MainModuleID = M.MainModuleID AND MM.Status = 1
            JOIN tblModuleRight MR ON MR.ModuleID = M.ModuleID
            JOIN tblRight R ON R.RightsID = MR.RightID
            LEFT JOIN tblGroupModuleRight GMR ON GMR.RightModuleID = MR.RightModuleID
                 AND GMR.GroupID = @groupId AND GMR.Status = 1
            WHERE M.Status = 1
            ORDER BY MM.SortOrder, M.SortOrder, R.RightsID
            """, new { groupId });
        return rows.AsList();
    }

    /// <summary>Replaces a group's rights on one module (legacy delimited-RightsIDs SP).</summary>
    public async Task SaveModuleRightsAsync(int groupId, int moduleId, IEnumerable<int> rightIds, int userId)
    {
        using var connection = Open();
        var parameters = new DynamicParameters();
        parameters.Add("@pIntGroupID", groupId);
        parameters.Add("@pIntModuleID", moduleId);
        parameters.Add("@pVarDelimiter", ",");
        parameters.Add("@pVarAccessRightsIDs", string.Join(",", rightIds.Distinct()));
        parameters.Add("@pBitStatus", true);
        parameters.Add("@pIntCreatedBy", userId);
        await connection.ExecuteAsync(
            "iAssetTrack_Sp_GroupModuleRightsAssignment_Update",
            parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<IReadOnlyList<UserGroupRow>> GetUserGroupsAsync(int userId)
    {
        using var connection = Open();
        var rows = await connection.QueryAsync<UserGroupRow>("""
            SELECT G.GroupID, G.[Group],
                   CAST(CASE WHEN GM.GroupMemberID IS NOT NULL THEN 1 ELSE 0 END AS bit) AS Member
            FROM tblGroup G
            LEFT JOIN tblGroupMember GM ON GM.GroupID = G.GroupID
                 AND GM.UserID = @userId AND GM.Status = 1
            WHERE G.Status = 1
            ORDER BY G.[Group]
            """, new { userId });
        return rows.AsList();
    }

    /// <summary>Replaces a user's group memberships (legacy iAssetTrack_Sp_User_AssignRights).</summary>
    public async Task SaveUserGroupsAsync(int userId, IEnumerable<int> groupIds, int actingUserId)
    {
        using var connection = Open();
        var parameters = new DynamicParameters();
        parameters.Add("@pIntUserID", userId);
        parameters.Add("@pVarDelimiter", ",");
        parameters.Add("@pVarGroupIds", string.Join(",", groupIds.Distinct()));
        parameters.Add("@pIntCreatedBy", actingUserId);
        await connection.ExecuteAsync(
            "iAssetTrack_Sp_User_AssignRights", parameters, commandType: CommandType.StoredProcedure);
    }
}

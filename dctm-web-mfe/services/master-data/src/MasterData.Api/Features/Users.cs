using System.Data;
using Dapper;
using MasterData.Api.Common;
using MasterData.Api.Infrastructure;

namespace MasterData.Api.Features;

// Columns from iAssetTrack_Sp_User_List (legacy UserSearch.aspx grid).
public sealed class UserRow
{
    public int UserID { get; set; }
    public string LoginName { get; set; } = "";
    public string? Name { get; set; }
    public int SiteRestriction { get; set; }
    // The SP returns display text ("Active"), not a flag.
    public string? Status { get; set; }
}

/// <summary>Read-only user list (UserSearch.aspx). No write path — users are managed in Keycloak.</summary>
public static class Users
{
    private const string Module = "User Search";

    public static void MapUsers(this IEndpointRouteBuilder api)
    {
        var group = api.MapGroup("/users").WithTags("Users");

        group.MapGet("/", async (IDbConnectionFactory factory) =>
        {
            using var connection = factory.Create();
            var rows = await connection.QueryAsync<UserRow>(
                "iAssetTrack_Sp_User_List", new { pIntUserID = 0 }, commandType: CommandType.StoredProcedure);
            return Results.Ok(rows);
        }).RequirePermission(Module, RequirePermissionExtensions.View);

        group.MapGet("/export", async (IDbConnectionFactory factory) =>
        {
            using var connection = factory.Create();
            var rows = await connection.QueryAsync<UserRow>(
                "iAssetTrack_Sp_User_List", new { pIntUserID = 0 }, commandType: CommandType.StoredProcedure);
            return ExcelExport.Sheet(rows, "Users");
        }).RequirePermission(Module, RequirePermissionExtensions.View);
    }
}

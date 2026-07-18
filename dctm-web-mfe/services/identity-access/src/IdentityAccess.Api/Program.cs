using System.Security.Claims;
using IdentityAccess.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddSingleton<PermissionsRepository>();
builder.Services.AddSingleton<AdminRepository>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [])
          .AllowAnyHeader()
          .AllowAnyMethod()));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // MetadataAddress is the container-internal Keycloak URL; browsers get tokens
        // whose issuer is the host-mapped URL, so both issuers are accepted.
        options.MetadataAddress = builder.Configuration["Keycloak:MetadataAddress"]
            ?? "http://localhost:8180/realms/dctrack/.well-known/openid-configuration";
        options.RequireHttpsMetadata = false; // dev only — Keycloak runs plain HTTP locally
        options.TokenValidationParameters = new()
        {
            ValidIssuers = builder.Configuration.GetSection("Keycloak:ValidIssuers").Get<string[]>()
                ?? ["http://localhost:8180/realms/dctrack"],
            ValidateAudience = false, // Keycloak public clients default aud=account; roles come from the legacy DB
            NameClaimType = "preferred_username",
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/healthz");

var me = app.MapGroup("/api/v1/me").RequireAuthorization();

me.MapGet("/", (ClaimsPrincipal user) => Results.Ok(new
{
    login = user.Identity?.Name,
    email = user.FindFirstValue(ClaimTypes.Email) ?? user.FindFirstValue("email"),
}));

// Replaces the security-trimmed SqlSiteMapProvider menu: main modules with the
// modules this user's groups grant any right on.
me.MapGet("/menu", async (ClaimsPrincipal user, PermissionsRepository repository) =>
{
    var login = user.Identity?.Name;
    if (string.IsNullOrEmpty(login)) return Results.Unauthorized();

    var modules = await repository.GetMenuAsync(login);
    var menu = modules
        .GroupBy(m => new { m.MainModuleID, m.MainModule, m.MainSort })
        .OrderBy(g => g.Key.MainSort)
        .Select(g => new
        {
            title = g.Key.MainModule,
            items = g.Select(m => new { m.ModuleID, label = m.Module, legacyUrl = m.PageURL }),
        });
    return Results.Ok(menu);
});

// Flat (module, right) pairs — source for frontend guards and API authorization policies.
me.MapGet("/permissions", async (ClaimsPrincipal user, PermissionsRepository repository) =>
{
    var login = user.Identity?.Name;
    if (string.IsNullOrEmpty(login)) return Results.Unauthorized();
    return Results.Ok(await repository.GetPermissionsAsync(login));
});

// --- Admin editors (GroupModuleRightsAssignment.aspx + the group part of ManageUsers.aspx) ---
// Gate: any right on the legacy admin module. The Update SPs remain the writers.
const string AdminModule = "Group Module Access Rights Assignment";

async Task<IResult?> RequireAdminAsync(ClaimsPrincipal user, PermissionsRepository permissions)
{
    var login = user.Identity?.Name;
    if (string.IsNullOrEmpty(login)) return Results.Unauthorized();
    var held = await permissions.GetPermissionsAsync(login);
    return held.Any(p => p.Module == AdminModule)
        ? null
        : Results.Problem(detail: $"Requires a right on '{AdminModule}'.", statusCode: 403);
}

var admin = app.MapGroup("/api/v1").RequireAuthorization();

// Full rights matrix for a group: main module → module → rights with granted flags.
admin.MapGet("/groups/{groupId:int}/module-rights",
    async (int groupId, ClaimsPrincipal user, PermissionsRepository permissions, AdminRepository repository) =>
{
    if (await RequireAdminAsync(user, permissions) is { } denied) return denied;
    var rows = await repository.GetModuleRightsAsync(groupId);
    var tree = rows
        .GroupBy(r => new { r.MainModuleID, r.MainModule, r.MainSort })
        .OrderBy(g => g.Key.MainSort)
        .Select(g => new
        {
            title = g.Key.MainModule,
            modules = g.GroupBy(r => new { r.ModuleID, r.Module, r.ModuleSort })
                .OrderBy(m => m.Key.ModuleSort)
                .Select(m => new
                {
                    moduleId = m.Key.ModuleID,
                    module = m.Key.Module,
                    rights = m.Select(r => new { r.RightsID, r.Rights, r.Granted }),
                }),
        });
    return Results.Ok(tree);
});

// Replace a group's rights on one module (matches the legacy per-module SP contract).
admin.MapPut("/groups/{groupId:int}/modules/{moduleId:int}/rights",
    async (int groupId, int moduleId, SaveRights body, ClaimsPrincipal user,
        PermissionsRepository permissions, AdminRepository repository) =>
{
    if (await RequireAdminAsync(user, permissions) is { } denied) return denied;
    var actingUser = await repository.ResolveUserIdAsync(user.Identity!.Name!) ?? 1;
    await repository.SaveModuleRightsAsync(groupId, moduleId, body.RightIds, actingUser);
    return Results.NoContent();
});

admin.MapGet("/users/{userId:int}/groups",
    async (int userId, ClaimsPrincipal user, PermissionsRepository permissions, AdminRepository repository) =>
{
    if (await RequireAdminAsync(user, permissions) is { } denied) return denied;
    return Results.Ok(await repository.GetUserGroupsAsync(userId));
});

admin.MapPut("/users/{userId:int}/groups",
    async (int userId, SaveGroups body, ClaimsPrincipal user,
        PermissionsRepository permissions, AdminRepository repository) =>
{
    if (await RequireAdminAsync(user, permissions) is { } denied) return denied;
    var actingUser = await repository.ResolveUserIdAsync(user.Identity!.Name!) ?? 1;
    await repository.SaveUserGroupsAsync(userId, body.GroupIds, actingUser);
    return Results.NoContent();
});

app.Run();

public sealed record SaveRights(int[] RightIds);
public sealed record SaveGroups(int[] GroupIds);

public partial class Program;

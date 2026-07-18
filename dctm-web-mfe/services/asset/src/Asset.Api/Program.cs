using Asset.Api.Features;
using Asset.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddSingleton<LegacyUserResolver>();
builder.Services.AddSingleton<PermissionRepository>();
builder.Services.AddSingleton<AssetStore>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [])
          .AllowAnyHeader()
          .AllowAnyMethod()));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MetadataAddress = builder.Configuration["Keycloak:MetadataAddress"]
            ?? "http://localhost:8180/realms/dctrack/.well-known/openid-configuration";
        options.RequireHttpsMetadata = false; // dev only
        options.TokenValidationParameters = new()
        {
            ValidIssuers = builder.Configuration.GetSection("Keycloak:ValidIssuers").Get<string[]>()
                ?? ["http://localhost:8180/realms/dctrack"],
            ValidateAudience = false,
            NameClaimType = "preferred_username",
        };
    });
builder.Services.AddAuthorization(options =>
{
    // Baseline: when enforcing, every endpoint needs a valid JWT (health/OpenAPI opt out).
    if (builder.Configuration.GetValue("Authorization:Enforce", false))
        options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser().Build();
});

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapOpenApi().AllowAnonymous();
app.MapHealthChecks("/healthz").AllowAnonymous();
app.MapAssets();
app.MapAssetWrite();
app.MapAssetImport();
app.MapInventory();

app.Run();

public partial class Program;

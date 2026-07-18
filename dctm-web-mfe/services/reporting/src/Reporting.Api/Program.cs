using Microsoft.AspNetCore.Authentication.JwtBearer;
using Reporting.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddSingleton<LegacyUserResolver>();
builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [])
          .AllowAnyHeader().AllowAnyMethod()));

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
app.MapReports();

app.Run();

public partial class Program;

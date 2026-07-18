using MasterData.Api.Common;
using MasterData.Api.Features;
using MasterData.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<LegacyUserResolver>();
builder.Services.AddSingleton<PermissionRepository>();

// JWT is accepted (and used to resolve the legacy audit user) but not yet required —
// standalone MFE dev and tests still work unauthenticated until the shell is the
// sole entry point.
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
    // When enforcing, every endpoint requires a valid JWT unless it opts out with
    // AllowAnonymous (health, OpenAPI). Per-endpoint permission filters add the
    // specific right on top. Closes the gap for endpoints without a module guard.
    if (builder.Configuration.GetValue("Authorization:Enforce", false))
        options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser().Build();
});
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [])
          .AllowAnyHeader()
          .AllowAnyMethod()));
builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

// Raw Dapper rows (ref endpoints) serialize as dictionaries; camel-case their keys
// so reference lists match the camelCase of the typed entity endpoints.
builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);

builder.Services.AddLookup<ManufacturerRow, ManufacturerWrite>(Manufacturers.SpMap);
builder.Services.AddLookup<LocationTypeRow, LocationTypeWrite>(LocationTypes.SpMap);
builder.Services.AddLookup<PurposeRow, PurposeWrite>(Purposes.SpMap);
builder.Services.AddLookup<MusterReasonRow, MusterReasonWrite>(MusterReasons.SpMap);
builder.Services.AddLookup<BusinessUnitRow, BusinessUnitWrite>(BusinessUnits.SpMap);
builder.Services.AddLookup<DivisionRow, DivisionWrite>(Divisions.SpMap);
builder.Services.AddLookup<OwnerRow, OwnerWrite>(Owners.SpMap);
builder.Services.AddLookup<TechnologyCategoryRow, TechnologyCategoryWrite>(TechnologyCategories.SpMap);
builder.Services.AddLookup<AssetGroupRow, AssetGroupWrite>(AssetGroups.SpMap);
builder.Services.AddLookup<SiteRow, SiteWrite>(Sites.SpMap);
builder.Services.AddLookup<LocationRow, LocationWrite>(Locations.SpMap);
builder.Services.AddLookup<AssetModelRow, AssetModelWrite>(AssetModels.SpMap);
builder.Services.AddLookup<TenantRow, TenantWrite>(Tenants.SpMap);
builder.Services.AddLookup<ApplicationTypeRow, ApplicationTypeWrite>(ApplicationTypes.SpMap);
builder.Services.AddLookup<ApplicationCriticalityRow, ApplicationCriticalityWrite>(ApplicationCriticalities.SpMap);
builder.Services.AddLookup<GroupRow, GroupWrite>(Groups.SpMap);
builder.Services.AddLookup<MobileDeviceRow, MobileDeviceWrite>(MobileDevices.SpMap);
builder.Services.AddLookup<AuditCycleRow, AuditCycleWrite>(AuditCycles.SpMap);
builder.Services.AddLookup<ApplicationRow, ApplicationWrite>(Applications.SpMap);

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapOpenApi().AllowAnonymous();
app.MapHealthChecks("/healthz").AllowAnonymous();

var api = app.MapGroup("/api/v1");
// The 3rd arg is the legacy tblModule.Module name that gates each verb's right
// (View/Create/Modify/Delete). Entities with no tblModule row (Business Unit,
// Location Type, Asset Group) are left unguarded.
api.MapLookup<ManufacturerRow, ManufacturerWrite>("/manufacturers", "Manufacturers", "Manufacturer");
api.MapLookup<LocationTypeRow, LocationTypeWrite>("/location-types", "LocationTypes");
api.MapLookup<PurposeRow, PurposeWrite>("/purposes", "Purposes", "Purpose");
api.MapLookup<MusterReasonRow, MusterReasonWrite>("/muster-reasons", "MusterReasons", "Reason");
api.MapLookup<BusinessUnitRow, BusinessUnitWrite>("/business-units", "BusinessUnits");
api.MapLookup<DivisionRow, DivisionWrite>("/divisions", "Divisions", "Division");
api.MapLookup<OwnerRow, OwnerWrite>("/owners", "Owners", "Owner");
api.MapLookup<TechnologyCategoryRow, TechnologyCategoryWrite>("/technology-categories", "TechnologyCategories", "Technology Category");
api.MapLookup<AssetGroupRow, AssetGroupWrite>("/asset-groups", "AssetGroups");
api.MapLookup<SiteRow, SiteWrite>("/sites", "Sites", "Site");
api.MapSiteGeo();
api.MapLookup<LocationRow, LocationWrite>("/locations", "Locations", "Location");
api.MapLookup<AssetModelRow, AssetModelWrite>("/asset-models", "AssetModels", "Asset Model");
api.MapLookup<TenantRow, TenantWrite>("/tenants", "Tenants", "Tenant");
api.MapLookup<ApplicationTypeRow, ApplicationTypeWrite>("/application-types", "ApplicationTypes", "Application Type");
api.MapLookup<ApplicationCriticalityRow, ApplicationCriticalityWrite>("/application-criticalities", "ApplicationCriticalities", "Application Criticality");
api.MapLookup<GroupRow, GroupWrite>("/groups", "Groups", "Group");
api.MapLookup<MobileDeviceRow, MobileDeviceWrite>("/devices", "Devices", "Register Device");
api.MapLookup<AuditCycleRow, AuditCycleWrite>("/audit-cycles", "AuditCycles", "Audit Cycle");
api.MapLookup<ApplicationRow, ApplicationWrite>("/applications", "Applications", "Create Application");
api.MapApplicationRefs();
api.MapHosts();
api.MapUsers();
api.MapReferenceData();
api.MapImportAssetModels();
api.MapImportApplications();
api.MapImportAppAssetMap();
app.MapAssignments();

app.Run();

public partial class Program;

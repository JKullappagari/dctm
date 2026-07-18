using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns from iAssetTrack_Sp_Tenant_List (legacy Tenant.aspx grid).
public sealed class TenantRow
{
    public int TenantId { get; set; }
    public string TenantFullName { get; set; } = "";
    public string? TenantShortName { get; set; }
    public string? TenantType { get; set; }
    public int? TenantTypeSize { get; set; }
    public int? UserCount { get; set; }
    public string? ContactFirstName { get; set; }
    public string? ContactLastName { get; set; }
    public string? ContactEmail { get; set; }
}

public sealed record TenantWrite(
    string TenantFullName, string? TenantShortName, string? TenantType, int? TenantTypeSize,
    string? ContactFName, string? ContactLName, string? ContactEmail, int? UserCount);

public static class Tenants
{
    // Core Tenant CRUD. The Update SP also carries AssignedLocs (location tree),
    // AdminPermissions and UserPermissions — the full tenant sub-system — which pass
    // blank here and are deferred to a dedicated tenant feature.
    public static readonly LookupSpMap<TenantWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_Tenant_List",
        UpsertSp: "iAssetTrack_Sp_Tenant_Update",
        DeleteSp: "iAssetTrack_Sp_Tenant_Delete",
        ExistsSp: "iAssetTrack_Sp_Tenant_DoesExist",
        IdParam: "@pIntTenantId",
        NameParam: "@pVarTenantFullName",
        IdsParam: "@pVarTenantIDs",
        NameOf: w => w.TenantFullName,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarTenantFullName", w.TenantFullName);
            p.Add("@pVarTenantShortName", w.TenantShortName ?? "");
            p.Add("@pVarTenantType", w.TenantType ?? "");
            p.Add("@pIntTenantTypeSize", (short)(w.TenantTypeSize ?? 0));
            p.Add("@pVarContactFName", w.ContactFName ?? "");
            p.Add("@pVarContactLName", w.ContactLName ?? "");
            p.Add("@pVarContactEmail", w.ContactEmail ?? "");
            p.Add("@pIntUserCount", (short)(w.UserCount ?? 0));
            p.Add("@pVarAssignedLocs", "");
            p.Add("@pVarAdminPermissions", "");
            p.Add("@pVarUserPermissions", "");
        },
        // Tenant uniqueness is full name + short name.
        AddExistsParams: (p, w) =>
        {
            p.Add("@pVarTenantFullName", w.TenantFullName);
            p.Add("@pVarTenantShortName", w.TenantShortName ?? "");
        },
        // The legacy iAssetTrack_Sp_Tenant_Delete is broken (filters WHERE TechID —
        // a column that doesn't exist on tblTenant), so it can't be reused. This
        // parameterized soft-delete reproduces the SP's intent correctly.
        DeleteSqlOverride:
            "UPDATE tblTenant SET Status = 0, LastModifiedDate = getdate(), LastModifiedBy = @userId " +
            "WHERE TenantId IN @ids");
}

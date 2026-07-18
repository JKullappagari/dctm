using Dapper;

namespace MasterData.Api.Common;

/// <summary>
/// Parameter names shared by every legacy iAssetTrack stored procedure.
/// </summary>
public static class LegacyParams
{
    public const string Status = "@pBitStatus";
    public const string CreatedBy = "@pIntCreatedBy";
    public const string LastModifiedBy = "@pIntLastModifiedBy";
}

/// <summary>
/// Maps one legacy lookup entity to its four stored procedures
/// (List / Update-upsert / Delete-by-CSV / DoesExist), preserving the
/// exact contracts of the Web Forms BAL+DALC layers.
/// </summary>
public sealed record LookupSpMap<TWrite>(
    string ListSp,
    string UpsertSp,
    string DeleteSp,
    string ExistsSp,
    string IdParam,
    string NameParam,
    string IdsParam,
    Func<TWrite, string> NameOf,
    Action<DynamicParameters, TWrite> AddUpsertParams,
    /// Optional override for DoesExist SPs that check more than one column
    /// (e.g. Owner: first name + last name + email). Defaults to NameParam = NameOf(dto).
    Action<DynamicParameters, TWrite>? AddExistsParams = null,
    /// Optional parameterized soft-delete SQL used instead of the Delete SP — for when
    /// the legacy Delete SP is defective. Receives @ids (int list) and @userId.
    /// (e.g. Tenant: the legacy SP filters on a non-existent TechID column.)
    string? DeleteSqlOverride = null,
    /// False for entities with no editable text name (e.g. Audit Cycle: a location +
    /// date range). Skips the name-required validation; NameOf is used only for messages.
    bool RequiresName = true,
    /// False when the Update SP has no @pBitStatus param (e.g. AuditCycle_Update).
    bool IncludeStatusOnUpsert = true,
    /// True when the Delete SP takes only the ids param, no Status/ModifiedBy
    /// (e.g. AuditCycle_Delete — a hard delete).
    bool DeleteTakesIdsOnly = false);

using System.Data;
using Asset.Api.Common;
using Asset.Api.Infrastructure;
using Dapper;

namespace Asset.Api.Features;

public sealed record AssetWriteRequest(
    string RefNumber,
    string? AssetName,
    int ModelId,
    int SiteId,
    int LocationId,
    int? BusinessUnitId,
    int? OwnerId,
    int? TechId,
    string? RackOrStand,
    int? StartPos,
    int? NoOfRUs,
    int? ParentAssetId,
    string? Host,
    string? Orientation,
    string? ExternalId = null);

/// <summary>
/// Create/edit asset (CreateAsset.aspx) via legacy iAssetTrack_Sp_Asset_UpdateNew.
/// The SP returns Result (0 = success) + MessageCode, resolved to text via tblMessage —
/// same feedback the legacy page showed. Only the core fields are exposed; the SP's
/// hardware-detail params (OS/CPU/…) pass blanks until an advanced section is needed.
/// </summary>
public static class AssetWrite
{
    public static void MapAssetWrite(this IEndpointRouteBuilder api)
    {
        var assets = api.MapGroup("/api/v1/assets").WithTags("Assets");

        assets.MapPost("/", (AssetWriteRequest body, HttpContext ctx, IDbConnectionFactory f) =>
            UpsertAsync(0, body, ctx, f)).RequirePermission("Create", "Create Asset");

        assets.MapPut("/{id:int}", (int id, AssetWriteRequest body, HttpContext ctx, IDbConnectionFactory f) =>
            UpsertAsync(id, body, ctx, f)).RequirePermission("Modify", "Create Asset");
    }

    public sealed record UpsertOutcome(bool Ok, int AssetId, string? Message);

    /// <summary>
    /// Shared insert/update over iAssetTrack_Sp_Asset_UpdateNew — used by the create/edit
    /// endpoints and by the Import Asset / Import Blades wizards. Derives the asset group
    /// from the model and the BU from the site (legacy page behavior) and resolves the
    /// SP's Result/MessageCode to text.
    /// </summary>
    public static async Task<UpsertOutcome> UpsertCoreAsync(
        System.Data.IDbConnection connection, int id, AssetWriteRequest body, int userId, bool isImport = false)
    {
        var assetGroupId = await connection.ExecuteScalarAsync<int?>(
            "SELECT AssetTypeID FROM tblAssetModel WHERE ModelID = @id", new { id = body.ModelId }) ?? 0;
        var buId = body.BusinessUnitId
            ?? await connection.ExecuteScalarAsync<int?>(
                "SELECT TOP 1 BusinessUnitID FROM tblBUSiteAssignment WHERE SiteID = @id AND Status = 1",
                new { id = body.SiteId })
            ?? 1;

        var p = new DynamicParameters();
        p.Add("@pIntAssetID", id, DbType.Int32, ParameterDirection.InputOutput);
        p.Add("@pVarRefNumber", body.RefNumber);
        p.Add("@pIntModelID", body.ModelId);
        p.Add("@pIntAssetGroupID", assetGroupId);
        p.Add("@pIntDefaultLocationID", body.LocationId);
        p.Add("@pIntBusinessUnitID", buId);
        p.Add("@pIntPrimarySiteID", body.SiteId);
        p.Add("@pVarAssetName", body.AssetName ?? "");
        p.Add("@pDtAssetCreatedDate", DateTime.UtcNow);
        p.Add("@pIntAssetCreatedBy", userId);
        p.Add("@pIntLastSeenLocationID", body.LocationId);
        p.Add("@pIntCurrentOwnerID", body.OwnerId ?? 0);
        p.Add("@pIntUpdatedBy", userId);
        p.Add("@pIntResult", 0, DbType.Int32, ParameterDirection.Output);
        p.Add("@pVarMessageCode", "", DbType.String, ParameterDirection.Output, size: 100);
        p.Add("@pVarOS", "");
        p.Add("@pVarCPU", "");
        p.Add("@pIntCPUCount", "");
        p.Add("@pVarCPUCore", "");
        p.Add("@pIntTechID", body.TechId ?? 0);
        p.Add("@pVarRackOrStand", body.RackOrStand ?? "");
        p.Add("@pIntStartPos", body.StartPos ?? 0);
        p.Add("@pIntNoOfRUs", body.NoOfRUs ?? 0);
        p.Add("@pBitIsImport", isImport);
        p.Add("@pBitSerialModelCheck", false);
        p.Add("@pIntParentAssetID", body.ParentAssetId ?? 0);
        p.Add("@PIsParent", false);
        p.Add("@PCurrentRFIDCardNumber", "");
        p.Add("@pVarHost", body.Host ?? "");
        p.Add("@POrientation", body.Orientation ?? "");
        p.Add("@pVarApplications", "");
        p.Add("@pVarInternalID", "");
        p.Add("@pVarExternalID", body.ExternalId ?? "");
        p.Add("@pFltDeratedPower", 0.0);

        await connection.ExecuteAsync(
            "iAssetTrack_Sp_Asset_UpdateNew", p, commandType: CommandType.StoredProcedure);

        var result = p.Get<int>("@pIntResult");
        var messageCode = p.Get<string?>("@pVarMessageCode") ?? "";
        var message = string.IsNullOrWhiteSpace(messageCode)
            ? null
            : await connection.ExecuteScalarAsync<string?>(
                "SELECT Message FROM tblMessage WHERE MessageCode = @messageCode", new { messageCode })
              ?? messageCode;

        return new UpsertOutcome(result == 0, p.Get<int>("@pIntAssetID"), message);
    }

    private static async Task<IResult> UpsertAsync(
        int id, AssetWriteRequest body, HttpContext ctx, IDbConnectionFactory factory)
    {
        if (string.IsNullOrWhiteSpace(body.RefNumber))
            return Results.BadRequest(new { error = "Ref number is required." });

        using var connection = factory.Create();
        var outcome = await UpsertCoreAsync(connection, id, body, await Audit.UserAsync(ctx));

        if (!outcome.Ok)
            return Results.Conflict(new { error = outcome.Message ?? "Asset save failed." });

        return id == 0
            ? Results.Created($"/api/v1/assets/{outcome.AssetId}", new { id = outcome.AssetId, message = outcome.Message })
            : Results.Ok(new { id = outcome.AssetId, message = outcome.Message });
    }
}

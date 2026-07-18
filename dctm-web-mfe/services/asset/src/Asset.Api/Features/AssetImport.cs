using System.Data;
using Asset.Api.Common;
using Asset.Api.Infrastructure;
using ClosedXML.Excel;
using Dapper;

namespace Asset.Api.Features;

public sealed record ImportRowResult(int Row, string Ref, string Status, string? Message);
public sealed record ImportResult(int Total, int Imported, int Failed, IReadOnlyList<ImportRowResult> Rows);

/// <summary>
/// Import Asset (ImportAsset.aspx) and Import Blades (ImportBlades.aspx): upload an
/// .xlsx, resolve the name columns to ids, and insert each asset through the shared
/// Asset_UpdateNew path. Imports into EXISTING locations — the legacy Racks-sheet
/// location-creation step is out of scope. Rows are independent; every outcome returns.
/// </summary>
public static class AssetImport
{
    public static void MapAssetImport(this IEndpointRouteBuilder app)
    {
        var import = app.MapGroup("/api/v1/import").WithTags("Import");

        import.MapGet("/assets/template", () => Template(
            "ModelName is Manufacturer + Model matched by name.",
            "Site", "Room", "Row", "Rack", "StartPosition",
            "AssetName", "AssetTag", "ExternalID", "HostName",
            "Manufacturer", "Model", "Orientation", "Owner"))
            .RequirePermission("View", "Import Asset");

        import.MapGet("/blades/template", () => Template(
            "Position is the slot in the parent enclosure.",
            "ParentAssetTag", "Position", "AssetName", "AssetTag", "ExternalID",
            "HostName", "Manufacturer", "Model", "Orientation", "Owner"))
            .RequirePermission("View", "Import Blades");

        import.MapPost("/assets", (IFormFile file, HttpContext ctx, IDbConnectionFactory f) =>
            RunAsync(file, ctx, f, blades: false))
            .RequirePermission("Create", "Import Asset").DisableAntiforgery();

        import.MapPost("/blades", (IFormFile file, HttpContext ctx, IDbConnectionFactory f) =>
            RunAsync(file, ctx, f, blades: true))
            .RequirePermission("Create", "Import Blades").DisableAntiforgery();
    }

    private static async Task<IResult> RunAsync(
        IFormFile file, HttpContext ctx, IDbConnectionFactory factory, bool blades)
    {
        if (file is null || file.Length == 0)
            return Results.BadRequest(new { error = "No file uploaded." });

        using var connection = factory.Create();
        var mfg = await NameMap(connection, "SELECT MfgID, MfgName FROM tblManufacturer WHERE Status = 1");
        var owners = await NameMap(connection,
            "SELECT OwnerID, OwnerFirstName + ' ' + ISNULL(OwnerLastName,'') FROM tblOwner WHERE Status = 1");
        var userId = await Audit.UserAsync(ctx);

        await using var stream = file.OpenReadStream();
        using var workbook = new XLWorkbook(stream);
        var sheet = workbook.Worksheet(1);
        var header = HeaderIndex(sheet);
        string? Col(IXLRow r, string n) => header.TryGetValue(n, out var c) ? r.Cell(c).GetString().Trim() : null;

        var results = new List<ImportRowResult>();
        var imported = 0;

        foreach (var row in sheet.RowsUsed().Skip(1))
        {
            var rowNum = row.RowNumber();
            var assetTag = Col(row, blades ? "AssetTag" : "AssetTag") ?? "";
            try
            {
                if (string.IsNullOrWhiteSpace(assetTag))
                {
                    results.Add(new(rowNum, "", "error", "AssetTag is required."));
                    continue;
                }

                var mfgName = Col(row, "Manufacturer") ?? "";
                var modelName = Col(row, "Model") ?? "";
                var modelId = await ResolveModel(connection, mfg, mfgName, modelName);
                if (modelId is null)
                {
                    results.Add(new(rowNum, assetTag, "error", $"Unknown model '{mfgName} / {modelName}'."));
                    continue;
                }

                int siteId, locationId, parentAssetId = 0;

                if (blades)
                {
                    // Blade goes into a parent enclosure — inherit its site/location.
                    var parentTag = Col(row, "ParentAssetTag") ?? "";
                    var parent = await connection.QuerySingleOrDefaultAsync<(int AssetID, int SiteID, int? LocId)?>(
                        "SELECT AssetID, PrimarySiteID, DefaultLocationID FROM tblAsset WHERE RefNumber = @tag AND IsApproved = 1",
                        new { tag = parentTag });
                    if (parent is null)
                    {
                        results.Add(new(rowNum, assetTag, "error", $"Unknown parent asset '{parentTag}'."));
                        continue;
                    }
                    parentAssetId = parent.Value.AssetID;
                    siteId = parent.Value.SiteID;
                    locationId = parent.Value.LocId ?? 0;
                }
                else
                {
                    var siteName = Col(row, "Site") ?? "";
                    var rack = Col(row, "Rack") ?? "";
                    var resolvedSite = await connection.ExecuteScalarAsync<int?>(
                        "SELECT SiteID FROM tblSite WHERE Site = @siteName AND Status = 1", new { siteName });
                    if (resolvedSite is null)
                    {
                        results.Add(new(rowNum, assetTag, "error", $"Unknown site '{siteName}'."));
                        continue;
                    }
                    siteId = resolvedSite.Value;

                    var loc = await connection.ExecuteScalarAsync<int?>("""
                        SELECT TOP 1 L.LocationID FROM tblLocation L
                        JOIN tblSiteLocationAssignment SLA ON SLA.LocationID = L.LocationID
                        WHERE L.Location = @rack AND SLA.SiteID = @siteId AND L.Status = 1
                        """, new { rack, siteId });
                    if (loc is null)
                    {
                        results.Add(new(rowNum, assetTag, "error", $"Rack '{rack}' not found in site '{siteName}'."));
                        continue;
                    }
                    locationId = loc.Value;
                }

                var body = new AssetWriteRequest(
                    RefNumber: assetTag,
                    AssetName: Col(row, "AssetName"),
                    ModelId: modelId.Value,
                    SiteId: siteId,
                    LocationId: locationId,
                    BusinessUnitId: null,
                    OwnerId: Lookup(owners, Col(row, "Owner")),
                    TechId: null,
                    RackOrStand: blades ? null : Col(row, "Rack"),
                    StartPos: ToInt(Col(row, blades ? "Position" : "StartPosition")),
                    NoOfRUs: null,
                    ParentAssetId: blades ? parentAssetId : null,
                    Host: Col(row, "HostName"),
                    Orientation: Col(row, "Orientation"),
                    ExternalId: Col(row, "ExternalID"));

                var outcome = await AssetWrite.UpsertCoreAsync(connection, 0, body, userId, isImport: true);
                if (outcome.Ok)
                {
                    imported++;
                    results.Add(new(rowNum, assetTag, "imported", outcome.Message));
                }
                else
                {
                    var reason = outcome.Message ?? (blades
                        ? "Rejected — check the model is a blade and the parent is an enclosure with a free slot."
                        : "Rejected by the system (duplicate tag or invalid data).");
                    results.Add(new(rowNum, assetTag, "error", reason));
                }
            }
            catch (Exception ex)
            {
                results.Add(new(rowNum, assetTag, "error", ex.Message));
            }
        }

        return Results.Ok(new ImportResult(
            results.Count, imported, results.Count(r => r.Status == "error"), results));
    }

    private static async Task<int?> ResolveModel(
        IDbConnection c, Dictionary<string, int> mfg, string mfgName, string modelName)
    {
        if (!mfg.TryGetValue(mfgName, out var mfgId)) return null;
        return await c.ExecuteScalarAsync<int?>(
            "SELECT ModelID FROM tblAssetModel WHERE ModelName = @modelName AND MfgID = @mfgId AND Status = 1",
            new { modelName, mfgId });
    }

    private static IResult Template(string note, params string[] headers)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Import");
        for (var i = 0; i < headers.Length; i++) sheet.Cell(1, i + 1).Value = headers[i];
        sheet.Row(1).Style.Font.Bold = true;
        sheet.Cell(3, 1).Value = note;
        sheet.Columns().AdjustToContents();
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return Results.File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Import-Template.xlsx");
    }

    private static async Task<Dictionary<string, int>> NameMap(IDbConnection c, string sql)
    {
        var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var (id, name) in await c.QueryAsync<(int, string)>(sql))
            if (!string.IsNullOrWhiteSpace(name)) map[name.Trim()] = id;
        return map;
    }

    private static Dictionary<string, int> HeaderIndex(IXLWorksheet sheet)
    {
        var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var cell in sheet.Row(1).CellsUsed())
            map[cell.GetString().Trim()] = cell.Address.ColumnNumber;
        return map;
    }

    private static int? Lookup(Dictionary<string, int> map, string? name) =>
        !string.IsNullOrWhiteSpace(name) && map.TryGetValue(name, out var id) ? id : null;

    private static int? ToInt(string? s) => int.TryParse(s, out var v) ? v : null;
}

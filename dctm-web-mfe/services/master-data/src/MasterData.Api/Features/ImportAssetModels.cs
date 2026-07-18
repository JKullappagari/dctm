using ClosedXML.Excel;
using Dapper;
using MasterData.Api.Common;
using MasterData.Api.Infrastructure;

namespace MasterData.Api.Features;

/// <summary>
/// Import Asset Models (ImportAssetModels.aspx): upload an .xlsx, resolve the FK name
/// columns (Manufacturer / ModelType / MountType / AirFlowDirection) to ids, and insert
/// each row through the same AssetModel contract the UI uses. Rows are independent —
/// one failure doesn't abort the rest; every row's outcome is returned.
/// </summary>
public static class ImportAssetModels
{
    public static void MapImportAssetModels(this IEndpointRouteBuilder api)
    {
        api.MapGet("/import/asset-models/template", () =>
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("AssetModels");
            var headers = new[]
            {
                "ModelName", "Manufacturer", "ModelType", "MountType", "AirFlowDirection",
                "UHeight", "Max_Power_Watts", "IsBlade", "IsEnclosure", "Description",
            };
            for (var i = 0; i < headers.Length; i++) sheet.Cell(1, i + 1).Value = headers[i];
            sheet.Row(1).Style.Font.Bold = true;
            sheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return Results.File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "AssetModels-Template.xlsx");
        }).RequirePermission("Import Asset Models", RequirePermissionExtensions.View);

        api.MapPost("/import/asset-models", async (
            IFormFile file, HttpContext ctx,
            ILookupStore<AssetModelRow, AssetModelWrite> store, IDbConnectionFactory factory) =>
        {
            if (file is null || file.Length == 0)
                return Results.BadRequest(new { error = "No file uploaded." });

            using var connection = factory.Create();
            var mfg = await NameMap(connection, "SELECT MfgID, MfgName FROM tblManufacturer WHERE Status = 1");
            var group = await NameMap(connection, "SELECT AssetGroupID, AssetGroup FROM tblAssetGroup WHERE Status = 1");
            var mount = await NameMap(connection, "SELECT MountTypeID, MountType FROM tblMountType");
            var airflow = await NameMap(connection, "SELECT ID, AirFlowDirection FROM tblAirFlowDirection");
            var userId = await Audit.UserAsync(ctx);
            // Asset models are BU-scoped and the Update SP rejects BU 0; default to the
            // acting user's BU (falling back to the first active BU).
            var defaultBu = await connection.ExecuteScalarAsync<int?>(
                "SELECT COALESCE(NULLIF(DefaultBU, 0), (SELECT TOP 1 BusinessUnitID FROM tblBusinessUnit WHERE Status = 1 ORDER BY BusinessUnitID)) " +
                "FROM tblUser WHERE UserID = @userId", new { userId })
                ?? await connection.ExecuteScalarAsync<int?>(
                    "SELECT TOP 1 BusinessUnitID FROM tblBusinessUnit WHERE Status = 1 ORDER BY BusinessUnitID")
                ?? 1;

            await using var upload = file.OpenReadStream();
            using var workbook = new XLWorkbook(upload);
            var sheet = workbook.Worksheet(1);
            var header = HeaderIndex(sheet);

            string? Col(IXLRow row, string name) =>
                header.TryGetValue(name, out var c) ? row.Cell(c).GetString().Trim() : null;

            var results = new List<ImportRowResult>();
            var imported = 0;
            foreach (var row in sheet.RowsUsed().Skip(1))
            {
                var rowNum = row.RowNumber();
                var modelName = Col(row, "ModelName") ?? "";
                var mfgName = Col(row, "Manufacturer") ?? "";

                if (string.IsNullOrWhiteSpace(modelName) || string.IsNullOrWhiteSpace(mfgName))
                {
                    results.Add(new(rowNum, modelName, "error", "ModelName and Manufacturer are required."));
                    continue;
                }
                if (!mfg.TryGetValue(mfgName, out var mfgId))
                {
                    results.Add(new(rowNum, modelName, "error", $"Unknown manufacturer '{mfgName}'."));
                    continue;
                }

                var dto = new AssetModelWrite(
                    ModelName: modelName,
                    Description: Col(row, "Description"),
                    MfgId: mfgId,
                    ModelTypeId: Lookup(group, Col(row, "ModelType")),
                    TechId: null,
                    BuId: defaultBu,
                    MountTypeId: Lookup(mount, Col(row, "MountType")),
                    AfDirectionId: Lookup(airflow, Col(row, "AirFlowDirection")),
                    UHeight: ToInt(Col(row, "UHeight")),
                    MaxPower: ToDouble(Col(row, "Max_Power_Watts")),
                    IsBlade: ToBool(Col(row, "IsBlade")),
                    IsEnclosure: ToBool(Col(row, "IsEnclosure")),
                    Comment: null);

                try
                {
                    var check = await store.CheckNameAsync(0, dto);
                    if (check == -1)
                    {
                        results.Add(new(rowNum, modelName, "skipped", "A model with this name already exists."));
                        continue;
                    }
                    await store.UpsertAsync(check > 0 ? check : 0, dto, userId);
                    imported++;
                    results.Add(new(rowNum, modelName, "imported", null));
                }
                catch (Exception ex)
                {
                    results.Add(new(rowNum, modelName, "error", ex.Message));
                }
            }

            return Results.Ok(new ImportResult(
                results.Count, imported, results.Count(r => r.Status == "error"), results));
        }).RequirePermission("Import Asset Models", RequirePermissionExtensions.Create).DisableAntiforgery();
    }

    private static async Task<Dictionary<string, int>> NameMap(System.Data.IDbConnection c, string sql)
    {
        var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var (id, name) in await c.QueryAsync<(int, string)>(sql))
            map[name.Trim()] = id;
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
    private static double? ToDouble(string? s) => double.TryParse(s, out var v) ? v : null;
    private static bool ToBool(string? s) =>
        s is not null && (s.Equals("true", StringComparison.OrdinalIgnoreCase) || s == "1" || s.Equals("yes", StringComparison.OrdinalIgnoreCase));
}

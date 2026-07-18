using System.Data;
using ClosedXML.Excel;
using Dapper;

namespace MasterData.Api.Features;

public sealed record ImportRowResult(int Row, string Model, string Status, string? Message);
public sealed record ImportResult(int Total, int Imported, int Failed, IReadOnlyList<ImportRowResult> Rows);

/// <summary>One materialized spreadsheet data row: its 1-based number and header→value cells.</summary>
public sealed record ImportDataRow(int Number, IReadOnlyDictionary<string, string> Values)
{
    /// <summary>The trimmed cell for a header, or null if the column is absent.</summary>
    public string? Col(string name) => Values.TryGetValue(name, out var v) ? v : null;
}

/// <summary>
/// Shared helpers for the .xlsx import wizards (Applications, Asset-App Map, …):
/// building the download template, resolving FK name→id maps, and iterating data rows
/// with a header-keyed column accessor.
/// </summary>
public static class ImportSupport
{
    public static IResult Template(string sheetName, IReadOnlyList<string> headers)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add(sheetName);
        for (var i = 0; i < headers.Count; i++) sheet.Cell(1, i + 1).Value = headers[i];
        sheet.Row(1).Style.Font.Bold = true;
        sheet.Columns().AdjustToContents();
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return Results.File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"{sheetName}-Template.xlsx");
    }

    public static async Task<Dictionary<string, int>> NameMap(IDbConnection c, string sql)
    {
        var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var (id, name) in await c.QueryAsync<(int, string)>(sql))
            if (!string.IsNullOrWhiteSpace(name)) map[name.Trim()] = id;
        return map;
    }

    /// <summary>
    /// Reads each used data row (past the header) into memory with a header-keyed cell
    /// accessor. Values are materialized up front so the workbook is fully closed before
    /// the rows are consumed — the Col accessor is safe to call at any time.
    /// </summary>
    public static IReadOnlyList<ImportDataRow> DataRows(IFormFile file)
    {
        using var upload = file.OpenReadStream();
        using var workbook = new XLWorkbook(upload);
        var sheet = workbook.Worksheet(1);

        var header = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var cell in sheet.Row(1).CellsUsed())
            header[cell.GetString().Trim()] = cell.Address.ColumnNumber;

        var result = new List<ImportDataRow>();
        foreach (var row in sheet.RowsUsed().Skip(1))
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var (name, col) in header)
                values[name] = row.Cell(col).GetString().Trim();
            result.Add(new ImportDataRow(row.RowNumber(), values));
        }
        return result;
    }

    public static int? Lookup(Dictionary<string, int> map, string? name) =>
        !string.IsNullOrWhiteSpace(name) && map.TryGetValue(name.Trim(), out var id) ? id : null;

    public static int? ToInt(string? s) => int.TryParse(s, out var v) ? v : null;
    public static double? ToDouble(string? s) => double.TryParse(s, out var v) ? v : null;
    public static bool ToBool(string? s) =>
        s is not null && (s.Equals("true", StringComparison.OrdinalIgnoreCase) || s == "1" || s.Equals("yes", StringComparison.OrdinalIgnoreCase));
}

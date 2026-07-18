using ClosedXML.Excel;

namespace MasterData.Api.Common;

/// <summary>
/// Server-side xlsx generation — the replacement for Infragistics WebExcelExporter,
/// shared by the generic lookup endpoints and any bespoke feature (e.g. Hosts).
/// </summary>
public static class ExcelExport
{
    public static IResult Sheet<TRow>(IEnumerable<TRow> rows, string entityName)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add(entityName);
        sheet.Cell(1, 1).InsertTable(rows, entityName, createTable: true);
        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return Results.File(
            stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"{entityName}.xlsx");
    }
}

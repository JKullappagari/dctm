using ClosedXML.Excel;
using MasterData.Api.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Api.Tests;

public sealed class ImportSupportTests
{
    [Fact]
    public void Template_produces_an_xlsx_with_the_given_headers()
    {
        var result = ImportSupport.Template("Applications", ["Application", "BusinessUnit"]);
        // Results.File is an internal type; render it to bytes via a fake HttpContext.
        var bytes = RenderFile(result);
        Assert.Equal(0x50, bytes[0]); // 'P'
        Assert.Equal(0x4B, bytes[1]); // 'K' — xlsx is a zip

        using var wb = new XLWorkbook(new MemoryStream(bytes));
        var sheet = wb.Worksheet(1);
        Assert.Equal("Application", sheet.Cell(1, 1).GetString());
        Assert.Equal("BusinessUnit", sheet.Cell(1, 2).GetString());
    }

    [Fact]
    public void DataRows_yields_each_row_with_header_keyed_access()
    {
        var file = BuildSheet(
            ["Application", "HostName"],
            [["MyApp", "srv-1"], ["Other", "srv-2"]]);

        var rows = ImportSupport.DataRows(file);
        Assert.Equal(2, rows.Count);
        Assert.Equal("MyApp", rows[0].Col("Application"));
        Assert.Equal("srv-1", rows[0].Col("HostName"));
        Assert.Equal(2, rows[0].Number);     // header is row 1
        Assert.Null(rows[0].Col("Missing")); // unknown column → null
        Assert.Equal("Other", rows[1].Col("Application"));
    }

    [Fact]
    public void DataRows_column_lookup_is_case_insensitive_and_trims()
    {
        var file = BuildSheet(["Application"], [["  Spaced  "]]);
        var row = Assert.Single(ImportSupport.DataRows(file));
        Assert.Equal("Spaced", row.Col("application")); // trimmed + case-insensitive header
    }

    [Theory]
    [InlineData("5", 5)]
    [InlineData("", null)]
    [InlineData("x", null)]
    public void ToInt_parses_or_returns_null(string input, int? expected) =>
        Assert.Equal(expected, ImportSupport.ToInt(input));

    [Theory]
    [InlineData("true", true)]
    [InlineData("1", true)]
    [InlineData("yes", true)]
    [InlineData("no", false)]
    [InlineData("", false)]
    public void ToBool_reads_common_truthy_spellings(string input, bool expected) =>
        Assert.Equal(expected, ImportSupport.ToBool(input));

    [Fact]
    public void Lookup_matches_by_trimmed_case_insensitive_name()
    {
        var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { ["Finance"] = 7 };
        Assert.Equal(7, ImportSupport.Lookup(map, " finance "));
        Assert.Null(ImportSupport.Lookup(map, "HR"));
        Assert.Null(ImportSupport.Lookup(map, null));
    }

    // --- helpers ---

    private static IFormFile BuildSheet(string[] headers, string[][] dataRows)
    {
        using var wb = new XLWorkbook();
        var sheet = wb.Worksheets.Add("Sheet1");
        for (var c = 0; c < headers.Length; c++) sheet.Cell(1, c + 1).Value = headers[c];
        for (var r = 0; r < dataRows.Length; r++)
            for (var c = 0; c < dataRows[r].Length; c++)
                sheet.Cell(r + 2, c + 1).Value = dataRows[r][c];

        var stream = new MemoryStream();
        wb.SaveAs(stream);
        stream.Position = 0;
        return new FormFile(stream, 0, stream.Length, "file", "test.xlsx");
    }

    private static byte[] RenderFile(IResult result)
    {
        var context = new DefaultHttpContext
        {
            // FileResult execution resolves ILoggerFactory from the request services.
            RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider(),
        };
        var body = new MemoryStream();
        context.Response.Body = body;
        result.ExecuteAsync(context).GetAwaiter().GetResult();
        return body.ToArray();
    }
}

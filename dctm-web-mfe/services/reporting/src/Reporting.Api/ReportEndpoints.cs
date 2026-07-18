using System.Data;
using ClosedXML.Excel;
using Dapper;

namespace Reporting.Api;

public static class ReportEndpoints
{
    public static void MapReports(this IEndpointRouteBuilder app)
    {
        var reports = app.MapGroup("/api/v1/reports").WithTags("Reports");

        // Catalog for the UI: which reports exist and each one's filter schema.
        reports.MapGet("/", () => Results.Ok(ReportRegistry.All.Select(r => new
        {
            r.Key, r.Title, r.Module,
            @params = r.Params.Select(p => new
            {
                p.Key, type = p.Type.ToString().ToLowerInvariant(), p.Label,
                p.OptionsPath, p.ValueField, p.LabelField,
            }),
        }))).RequireAuthorization();

        reports.MapGet("/{key}", async (string key, HttpContext ctx,
            IDbConnectionFactory factory, LegacyUserResolver users) =>
        {
            var def = ReportRegistry.Find(key);
            if (def is null) return Results.NotFound();
            if (await Deny(ctx, def) is { } denied) return denied;

            var rows = await RunAsync(def, ctx, factory, await users.ResolveAsync(ctx));
            return Results.Ok(rows);
        });

        reports.MapGet("/{key}/export", async (string key, HttpContext ctx,
            IDbConnectionFactory factory, LegacyUserResolver users) =>
        {
            var def = ReportRegistry.Find(key);
            if (def is null) return Results.NotFound();
            if (await Deny(ctx, def) is { } denied) return denied;

            var rows = await RunAsync(def, ctx, factory, await users.ResolveAsync(ctx));
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(def.Title.Length > 31 ? def.Title[..31] : def.Title);
            if (rows.Count > 0)
            {
                var cols = ((IDictionary<string, object>)rows[0]).Keys.ToList();
                for (var i = 0; i < cols.Count; i++) sheet.Cell(1, i + 1).Value = cols[i];
                sheet.Row(1).Style.Font.Bold = true;
                for (var r = 0; r < rows.Count; r++)
                {
                    var dict = (IDictionary<string, object>)rows[r];
                    for (var c = 0; c < cols.Count; c++)
                        sheet.Cell(r + 2, c + 1).Value = XLCellValue.FromObject(dict[cols[c]]?.ToString());
                }
                sheet.Columns().AdjustToContents();
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return Results.File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{def.Key}.xlsx");
        });
    }

    private static async Task<List<dynamic>> RunAsync(
        ReportDef def, HttpContext ctx, IDbConnectionFactory factory, int userId)
    {
        var p = new DynamicParameters();
        foreach (var param in def.Params)
        {
            var raw = ctx.Request.Query[param.Key].ToString();
            // A blank filter with a sentinel ("All") is sent as that sentinel, so the
            // legacy dynamic-SQL SPs take their no-filter path instead of building `IN ()`.
            var value = string.IsNullOrWhiteSpace(raw) && param.EmptyValue is not null
                ? param.EmptyValue
                : Convert(param.Type, raw);
            p.Add(param.SpParam, value);
        }
        if (def.NeedsUser) p.Add("@pIntLoggedInUserId", userId);

        using var connection = factory.Create();
        var rows = await connection.QueryAsync(def.Sp, p, commandType: CommandType.StoredProcedure);
        return rows.ToList();
    }

    private static object? Convert(ParamType type, string raw) => type switch
    {
        ParamType.Int => int.TryParse(raw, out var i) ? i : 0,           // 0 = all
        ParamType.Bool => raw is "true" or "1" or "yes",
        ParamType.Date or ParamType.Text or ParamType.LocList => raw ?? "",
        _ => raw,
    };

    // Report permission: any right on the report's legacy module (matches the
    // security-trimmed menu). No-op when enforcement is off.
    private static async Task<IResult?> Deny(HttpContext ctx, ReportDef def)
    {
        var config = ctx.RequestServices.GetRequiredService<IConfiguration>();
        if (!config.GetValue("Authorization:Enforce", false)) return null;

        var login = ctx.User.Identity?.IsAuthenticated == true ? ctx.User.Identity.Name : null;
        if (string.IsNullOrEmpty(login)) return Results.Unauthorized();

        using var connection = ctx.RequestServices.GetRequiredService<IDbConnectionFactory>().Create();
        var count = await connection.ExecuteScalarAsync<int>("""
            SELECT COUNT(1)
            FROM tblUser U
            JOIN tblGroupMember GM ON GM.UserID = U.UserID AND GM.Status = 1
            JOIN tblGroupModuleRight GMR ON GMR.GroupID = GM.GroupID AND GMR.Status = 1
            JOIN tblModuleRight MR ON MR.RightModuleID = GMR.RightModuleID
            JOIN tblModule M ON M.ModuleID = MR.ModuleID AND M.Status = 1
            WHERE U.LoginName = @login AND U.Status = 1 AND M.Module = @module
            """, new { login, module = def.Module });
        return count > 0 ? null
            : Results.Problem(detail: $"Requires access to '{def.Module}'.", statusCode: 403);
    }
}

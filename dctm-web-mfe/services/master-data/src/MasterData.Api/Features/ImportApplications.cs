using ClosedXML.Excel;
using Dapper;
using MasterData.Api.Common;
using MasterData.Api.Infrastructure;

namespace MasterData.Api.Features;

/// <summary>
/// Import Applications (ImportApplications.aspx): upload an .xlsx, resolve the FK name
/// columns (Business Unit / Application Type / Criticality / Owner / Status) to ids, and
/// insert each row through the same Application contract the CreateApplication page uses.
/// Rows are independent — one failure doesn't abort the rest; every outcome is returned.
/// </summary>
public static class ImportApplications
{
    private static readonly string[] Headers =
        ["Application", "Description", "BusinessUnit", "ApplicationType", "Criticality", "Owner", "Status"];

    public static void MapImportApplications(this IEndpointRouteBuilder api)
    {
        api.MapGet("/import/applications/template", () =>
            ImportSupport.Template("Applications", Headers))
            .RequirePermission("Import Applications", RequirePermissionExtensions.View);

        api.MapPost("/import/applications", async (
            IFormFile file, HttpContext ctx,
            ILookupStore<ApplicationRow, ApplicationWrite> store, IDbConnectionFactory factory) =>
        {
            if (file is null || file.Length == 0)
                return Results.BadRequest(new { error = "No file uploaded." });

            using var connection = factory.Create();
            var bu = await ImportSupport.NameMap(connection, "SELECT BusinessUnitID, BusinessUnit FROM tblBusinessUnit WHERE Status = 1");
            var type = await ImportSupport.NameMap(connection, "SELECT ApplTypeID, ApplType FROM tblApplType WHERE Status = 1");
            var crit = await ImportSupport.NameMap(connection, "SELECT ApplCriticalityID, ApplCriticality FROM tblApplCriticality WHERE Status = 1");
            var status = await ImportSupport.NameMap(connection, "SELECT AppStatusID, AppStatus FROM tblAppStatus");
            var owner = await OwnerMap(connection);
            var userId = await Audit.UserAsync(ctx);

            var results = new List<ImportRowResult>();
            var imported = 0;
            foreach (var dataRow in ImportSupport.DataRows(file))
            {
                var num = dataRow.Number;
                Func<string, string?> Col = dataRow.Col;
                var name = Col("Application");
                if (string.IsNullOrWhiteSpace(name))
                {
                    results.Add(new(num, name ?? "", "error", "Application is required."));
                    continue;
                }

                var dto = new ApplicationWrite(
                    ApplName: name,
                    Description: Col("Description"),
                    BuId: ImportSupport.Lookup(bu, Col("BusinessUnit")),
                    ApplTypeId: ImportSupport.Lookup(type, Col("ApplicationType")),
                    CriticalityId: ImportSupport.Lookup(crit, Col("Criticality")),
                    OwnerId: ImportSupport.Lookup(owner, Col("Owner")),
                    AppStatusId: ImportSupport.Lookup(status, Col("Status")));

                try
                {
                    var check = await store.CheckNameAsync(0, dto);
                    if (check == -1)
                    {
                        results.Add(new(num, name, "skipped", "An application with these details already exists."));
                        continue;
                    }
                    await store.UpsertAsync(check > 0 ? check : 0, dto, userId);
                    imported++;
                    results.Add(new(num, name, "imported", null));
                }
                catch (Exception ex)
                {
                    results.Add(new(num, name, "error", ex.Message));
                }
            }

            return Results.Ok(new ImportResult(
                results.Count, imported, results.Count(r => r.Status == "error"), results));
        }).RequirePermission("Import Applications", RequirePermissionExtensions.Create).DisableAntiforgery();
    }

    // Owners are matched by "First Last", first name alone, or email — whichever the sheet uses.
    private static async Task<Dictionary<string, int>> OwnerMap(System.Data.IDbConnection c)
    {
        var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var rows = await c.QueryAsync<(int OwnerID, string? First, string? Last, string? Email)>(
            "SELECT OwnerID, OwnerFirstName, OwnerLastName, Email FROM tblOwner WHERE Status = 1");
        foreach (var (id, first, last, email) in rows)
        {
            void Put(string? key) { if (!string.IsNullOrWhiteSpace(key)) map[key.Trim()] = id; }
            Put($"{first} {last}".Trim());
            Put(first);
            Put(email);
        }
        return map;
    }
}

using System.Net;
using System.Net.Http.Json;
using MasterData.Api.Common;
using MasterData.Api.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MasterData.Api.Tests;

public sealed class ManufacturerEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ManufacturerEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<ILookupStore<ManufacturerRow, ManufacturerWrite>>();
                services.AddSingleton<ILookupStore<ManufacturerRow, ManufacturerWrite>, FakeManufacturerStore>();
            }));
    }

    private HttpClient Client() => _factory.CreateClient();

    [Fact]
    public async Task List_returns_all_rows()
    {
        var rows = await Client().GetFromJsonAsync<List<ManufacturerRow>>("/api/v1/manufacturers");
        Assert.NotNull(rows);
        Assert.Equal(2, rows.Count);
        Assert.Contains(rows, r => r.MfgName == "Dell");
    }

    [Fact]
    public async Task Get_by_id_returns_row_or_404()
    {
        var row = await Client().GetFromJsonAsync<ManufacturerRow>("/api/v1/manufacturers/1");
        Assert.Equal("Dell", row!.MfgName);

        var missing = await Client().GetAsync("/api/v1/manufacturers/999");
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }

    [Fact]
    public async Task Create_returns_201_with_new_id()
    {
        var response = await Client().PostAsJsonAsync(
            "/api/v1/manufacturers", new ManufacturerWrite("HPE", "Storage"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ReactivationResponse>();
        Assert.True(body!.Id > 0);
        Assert.False(body.Reactivated);
        Assert.EndsWith($"/api/v1/manufacturers/{body.Id}", response.Headers.Location!.ToString());
    }

    [Fact]
    public async Task Create_duplicate_name_returns_409()
    {
        var response = await Client().PostAsJsonAsync(
            "/api/v1/manufacturers", new ManufacturerWrite("Dell", null));
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Create_with_soft_deleted_name_reactivates_original_id()
    {
        // Legacy DoesExist SP returns the soft-deleted row's id; the page re-upserts
        // with that id instead of inserting a new row (Manufacturer.aspx.cs behavior).
        var response = await Client().PostAsJsonAsync(
            "/api/v1/manufacturers", new ManufacturerWrite("Nortel", "back from the dead"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ReactivationResponse>();
        Assert.Equal(3, body!.Id);          // original soft-deleted id, not a new one
        Assert.True(body.Reactivated);
    }

    private sealed record ReactivationResponse(int Id, bool Reactivated);

    [Fact]
    public async Task Create_blank_name_returns_400()
    {
        var response = await Client().PostAsJsonAsync(
            "/api/v1/manufacturers", new ManufacturerWrite("  ", null));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_existing_returns_204_and_unknown_returns_404()
    {
        var client = Client();

        var ok = await client.PutAsJsonAsync(
            "/api/v1/manufacturers/2", new ManufacturerWrite("Cisco Systems", "Network"));
        Assert.Equal(HttpStatusCode.NoContent, ok.StatusCode);

        var missing = await client.PutAsJsonAsync(
            "/api/v1/manufacturers/999", new ManufacturerWrite("Ghost", null));
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }

    [Fact]
    public async Task Delete_with_csv_ids_returns_204()
    {
        var response = await Client().DeleteAsync("/api/v1/manufacturers?ids=1,2");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_with_invalid_ids_returns_400()
    {
        var response = await Client().DeleteAsync("/api/v1/manufacturers?ids=1,abc");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Export_returns_xlsx_file()
    {
        var response = await Client().GetAsync("/api/v1/manufacturers/export");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            response.Content.Headers.ContentType!.MediaType);
        var bytes = await response.Content.ReadAsByteArrayAsync();
        Assert.True(bytes.Length > 0);
        Assert.Equal(0x50, bytes[0]); // 'P' — xlsx files are zip archives ("PK")
        Assert.Equal(0x4B, bytes[1]); // 'K'
    }

    [Fact]
    public async Task Healthz_returns_healthy()
    {
        var response = await Client().GetAsync("/healthz");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

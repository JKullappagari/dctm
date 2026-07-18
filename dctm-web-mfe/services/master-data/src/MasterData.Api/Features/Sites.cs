using Dapper;
using MasterData.Api.Common;
using MasterData.Api.Infrastructure;

namespace MasterData.Api.Features;

// Columns as returned by iAssetTrack_Sp_Site_List (joins tblCountry/tblCity for display names).
public sealed class SiteRow
{
    public int SiteID { get; set; }
    public string Site { get; set; } = "";
    public string? Description { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
}

public sealed record SiteWrite(string Site, string? Description, int? CountryId, int? CityId);

/// <summary>Country/City ids for a site — the list SP only returns display names,
/// but the edit form needs ids to preselect the cascading dropdowns.</summary>
public sealed record SiteGeo(int? CountryID, int? CityID);

public sealed class RegionRow { public string Region { get; set; } = ""; }
public sealed class CountryRow { public int CountryID { get; set; } public string CountryName { get; set; } = ""; }
public sealed class CityRow { public int CityID { get; set; } public string CityName { get; set; } = ""; }

public static class Sites
{
    public static readonly LookupSpMap<SiteWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_Site_List",
        UpsertSp: "iAssetTrack_Sp_Site_Update",
        DeleteSp: "iAssetTrack_Sp_Site_Delete",
        ExistsSp: "iAssetTrack_Sp_Site_DoesExist",
        IdParam: "@pIntSiteID",
        NameParam: "@pVarSite",
        IdsParam: "@pVarSiteIDs",
        NameOf: w => w.Site,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarSite", w.Site);
            p.Add("@pVarDescription", w.Description ?? "");
            p.Add("@pIntCountryID", w.CountryId);
            p.Add("@pIntCityID", w.CityId);
        });

    /// <summary>
    /// Geo lookups for the cascading Region → Country → City selects.
    /// Countries/cities reuse legacy SPs; there is no SP for regions, so that one
    /// is a read-only query on tblCountry (documented deviation).
    /// </summary>
    public static void MapSiteGeo(this IEndpointRouteBuilder api)
    {
        api.MapGet("/sites/{id:int}/geo", async (int id, IDbConnectionFactory factory) =>
        {
            using var connection = factory.Create();
            var geo = await connection.QuerySingleOrDefaultAsync<SiteGeo>(
                "SELECT COUNTRYID AS CountryID, CITYID AS CityID FROM tblSite WHERE SiteID = @id",
                new { id });
            return geo is null ? Results.NotFound() : Results.Ok(geo);
        }).WithTags("Sites");

        var geo = api.MapGroup("/geo").WithTags("Geo");

        geo.MapGet("/regions", async (IDbConnectionFactory factory) =>
        {
            using var connection = factory.Create();
            var rows = await connection.QueryAsync<RegionRow>(
                "SELECT DISTINCT Region FROM tblCountry WHERE Region IS NOT NULL ORDER BY Region");
            return Results.Ok(rows);
        });

        geo.MapGet("/countries", async (string region, IDbConnectionFactory factory) =>
        {
            using var connection = factory.Create();
            var rows = await connection.QueryAsync<CountryRow>(
                "iAssetTrack_Sp_Country_List_By_Region",
                new { pVarRegion = region },
                commandType: System.Data.CommandType.StoredProcedure);
            return Results.Ok(rows);
        });

        geo.MapGet("/cities", async (int countryId, IDbConnectionFactory factory) =>
        {
            using var connection = factory.Create();
            var rows = await connection.QueryAsync<CityRow>(
                "iAssetTrack_Sp_City_List_By_Country",
                new { pIntCountryID = countryId },
                commandType: System.Data.CommandType.StoredProcedure);
            return Results.Ok(rows);
        });
    }
}

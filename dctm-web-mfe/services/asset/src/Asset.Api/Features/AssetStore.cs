using System.Data;
using Asset.Api.Infrastructure;
using Dapper;

namespace Asset.Api.Features;

// Display row for the asset list/search (direct paged query — the legacy 25-param
// search SP is deferred; this covers the common browse/filter case).
public class AssetListRow
{
    public int AssetID { get; set; }
    public string? RefNumber { get; set; }
    public string? AssetName { get; set; }
    public string? ModelName { get; set; }
    public string? MfgName { get; set; }
    public string? Site { get; set; }
    public string? Status { get; set; }
    public string? CurrentRFIDCardNumber { get; set; }
}

public sealed class AssetDetail : AssetListRow
{
    public bool IsWriteOff { get; set; }
    public bool IsMustered { get; set; }
    public bool IsPermRestrict { get; set; }
    public DateTime? BarredStartDate { get; set; }
    public DateTime? BarredEndDate { get; set; }
}

public sealed record PagedAssets(int Total, int Page, int Size, IReadOnlyList<AssetListRow> Items);

public sealed class AssetStore(IDbConnectionFactory connectionFactory)
{
    private const string BaseFrom = """
        FROM tblAsset a
        LEFT JOIN tblAssetModel m ON m.ModelID = a.ModelID
        LEFT JOIN tblManufacturer mf ON mf.MfgID = m.MfgID
        LEFT JOIN tblSite s ON s.SiteID = a.PrimarySiteID
        LEFT JOIN tblStatusMaster st ON st.StatusID = a.CurrentStatusID
        WHERE a.IsApproved = 1
        """;

    public async Task<PagedAssets> SearchAsync(string? term, int page, int size)
    {
        page = Math.Max(page, 1);
        size = Math.Clamp(size, 1, 200);

        using var connection = connectionFactory.Create();
        var filter = string.IsNullOrWhiteSpace(term)
            ? ""
            : " AND (a.RefNumber LIKE @like OR a.AssetName LIKE @like)";
        var args = new { like = $"%{term}%", skip = (page - 1) * size, take = size };

        var total = await connection.ExecuteScalarAsync<int>(
            $"SELECT COUNT(1) {BaseFrom}{filter}", args);

        var items = await connection.QueryAsync<AssetListRow>($"""
            SELECT a.AssetID, a.RefNumber, a.AssetName,
                   m.ModelName, mf.MfgName, s.Site, st.Status, a.CurrentRFIDCardNumber
            {BaseFrom}{filter}
            ORDER BY a.AssetID
            OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY
            """, args);

        return new PagedAssets(total, page, size, items.AsList());
    }

    public async Task<AssetDetail?> GetAsync(int id)
    {
        using var connection = connectionFactory.Create();
        return await connection.QuerySingleOrDefaultAsync<AssetDetail>($"""
            SELECT a.AssetID, a.RefNumber, a.AssetName,
                   m.ModelName, mf.MfgName, s.Site, st.Status, a.CurrentRFIDCardNumber,
                   a.IsWriteOff, a.IsMustered, a.IsPermRestrict, a.BarredStartDate, a.BarredEndDate
            {BaseFrom} AND a.AssetID = @id
            """, new { id });
    }

    public async Task<AssetFlags?> GetFlagsAsync(int id)
    {
        using var connection = connectionFactory.Create();
        return await connection.QuerySingleOrDefaultAsync<AssetFlags>("""
            SELECT AssetID, IsWriteOff, IsMustered, IsPermRestrict,
                   BarredStartDate, BarredEndDate, CurrentRFIDCardNumber
            FROM tblAsset WHERE AssetID = @id AND IsApproved = 1
            """, new { id });
    }

    public async Task ExecAsync(string sp, DynamicParameters parameters)
    {
        using var connection = connectionFactory.Create();
        await connection.ExecuteAsync(sp, parameters, commandType: CommandType.StoredProcedure);
    }
}

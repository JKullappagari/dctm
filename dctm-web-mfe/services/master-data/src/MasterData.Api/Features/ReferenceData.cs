using System.Data;
using Dapper;
using MasterData.Api.Infrastructure;

namespace MasterData.Api.Features;

/// <summary>
/// Read-only reference lists that back FK dropdowns on the larger forms
/// (AssetModel, and later CreateAsset). Each legacy SP only has a _List, so
/// these are query-only endpoints under /api/v1/ref. Column shapes are taken
/// verbatim from the SPs, so the JSON keys match what the frontend selects expect.
/// </summary>
public static class ReferenceData
{
    private static readonly (string Path, string Sp)[] Lists =
    [
        ("mount-types", "iAssetTrack_Sp_MountType_List"),
        ("airflow-directions", "iAssetTrack_Sp_AirFlowDirection_List"),
        ("orientations", "iAssetTrack_Sp_Orientation_List"),
        ("input-connector-types", "iAssetTrack_Sp_InputConnectorType_List"),
        ("output-connector-types", "iAssetTrack_Sp_OutputConnectorType_List"),
    ];

    public static void MapReferenceData(this IEndpointRouteBuilder api)
    {
        var group = api.MapGroup("/ref").WithTags("Reference");
        foreach (var (path, sp) in Lists)
        {
            group.MapGet($"/{path}", async (IDbConnectionFactory factory) =>
            {
                using var connection = factory.Create();
                var rows = await connection.QueryAsync(sp, commandType: CommandType.StoredProcedure);
                return Results.Ok(rows);
            });
        }
    }
}

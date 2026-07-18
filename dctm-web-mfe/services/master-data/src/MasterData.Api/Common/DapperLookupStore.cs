using System.Data;
using Dapper;
using MasterData.Api.Infrastructure;

namespace MasterData.Api.Common;

public sealed class DapperLookupStore<TRow, TWrite>(
    IDbConnectionFactory connectionFactory,
    LookupSpMap<TWrite> map) : ILookupStore<TRow, TWrite>
{
    public async Task<IReadOnlyList<TRow>> ListAsync(int id = 0)
    {
        using var connection = connectionFactory.Create();
        var parameters = new DynamicParameters();
        parameters.Add(map.IdParam, id);
        var rows = await connection.QueryAsync<TRow>(
            map.ListSp, parameters, commandType: CommandType.StoredProcedure);
        return rows.AsList();
    }

    public async Task<int> UpsertAsync(int id, TWrite dto, int userId)
    {
        using var connection = connectionFactory.Create();
        var parameters = new DynamicParameters();
        parameters.Add(map.IdParam, id, DbType.Int32, ParameterDirection.InputOutput);
        if (map.IncludeStatusOnUpsert) parameters.Add(LegacyParams.Status, 1);
        parameters.Add(LegacyParams.CreatedBy, userId);
        map.AddUpsertParams(parameters, dto);
        await connection.ExecuteAsync(
            map.UpsertSp, parameters, commandType: CommandType.StoredProcedure);
        return parameters.Get<int>(map.IdParam);
    }

    public async Task<int> CheckNameAsync(int id, TWrite dto)
    {
        using var connection = connectionFactory.Create();
        var parameters = new DynamicParameters();
        parameters.Add(map.IdParam, id);
        if (map.AddExistsParams is not null)
            map.AddExistsParams(parameters, dto);
        else
            parameters.Add(map.NameParam, map.NameOf(dto));
        return await connection.ExecuteScalarAsync<int>(
            map.ExistsSp, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteAsync(IEnumerable<int> ids, int userId)
    {
        using var connection = connectionFactory.Create();

        if (map.DeleteSqlOverride is { } sql)
        {
            // Legacy Delete SP is defective — run correct parameterized soft-delete instead.
            await connection.ExecuteAsync(sql, new { ids = ids.ToArray(), userId });
            return;
        }

        var parameters = new DynamicParameters();
        parameters.Add(map.IdsParam, string.Join(",", ids));
        if (map.DeleteTakesIdsOnly)
        {
            // e.g. AuditCycle_Delete has only the ids param (hard delete).
            await connection.ExecuteAsync(map.DeleteSp, parameters, commandType: CommandType.StoredProcedure);
            return;
        }
        parameters.Add(LegacyParams.Status, 0);
        parameters.Add(LegacyParams.LastModifiedBy, userId);
        await connection.ExecuteAsync(
            map.DeleteSp, parameters, commandType: CommandType.StoredProcedure);
    }
}

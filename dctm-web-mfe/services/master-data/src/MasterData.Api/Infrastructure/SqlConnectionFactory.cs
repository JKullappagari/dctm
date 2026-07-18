using System.Data;
using Microsoft.Data.SqlClient;

namespace MasterData.Api.Infrastructure;

public sealed class SqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    public IDbConnection Create()
    {
        var connectionString = configuration.GetConnectionString("MasterData")
            ?? throw new InvalidOperationException("Connection string 'MasterData' is not configured.");
        return new SqlConnection(connectionString);
    }
}

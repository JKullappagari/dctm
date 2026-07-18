using System.Data;
using Microsoft.Data.SqlClient;

namespace Asset.Api.Infrastructure;

public sealed class SqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    public IDbConnection Create()
    {
        var connectionString = configuration.GetConnectionString("Asset")
            ?? throw new InvalidOperationException("Connection string 'Asset' is not configured.");
        return new SqlConnection(connectionString);
    }
}

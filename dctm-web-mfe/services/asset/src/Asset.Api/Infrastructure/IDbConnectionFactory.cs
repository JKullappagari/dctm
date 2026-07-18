using System.Data;

namespace Asset.Api.Infrastructure;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}

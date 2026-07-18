using System.Data;

namespace MasterData.Api.Infrastructure;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}

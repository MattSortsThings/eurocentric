using System.Data;
using Microsoft.Data.SqlClient;

namespace Eurocentric.Infrastructure.DataAccess.Dapper;

/// <summary>
///     Application database connection factory.
/// </summary>
internal sealed class AzureSqlDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    internal AzureSqlDbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        IDbConnection connection = new SqlConnection(_connectionString);

        return Task.FromResult(connection);
    }
}

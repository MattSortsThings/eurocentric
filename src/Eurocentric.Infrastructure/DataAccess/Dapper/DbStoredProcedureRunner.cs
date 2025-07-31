using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Eurocentric.Infrastructure.DataAccess.Dapper;

/// <summary>
///     Executes stored procedures in the application database.
/// </summary>
internal sealed class DbStoredProcedureRunner : IDbStoredProcedureRunner
{
    private readonly int _commandTimeoutInSeconds;
    private readonly string _connectionString;

    private DbStoredProcedureRunner(string connectionString, int commandTimeoutInSeconds)
    {
        _connectionString = connectionString;
        _commandTimeoutInSeconds = commandTimeoutInSeconds;
    }

    public async Task<T[]> ExecuteAsync<T>(string storedProcedureName, DynamicParameters parameters,
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(storedProcedureName);
        ArgumentNullException.ThrowIfNull(parameters);

        await using SqlConnection dbConnection = new(_connectionString);

        await dbConnection.OpenAsync(cancellationToken);

        IEnumerable<T> items = await dbConnection.QueryAsync<T>(storedProcedureName,
            parameters,
            commandTimeout: _commandTimeoutInSeconds,
            commandType: CommandType.StoredProcedure);

        return items.ToArray();
    }

    internal static DbStoredProcedureRunner Create(string? connectionString, int commandTimeoutInSeconds = 30)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        return new DbStoredProcedureRunner(connectionString, commandTimeoutInSeconds);
    }
}

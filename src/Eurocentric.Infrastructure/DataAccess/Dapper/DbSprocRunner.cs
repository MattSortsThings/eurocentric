using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Eurocentric.Infrastructure.DataAccess.Dapper;

/// <summary>
///     Executes stored procedures in the application database.
/// </summary>
internal sealed class DbSprocRunner : IDbSprocRunner
{
    private readonly int _commandTimeoutInSeconds;
    private readonly string _connectionString;

    private DbSprocRunner(string connectionString, int commandTimeoutInSeconds)
    {
        _connectionString = connectionString;
        _commandTimeoutInSeconds = commandTimeoutInSeconds;
    }

    /// <inheritdoc />
    public async Task<T[]> ExecuteAsync<T>(string sprocName,
        DynamicParameters parameters,
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(sprocName);
        ArgumentNullException.ThrowIfNull(parameters);

        await using SqlConnection dbConnection = new(_connectionString);

        await dbConnection.OpenAsync(cancellationToken);

        IEnumerable<T> items = await dbConnection.QueryAsync<T>(sprocName,
            parameters,
            commandTimeout: _commandTimeoutInSeconds,
            commandType: CommandType.StoredProcedure);

        return items.ToArray();
    }

    internal static DbSprocRunner Create(string? connectionString, int commandTimeoutInSeconds = 30)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        return new DbSprocRunner(connectionString, commandTimeoutInSeconds);
    }
}

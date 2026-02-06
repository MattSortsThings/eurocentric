using System.Data;
using Dapper;
using Eurocentric.Components.DataAccess.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.DataAccess.Dapper;

/// <summary>
///     Executes stored procedures in the application database.
/// </summary>
public sealed class DbSprocRunner
{
    private readonly IOptions<DbConnectionOptions> _dbConnectionOptions;

    private DbSprocRunner(IOptions<DbConnectionOptions> dbConnectionOptions)
    {
        _dbConnectionOptions = dbConnectionOptions;
    }

    /// <summary>
    ///     Executes the named stored procedure.
    /// </summary>
    /// <param name="dbSprocName">The name of the stored procedure to be executed.</param>
    /// <param name="parameters">Optional parameters to be passed to the stored procedure.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous
    ///     operation to complete.
    /// </param>
    public async Task ExecuteAsync(
        string dbSprocName,
        DynamicParameters? parameters = null,
        CancellationToken cancellationToken = default
    )
    {
        await using SqlConnection dbConnection = new(_dbConnectionOptions.Value.ConnectionString);
        await dbConnection.OpenAsync(cancellationToken);

        try
        {
            await dbConnection.ExecuteAsync(
                dbSprocName,
                parameters,
                commandTimeout: _dbConnectionOptions.Value.CommandTimeoutInSeconds,
                commandType: CommandType.StoredProcedure
            );
        }
        finally
        {
            dbConnection.Close();
        }
    }

    internal static DbSprocRunner Create(IOptions<DbConnectionOptions> dbConnectionOptions) => new(dbConnectionOptions);
}

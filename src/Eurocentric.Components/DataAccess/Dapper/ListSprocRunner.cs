using System.Data;
using Dapper;
using Eurocentric.Components.DataAccess.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.DataAccess.Dapper;

/// <summary>
///     Executes named stored procedures that return a list of rows.
/// </summary>
/// <param name="options">Contains options used for connecting to the Azure SQL database.</param>
internal sealed class ListSprocRunner(IOptions<AzureSqlDbOptions> options)
{
    /// <summary>
    ///     Asynchronously executes the named stored procedure with the provided parameters and returns the results
    /// </summary>
    /// <param name="sprocName">The stored procedure name.</param>
    /// <param name="parameters">Parameters to be passed to the stored procedure.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous execute operation to complete.
    /// </param>
    /// <typeparam name="T">The result row type.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous execute operation. The task's result is the results
    ///     returned by the stored procedure, comprising a list of 0 or more objects of type <typeparamref name="T" />.
    /// </returns>
    public async Task<List<T>> ExecuteAsync<T>(
        string sprocName,
        DynamicParameters parameters,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        await using SqlConnection dbConnection = new(options.Value.ConnectionString);

        await dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            IEnumerable<T> query = await dbConnection.QueryAsync<T>(
                sprocName,
                parameters,
                commandTimeout: options.Value.CommandTimeoutInSeconds,
                commandType: CommandType.StoredProcedure
            );

            return query.ToList();
        }
        finally
        {
            await dbConnection.CloseAsync().ConfigureAwait(false);
        }
    }
}

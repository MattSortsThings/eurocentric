using System.Data;
using Dapper;
using Eurocentric.Components.DataAccess.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.DataAccess.Dapper;

/// <summary>
///     Executes a named stored procedure in the database.
/// </summary>
/// <param name="options">Contains options used for connecting to the Azure SQL database.</param>
internal sealed class SprocRunner(IOptions<AzureSqlDbOptions> options)
{
    /// <summary>
    ///     Asynchronously executes the named stored procedure with the provided dynamic parameters and returns the list of
    ///     results returned by the stored procedure.
    /// </summary>
    /// <param name="sprocName">The namespace-qualified name of the stored procedure to be executed.</param>
    /// <param name="parameters">Parameters to be passed to the stored procedure.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous execute operation to complete.
    /// </param>
    /// <typeparam name="T">The returned result type.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous execute operation. The task's result is the results of the
    ///     stored procedure.
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
            IEnumerable<T> query = await dbConnection
                .QueryAsync<T>(
                    sprocName,
                    commandTimeout: options.Value.CommandTimeoutInSeconds,
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                )
                .ConfigureAwait(false);

            return query.ToList();
        }
        finally
        {
            await dbConnection.CloseAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Asynchronously executes the named stored procedure with the provided dynamic parameters and returns the multiple
    ///     results returned by the stored procedure.
    /// </summary>
    /// <param name="sprocName">The namespace-qualified name of the stored procedure to be executed.</param>
    /// <param name="parameters">Parameters to be passed to the stored procedure.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous execute operation to complete.
    /// </param>
    /// <typeparam name="T1">The first returned result type.</typeparam>
    /// <typeparam name="T2">The second returned result type.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous execute operation. The task's result is a 2-tuple of the
    ///     results of the stored procedure, comprising a list of type <typeparamref name="T1" /> and a single instance of type
    ///     <typeparamref name="T2" />.
    /// </returns>
    public async Task<(List<T1> FirstResults, T2 SecondResult)> ExecuteMultipleAsync<T1, T2>(
        string sprocName,
        DynamicParameters parameters,
        CancellationToken cancellationToken = default
    )
        where T1 : class
        where T2 : class
    {
        await using SqlConnection dbConnection = new(options.Value.ConnectionString);

        await dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            await using SqlMapper.GridReader query = await dbConnection.QueryMultipleAsync(
                sprocName,
                parameters,
                commandTimeout: options.Value.CommandTimeoutInSeconds,
                commandType: CommandType.StoredProcedure
            );

            List<T1> first = new(await query.ReadAsync<T1>().ConfigureAwait(false));
            T2 second = await query.ReadSingleAsync<T2>().ConfigureAwait(false);

            return (first, second);
        }
        finally
        {
            await dbConnection.CloseAsync();
        }
    }
}

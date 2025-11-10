using System.Data;
using Dapper;
using Eurocentric.Components.DataAccess.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.DataAccess.Dapper;

/// <summary>
///     Executes named stored procedures that return a results 2-tuple comprising a single row followed by a list of rows.
/// </summary>
/// <param name="options">Contains options used for connecting to the Azure SQL database.</param>
internal sealed class SingleThenListSprocRunner(IOptions<AzureSqlDbOptions> options)
{
    /// <summary>
    ///     Asynchronously executes the named stored procedure with the provided parameters and returns the results 2-tuple.
    /// </summary>
    /// <param name="sprocName">The stored procedure name.</param>
    /// <param name="parameters">Parameters to be passed to the stored procedure.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous execute operation to complete.
    /// </param>
    /// <typeparam name="T1">The first result row type.</typeparam>
    /// <typeparam name="T2">The second result row type.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous execute operation. The task's result is the results
    ///     2-tuple returned by the stored procedure, comprising a single object of type <typeparamref name="T1" /> and
    ///     a list of 0 or more objects of type <typeparamref name="T2" />.
    /// </returns>
    public async Task<(T1 FirstResult, List<T2> SecondResult)> ExecuteAsync<T1, T2>(
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
            await using SqlMapper.GridReader queryReader = await dbConnection.QueryMultipleAsync(
                sprocName,
                parameters,
                commandTimeout: options.Value.CommandTimeoutInSeconds,
                commandType: CommandType.StoredProcedure
            );

            T1 firstResult = queryReader.ReadSingle<T1>();

            List<T2> secondResult = queryReader.Read<T2>().ToList();

            return (firstResult, secondResult);
        }
        finally
        {
            await dbConnection.CloseAsync().ConfigureAwait(false);
        }
    }
}

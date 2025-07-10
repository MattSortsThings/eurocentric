using Dapper;

namespace Eurocentric.Infrastructure.DataAccess.Dapper;

/// <summary>
///     Allows the client to run a stored procedure on the application database.
/// </summary>
public interface IDbStoredProcedureRunner
{
    /// <summary>
    ///     Executes the specified stored procedure on the application database and returns the paired data retrieved by the
    ///     query.
    /// </summary>
    /// <param name="storedProcedureName">The database stored procedure name.</param>
    /// <param name="parameters">The dynamic parameters to be passed to the stored procedure.</param>
    /// <param name="cancellationToken">
    ///     A cancellation token to observe while waiting for the asynchronous operation to complete.
    /// </param>
    /// <typeparam name="T1">The first record type.</typeparam>
    /// <typeparam name="T2">The second record type.</typeparam>
    /// <returns>
    ///     A 2-tuple comprising a single record of type <typeparamref name="T1" /> and an array of record of type
    ///     <typeparamref name="T2" />.
    /// </returns>
    public Task<(T1 First, T2[] Second)> ExecuteAsync<T1, T2>(string storedProcedureName,
        DynamicParameters? parameters = null,
        CancellationToken cancellationToken = default)
        where T1 : class where T2 : class;
}

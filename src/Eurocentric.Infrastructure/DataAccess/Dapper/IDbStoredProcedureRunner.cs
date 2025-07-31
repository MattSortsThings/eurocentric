using Dapper;

namespace Eurocentric.Infrastructure.DataAccess.Dapper;

/// <summary>
///     Executes stored procedures in the application database.
/// </summary>
public interface IDbStoredProcedureRunner
{
    /// <summary>
    ///     Asynchronously executes the specified stored procedure with the provided parameters and returns the results.
    /// </summary>
    /// <param name="storedProcedureName">The schema-qualified name of the stored procedure to be run.</param>
    /// <param name="parameters">Contains input and output parameters to be passed to the stored procedure.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the operation to complete.</param>
    /// <typeparam name="T">
    ///     The stored procedure result record type. This must be a record type with a parameterless
    ///     constructor and init-only properties matching the snake-case column names and data types of the stored procedure
    ///     result record type.
    /// </typeparam>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation. The result of the task contains the ordered
    ///     records returned by the stored procedure.
    /// </returns>
    public Task<T[]> ExecuteAsync<T>(string storedProcedureName,
        DynamicParameters parameters,
        CancellationToken cancellationToken = default)
        where T : class;
}

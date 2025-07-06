using System.Data;

namespace Eurocentric.Infrastructure.DataAccess.Dapper;

/// <summary>
///     Application database connection factory.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    ///     Asynchronously creates and returns a connection to the application database.
    /// </summary>
    /// <remarks>The client is responsible for disposing of the database connection.</remarks>
    /// <param name="cancellationToken">Propagates a notification that the asynchronous operation should be cancelled.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The result of the task is a new instance of a type that
    ///     implements <see cref="IDbConnection" />.
    /// </returns>
    public Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}

namespace Eurocentric.Domain.Abstractions.Repositories;

/// <summary>
///     A unit of work that is committed as a single transaction.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///     Commits the unit of work.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    Task CommitAsync(CancellationToken cancellationToken = default);
}

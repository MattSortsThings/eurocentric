namespace Eurocentric.Domain.Persistence;

/// <summary>
///     A unit of work that is committed or rolled back as a single transaction.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///     Commits all the changes as a single transaction, or rolls them all back if any is unsuccessful.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    Task CommitAsync(CancellationToken cancellationToken = default);
}

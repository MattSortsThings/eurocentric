namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Read-write repository for <see cref="Contest" /> aggregates.
/// </summary>
public interface IContestWriteRepository
{
    /// <summary>
    ///     Adds the specified <see cref="Contest" /> aggregate to the repository.
    /// </summary>
    /// <remarks>Changes are not committed until the <see cref="SaveChangesAsync" /> method is invoked.</remarks>
    /// <param name="contest">The contest to be added.</param>
    void Add(Contest contest);

    /// <summary>
    ///     Asynchronously saves all changes in the current transaction.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous commit operation to complete.
    /// </param>
    /// <returns>A <see cref="Task" /> representing the asynchronous commit operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

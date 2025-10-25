namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Read-only repository for <see cref="Contest" /> aggregates.
/// </summary>
public interface IContestReadRepository
{
    /// <summary>
    ///     Asynchronously retrieves all the <see cref="Contest" /> aggregates in the system, in an untracked state, ordered by
    ///     <see cref="Contest.ContestYear" />.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous retrieval operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous retrieval operation. The task result is the retrieved array
    ///     of untracked <see cref="Contest" /> aggregates.
    /// </returns>
    Task<Contest[]> GetAllAsync(CancellationToken cancellationToken = default);
}

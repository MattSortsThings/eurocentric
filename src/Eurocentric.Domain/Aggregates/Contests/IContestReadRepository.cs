using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

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

    /// <summary>
    ///     Asynchronously retrieves the <see cref="Contest" /> aggregate in the system with the specified
    ///     <see cref="Contest.Id" /> value, in an untracked state, if a matching aggregate exists.
    /// </summary>
    /// <param name="contestId">The ID of the requested contest.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous retrieval operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous retrieval operation. The task result is <i>either</i>
    ///     the retrieved untracked <see cref="Contest" /> <i>or</i> an error.
    /// </returns>
    Task<Result<Contest, IDomainError>> GetByIdAsync(
        ContestId contestId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Returns all the <see cref="Contest" /> aggregates in the system as an untracked queryable.
    /// </summary>
    /// <returns>An object to allow read-only queries on the <see cref="Contest" /> aggregates.</returns>
    IQueryable<Contest> GetAsQueryable();
}

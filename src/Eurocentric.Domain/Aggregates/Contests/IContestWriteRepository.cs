using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

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
    ///     Updates the specified <see cref="Contest" /> aggregate in the repository.
    /// </summary>
    /// <remarks>Changes are not committed until the <see cref="SaveChangesAsync" /> method is invoked.</remarks>
    /// <param name="contest">The contest to be updated.</param>
    void Update(Contest contest);

    /// <summary>
    ///     Asynchronously retrieves the <see cref="Contest" /> aggregate in the system with the specified
    ///     <see cref="Contest.Id" /> value, in a tracked state, if a matching aggregate exists.
    /// </summary>
    /// <param name="contestId">The ID of the requested contest.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous retrieval operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous retrieval operation. The task result is <i>either</i>
    ///     the retrieved tracked <see cref="Contest" /> <i>or</i> an error.
    /// </returns>
    Task<Result<Contest, IDomainError>> GetByIdAsync(
        ContestId contestId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Asynchronously saves all changes in the current transaction.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous commit operation to complete.
    /// </param>
    /// <returns>A <see cref="Task" /> representing the asynchronous commit operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Read-only repository for <see cref="Broadcast" /> aggregates.
/// </summary>
public interface IBroadcastReadRepository
{
    /// <summary>
    ///     Asynchronously retrieves all the <see cref="Broadcast" /> aggregates in the system, in an untracked state,
    ///     ordered by <see cref="Broadcast.BroadcastDate" />.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous retrieval operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous retrieval operation. The task result is the retrieved
    ///     array of untracked <see cref="Broadcast" /> aggregates.
    /// </returns>
    Task<Broadcast[]> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves the <see cref="Broadcast" /> aggregate in the system with the specified
    ///     <see cref="Broadcast.Id" /> value, in an untracked state, if a matching aggregate exists.
    /// </summary>
    /// <param name="broadcastId">The ID of the requested broadcast.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous retrieval operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous retrieval operation. The task result is <i>either</i>
    ///     the retrieved untracked <see cref="Broadcast" /> <i>or</i> an error.
    /// </returns>
    Task<Result<Broadcast, IDomainError>> GetByIdAsync(
        BroadcastId broadcastId,
        CancellationToken cancellationToken = default
    );
}

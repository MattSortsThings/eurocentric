using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Read-write repository for <see cref="Broadcast" /> aggregates.
/// </summary>
public interface IBroadcastWriteRepository
{
    /// <summary>
    ///     Adds the specified <see cref="Broadcast" /> aggregate to the repository.
    /// </summary>
    /// <remarks>Changes are not committed until the <see cref="SaveChangesAsync" /> method is invoked.</remarks>
    /// <param name="broadcast">The broadcast to be added.</param>
    void Add(Broadcast broadcast);

    /// <summary>
    ///     Removes the specified <see cref="Broadcast" /> aggregate from the repository.
    /// </summary>
    /// <remarks>Changes are not committed until the <see cref="SaveChangesAsync" /> method is invoked.</remarks>
    /// <param name="broadcast">The broadcast to be removed.</param>
    void Remove(Broadcast broadcast);

    /// <summary>
    ///     Asynchronously retrieves the <see cref="Broadcast" /> aggregate in the system with the specified
    ///     <see cref="Broadcast.Id" /> value, in a tracked state, if a matching aggregate exists.
    /// </summary>
    /// <param name="broadcastId">The ID of the requested broadcast.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous retrieval operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous retrieval operation. The task result is <i>either</i>
    ///     the retrieved tracked <see cref="Broadcast" /> <i>or</i> an error.
    /// </returns>
    Task<Result<Broadcast, IDomainError>> GetByIdAsync(
        BroadcastId broadcastId,
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

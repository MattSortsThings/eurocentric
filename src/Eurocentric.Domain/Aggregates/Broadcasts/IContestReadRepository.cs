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
}

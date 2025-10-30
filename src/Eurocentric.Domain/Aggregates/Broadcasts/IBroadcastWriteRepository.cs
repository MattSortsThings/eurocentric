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
    ///     Asynchronously saves all changes in the current transaction.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous commit operation to complete.
    /// </param>
    /// <returns>A <see cref="Task" /> representing the asynchronous commit operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

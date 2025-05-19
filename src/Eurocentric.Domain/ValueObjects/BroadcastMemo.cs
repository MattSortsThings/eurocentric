using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Summarizes a broadcast aggregate.
/// </summary>
public sealed class BroadcastMemo : ValueObject
{
    /// <summary>
    ///     Creates a new <see cref="BroadcastMemo" /> instance.
    /// </summary>
    /// <param name="broadcastId">Identifies the broadcast aggregate.</param>
    /// <param name="contestStage">The broadcast aggregate's stage in its parent contest.</param>
    /// <param name="status">The current status of the broadcast aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    public BroadcastMemo(BroadcastId broadcastId, ContestStage contestStage, BroadcastStatus status)
    {
        BroadcastId = broadcastId ?? throw new ArgumentNullException(nameof(broadcastId));
        ContestStage = contestStage;
        Status = status;
    }

    /// <summary>
    ///     Gets the ID of the broadcast aggregate.
    /// </summary>
    public BroadcastId BroadcastId { get; }

    /// <summary>
    ///     Gets the broadcast aggregate's stage in its parent contest aggregate.
    /// </summary>
    public ContestStage ContestStage { get; }

    /// <summary>
    ///     Gets the current status of the broadcast aggregate.
    /// </summary>
    public BroadcastStatus Status { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return BroadcastId;
        yield return ContestStage;
        yield return Status;
    }
}

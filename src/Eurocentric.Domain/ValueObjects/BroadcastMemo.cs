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
    /// <param name="completed">Indicates whether the broadcast has been completed.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    public BroadcastMemo(BroadcastId broadcastId, ContestStage contestStage, bool completed = false)
    {
        BroadcastId = broadcastId ?? throw new ArgumentNullException(nameof(broadcastId));
        ContestStage = contestStage;
        Completed = completed;
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
    ///     Gets a boolean value indicating whether the broadcast has been completed.
    /// </summary>
    public bool Completed { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return BroadcastId;
        yield return ContestStage;
        yield return Completed;
    }
}

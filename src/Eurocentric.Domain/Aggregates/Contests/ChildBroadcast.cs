using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a broadcast aggregate in its parent contest.
/// </summary>
public sealed class ChildBroadcast : Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private ChildBroadcast()
    {
    }

    public ChildBroadcast(BroadcastId broadcastId, ContestStage contestStage)
    {
        BroadcastId = broadcastId;
        ContestStage = contestStage;
    }

    /// <summary>
    ///     Gets the ID of the broadcast aggregate represented by the child broadcast.
    /// </summary>
    public BroadcastId BroadcastId { get; private init; } = null!;

    /// <summary>
    ///     Gets the broadcast's stage in its parent contest.
    /// </summary>
    public ContestStage ContestStage { get; private init; }

    /// <summary>
    ///     Gets a boolean value indicating whether all points have been awarded in the broadcast.
    /// </summary>
    public bool Completed { get; private set; }
}

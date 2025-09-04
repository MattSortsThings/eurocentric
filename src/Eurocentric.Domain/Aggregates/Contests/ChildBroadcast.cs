using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a single broadcast from the perspective of its parent contest.
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
    ///     Gets the ID of the broadcast.
    /// </summary>
    public BroadcastId BroadcastId { get; private init; } = null!;

    /// <summary>
    ///     Gets the broadcast's stage in its parent contest.
    /// </summary>
    public ContestStage ContestStage { get; private init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the broadcast is completed.
    /// </summary>
    public bool Completed { get; private set; }
}

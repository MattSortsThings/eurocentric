using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Aggregates.Contests;

/// <summary>
///     Represents a broadcast from the perspective of its parent contest.
/// </summary>
public sealed record ChildBroadcast
{
    /// <summary>
    ///     Gets the broadcast's ID.
    /// </summary>
    public Guid ChildBroadcastId { get; init; }

    /// <summary>
    ///     Gets the broadcast's contest stage.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     Gets a value indicating whether the broadcast is completed.
    /// </summary>
    public bool Completed { get; init; }
}

using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a broadcast from the perspective of its parent contest.
/// </summary>
public sealed record ChildBroadcast
{
    /// <summary>
    ///     Gets the ID of the child broadcast.
    /// </summary>
    public Guid ChildBroadcastId { get; init; }

    /// <summary>
    ///     Gets the child broadcast's contest stage in the contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the child broadcast is completed.
    /// </summary>
    public bool Completed { get; init; }
}

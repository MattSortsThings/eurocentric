using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0.Contests;

/// <summary>
///     Represents a broadcast from the perspective of its parent contest.
/// </summary>
public sealed class ChildBroadcast
{
    /// <summary>
    ///     Gets or initializes the child broadcast's ID.
    /// </summary>
    public required Guid ChildBroadcastId { get; init; }

    /// <summary>
    ///     Gets or initializes the child broadcast's contest stage.
    /// </summary>
    public required ContestStage ContestStage { get; init; }

    /// <summary>
    ///     Gets or sets a boolean value denoting whether the child broadcast has been completed.
    /// </summary>
    public required bool Completed { get; set; }
}

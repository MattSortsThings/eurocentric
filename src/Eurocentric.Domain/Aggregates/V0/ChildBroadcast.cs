using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a broadcast from the perspective of its parent contest.
/// </summary>
public sealed class ChildBroadcast
{
    /// <summary>
    ///     Gets the child broadcast's ID.
    /// </summary>
    public Guid ChildBroadcastId { get; init; }

    /// <summary>
    ///     Gets the child broadcast's contest stage.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     Gets the child broadcast's voting rules.
    /// </summary>
    public VotingRules VotingRules { get; init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the child broadcast is completed.
    /// </summary>
    public bool Completed { get; init; }
}

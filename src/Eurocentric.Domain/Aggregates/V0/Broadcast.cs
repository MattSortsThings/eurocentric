using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a broadcast.
/// </summary>
public sealed record Broadcast
{
    /// <summary>
    ///     Gets the broadcast's ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Gets the broadcast's date.
    /// </summary>
    public DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     Gets the ID of the broadcast's parent contest.
    /// </summary>
    public Guid ParentContestId { get; init; }

    /// <summary>
    ///     Gets the broadcast's stage in its parent contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     Gets the broadcast's voting rules.
    /// </summary>
    public VotingRules VotingRules { get; init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the child broadcast is completed.
    /// </summary>
    public bool Completed { get; init; }

    /// <summary>
    ///     Gets an unordered list of the contest's competitors.
    /// </summary>
    public List<Competitor> Competitors { get; init; } = [];

    /// <summary>
    ///     Gets an unordered list of the contest's televotes.
    /// </summary>
    public List<Televote> Televotes { get; init; } = [];

    /// <summary>
    ///     Gets an unordered list of the contest's juries.
    /// </summary>
    public List<Jury> Juries { get; init; } = [];
}

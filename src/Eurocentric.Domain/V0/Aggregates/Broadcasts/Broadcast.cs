using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Aggregates.Broadcasts;

/// <summary>
///     Represents a broadcast aggregate.
/// </summary>
public sealed record Broadcast
{
    /// ///
    /// <summary>
    ///     Gets the aggregate's system ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Gets the date on which the broadcast is televised.
    /// </summary>
    public DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     Gets the ID of the parent contest.
    /// </summary>
    public Guid ParentContestId { get; init; }

    /// <summary>
    ///     Gets the broadcast's stage in its contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the broadcast is completed.
    /// </summary>
    public bool Completed { get; init; }

    /// <summary>
    ///     Gets a list of all the competitors in the broadcast.
    /// </summary>
    public List<Competitor> Competitors { get; init; } = [];

    /// <summary>
    ///     Gets a list of all the juries in the broadcast, if any.
    /// </summary>
    public List<Jury> Juries { get; init; } = [];

    /// <summary>
    ///     Gets a list of all the televotes in the broadcast.
    /// </summary>
    public List<Televote> Televotes { get; init; } = [];
}

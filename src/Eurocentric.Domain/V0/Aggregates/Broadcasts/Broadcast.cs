using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Aggregates.Broadcasts;

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
    ///     Gets the date on which the broadcast is televised.
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
    ///     Gets a value indicating whether the broadcast is completed.
    /// </summary>
    public bool Completed { get; init; }

    /// <summary>
    ///     Gets a list of all the competitors in the broadcast.
    /// </summary>
    public List<Competitor> Competitors { get; init; } = [];

    /// <summary>
    ///     Gets a list of all the televotes in the broadcast.
    /// </summary>
    public List<Televote> Televotes { get; init; } = [];

    /// <summary>
    ///     Gets a list of all the juries in the broadcast.
    /// </summary>
    public List<Jury> Juries { get; init; } = [];
}

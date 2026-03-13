using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0.Broadcasts;

/// <summary>
///     Represents a broadcast.
/// </summary>
public sealed class Broadcast
{
    /// <summary>
    ///     Gets or initializes the aggregate's system identifier.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    ///     Gets or initializes the broadcast's date.
    /// </summary>
    public required DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     Gets or initializes the broadcast's parent contest ID.
    /// </summary>
    public required Guid ParentContestId { get; init; }

    /// <summary>
    ///     Gets or initializes the broadcast's contest stage.
    /// </summary>
    public required ContestStage ContestStage { get; init; }

    /// <summary>
    ///     Gets or initializes the broadcast's format.
    /// </summary>
    public required BroadcastFormat BroadcastFormat { get; init; }

    /// <summary>
    ///     Gets or sets a boolean value denoting whether the broadcast has been completed.
    /// </summary>
    public required bool Completed { get; set; }

    /// <summary>
    ///     Gets or initializes an unordered list of the broadcast's competitors.
    /// </summary>
    public required List<Competitor> Competitors { get; init; }

    /// <summary>
    ///     Gets or initializes an unordered list of the broadcast's juries; this may be empty.
    /// </summary>
    public required List<Jury> Juries { get; init; }

    /// <summary>
    ///     Gets or initializes an unordered list of the broadcast's televotes.
    /// </summary>
    public required List<Televote> Televotes { get; init; }
}

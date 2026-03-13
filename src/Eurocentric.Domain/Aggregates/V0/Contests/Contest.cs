using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0.Contests;

/// <summary>
///     Represents a contest.
/// </summary>
public sealed class Contest
{
    /// <summary>
    ///     Gets or initializes the aggregate's system identifier.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    ///     Gets or initializes the contest's year.
    /// </summary>
    public required int ContestYear { get; init; }

    /// <summary>
    ///     Gets or initializes the UK English name of the contest's host city.
    /// </summary>
    public required string CityName { get; init; }

    /// <summary>
    ///     Gets or initializes the contest's Semi-Final broadcast format.
    /// </summary>
    public required BroadcastFormat SemiFinalBroadcastFormat { get; init; }

    /// <summary>
    ///     Gets or initializes the contest's Grand Final broadcast format.
    /// </summary>
    public required BroadcastFormat GrandFinalBroadcastFormat { get; init; }

    /// <summary>
    ///     Gets or sets a boolean value denoting whether the contest is queryable.
    /// </summary>
    public required bool Queryable { get; set; }

    /// <summary>
    ///     Gets or initializes the contest's optional global televote.
    /// </summary>
    public GlobalTelevote? GlobalTelevote { get; init; }

    /// <summary>
    ///     Gets or initializes an unordered list of the contest's child broadcasts.
    /// </summary>
    public required List<ChildBroadcast> ChildBroadcasts { get; init; }

    /// <summary>
    ///     Gets or initializes an unordered list of the contest's participants.
    /// </summary>
    public required List<Participant> Participants { get; init; }
}

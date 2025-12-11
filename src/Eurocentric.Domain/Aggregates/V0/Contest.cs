using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a contest.
/// </summary>
public sealed record Contest
{
    /// <summary>
    ///     Gets the contest ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Gets the contest's year.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     Gets the contest's city name.
    /// </summary>
    public string CityName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the contest's Grand Final voting rules.
    /// </summary>
    public VotingRules GrandFinalVotingRules { get; init; }

    /// <summary>
    ///     Gets the contest's Semi-Final voting rules.
    /// </summary>
    public VotingRules SemiFinalVotingRules { get; init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the contest is queryable.
    /// </summary>
    public bool Queryable { get; init; }

    /// <summary>
    ///     Gets the contest's optional global televote.
    /// </summary>
    public GlobalTelevote? GlobalTelevote { get; init; }

    /// <summary>
    ///     Gets an unordered list of the contest's child broadcasts.
    /// </summary>
    public List<ChildBroadcast> ChildBroadcasts { get; init; } = [];

    /// <summary>
    ///     Gets an unordered list of the contest's participants.
    /// </summary>
    public List<Participant> Participants { get; init; } = [];
}

using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a contest.
/// </summary>
public sealed class Contest
{
    /// <summary>
    ///     Gets the contest's ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Gets the contest's year.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     Gets the contest's host city name.
    /// </summary>
    public string CityName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the Semi-Final voting rules for the contest.
    /// </summary>
    public VotingRules SemiFinalVotingRules { get; init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the contest is completed.
    /// </summary>
    public bool Completed { get; init; }

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

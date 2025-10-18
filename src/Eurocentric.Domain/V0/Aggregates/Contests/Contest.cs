using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Aggregates.Contests;

/// <summary>
///     Represents a contest.
/// </summary>
public abstract record Contest
{
    /// <summary>
    ///     Gets the contest's ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Gets the year in which the contest is held.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     Gets the UK English name of the city in which the contest is held.
    /// </summary>
    public string CityName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the contest's rules.
    /// </summary>
    public abstract ContestRules ContestRules { get; init; }

    /// <summary>
    ///     Gets a value indicating whether the contest and its associated data are queryable.
    /// </summary>
    public bool Queryable { get; init; }

    /// <summary>
    ///     Gets a list of all the contest's child broadcasts.
    /// </summary>
    public List<ChildBroadcast> ChildBroadcasts { get; init; } = [];

    /// <summary>
    ///     Gets a list of all the contest's participants.
    /// </summary>
    public List<Participant> Participants { get; init; } = [];

    /// <summary>
    ///     Gets the contest's global televote, if it has one.
    /// </summary>
    public GlobalTelevote? GlobalTelevote { get; init; }
}

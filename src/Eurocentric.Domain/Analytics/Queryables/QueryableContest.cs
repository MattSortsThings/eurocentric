namespace Eurocentric.Domain.Analytics.Queryables;

/// <summary>
///     A queryable contest.
/// </summary>
/// <remarks>A contest is queryable once all three of its child broadcasts have been created and completed.</remarks>
public sealed record QueryableContest
{
    /// <summary>
    ///     Gets the year in which the contest is held.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     Gets the UK English name of the city in which the contest is held.
    /// </summary>
    public string CityName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the number of participants in the contest.
    /// </summary>
    public int Participants { get; init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the contest uses the "Rest of the World" televote.
    /// </summary>
    public bool UsesRestOfWorldTelevote { get; init; }
}

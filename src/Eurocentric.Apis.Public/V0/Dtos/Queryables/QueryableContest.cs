using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V0.Dtos.Queryables;

/// <summary>
///     A queryable contest.
/// </summary>
/// <remarks>A contest is queryable once all three of its child broadcasts have been created and completed.</remarks>
public sealed record QueryableContest : ISchemaExampleProvider<QueryableContest>
{
    /// <summary>
    ///     The year in which the contest is held.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The UK English name of the city in which the contest is held.
    /// </summary>
    public string CityName { get; init; } = string.Empty;

    /// <summary>
    ///     The number of participants in the contest.
    /// </summary>
    public int Participants { get; init; }

    /// <summary>
    ///     A boolean value indicating whether the contest uses the "Rest of the World" televote.
    /// </summary>
    public bool UsesRestOfWorldTelevote { get; init; }

    public static QueryableContest CreateExample() =>
        new()
        {
            ContestYear = 2025,
            CityName = "Basel",
            Participants = 37,
            UsesRestOfWorldTelevote = true,
        };
}

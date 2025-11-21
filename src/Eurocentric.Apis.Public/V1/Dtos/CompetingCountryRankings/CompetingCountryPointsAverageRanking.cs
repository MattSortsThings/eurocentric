using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.CompetingCountryRankings;

/// <summary>
///     A single competing country points average rankings row.
/// </summary>
public sealed record CompetingCountryPointsAverageRanking
    : IDtoSchemaExampleProvider<CompetingCountryPointsAverageRanking>
{
    /// <summary>
    ///     The competing country's rank based on descending points average.
    /// </summary>
    public int Rank { get; init; }

    /// <summary>
    ///     The competing country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The competing country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     The average individual points value the competing country received across broadcasts.
    /// </summary>
    public decimal PointsAverage { get; init; }

    /// <summary>
    ///     The sum total points the competing country received across broadcasts.
    /// </summary>
    public int TotalPoints { get; init; }

    /// <summary>
    ///     The number of points awards in the queried filtered voting data for the competing country.
    /// </summary>
    public int PointsAwards { get; init; }

    /// <summary>
    ///     The number of unique broadcasts in the queried filtered voting data for the competing country.
    /// </summary>
    public int Broadcasts { get; init; }

    /// <summary>
    ///     The number of unique contests in the queried filtered voting data for the competing country.
    /// </summary>
    public int Contests { get; init; }

    /// <summary>
    ///     The number of unique voting countries in the queried filtered voting data for the competing country.
    /// </summary>
    public int VotingCountries { get; init; }

    public static CompetingCountryPointsAverageRanking CreateExample() =>
        new()
        {
            Rank = 1,
            CountryCode = "AA",
            CountryName = "CountryName",
            PointsAverage = 9.5m,
            TotalPoints = 1900,
            PointsAwards = 200,
            Broadcasts = 4,
            Contests = 2,
            VotingCountries = 50,
        };
}

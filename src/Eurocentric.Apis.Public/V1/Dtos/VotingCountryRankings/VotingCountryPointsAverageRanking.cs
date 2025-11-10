namespace Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;

/// <summary>
///     A single voting country points average rankings row.
/// </summary>
public sealed record VotingCountryPointsAverageRanking
{
    /// <summary>
    ///     The voting country's rank based on descending points average.
    /// </summary>
    public int Rank { get; init; }

    /// <summary>
    ///     The voting country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The voting country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     The average individual points value the voting country gave to the specified competing country across broadcasts.
    /// </summary>
    public decimal PointsAverage { get; init; }

    /// <summary>
    ///     The sum total points the voting country gave to the specified competing country across broadcasts.
    /// </summary>
    public int TotalPoints { get; init; }

    /// <summary>
    ///     The quantity of points awards the competing country gave to the specified competing country across broadcasts.
    /// </summary>
    public int PointsAwards { get; init; }

    /// <summary>
    ///     The number of unique broadcasts in the queried voting data for the voting country.
    /// </summary>
    public int Broadcasts { get; init; }

    /// <summary>
    ///     The number of unique contests in the queried voting data for the voting country.
    /// </summary>
    public int Contests { get; init; }
}

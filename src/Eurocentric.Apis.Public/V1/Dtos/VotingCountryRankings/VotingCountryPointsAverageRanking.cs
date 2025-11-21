using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;

/// <summary>
///     A single voting country points average rankings row.
/// </summary>
public sealed record VotingCountryPointsAverageRanking : IDtoSchemaExampleProvider<VotingCountryPointsAverageRanking>
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
    ///     The number of points awards in the queried filtered voting data for the voting country.
    /// </summary>
    public int PointsAwards { get; init; }

    /// <summary>
    ///     The number of unique broadcasts in the queried filtered voting data for the voting country.
    /// </summary>
    public int Broadcasts { get; init; }

    /// <summary>
    ///     The number of unique contests in the queried filtered voting data for the voting country.
    /// </summary>
    public int Contests { get; init; }

    public static VotingCountryPointsAverageRanking CreateExample() =>
        new()
        {
            Rank = 1,
            CountryCode = "AA",
            CountryName = "CountryName",
            PointsAverage = 9.5m,
            TotalPoints = 38,
            PointsAwards = 4,
            Broadcasts = 4,
            Contests = 2,
        };

    public bool Equals(VotingCountryPointsAverageRanking? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Rank == other.Rank
            && CountryCode == other.CountryCode
            && CountryName == other.CountryName
            && PointsAverage == other.PointsAverage
            && TotalPoints == other.TotalPoints
            && PointsAwards == other.PointsAwards
            && Broadcasts == other.Broadcasts
            && Contests == other.Contests;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Rank,
            CountryCode,
            CountryName,
            PointsAverage,
            TotalPoints,
            PointsAwards,
            Broadcasts,
            Contests
        );
}

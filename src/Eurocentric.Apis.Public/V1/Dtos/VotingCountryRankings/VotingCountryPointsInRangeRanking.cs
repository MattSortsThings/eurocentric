using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;

/// <summary>
///     A single voting country points in range rankings row.
/// </summary>
public sealed record VotingCountryPointsInRangeRanking : IDtoSchemaExampleProvider<VotingCountryPointsInRangeRanking>
{
    /// <summary>
    ///     The voting country's rank based on descending points in range.
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
    ///     The frequency of points awards within the specified value range the voting country gave to the specified
    ///     competing country, relative to the number of points awards the voting country gave.
    /// </summary>
    public decimal PointsInRange { get; init; }

    /// <summary>
    ///     The frequency of points awards within the specified value range the voting country gave to the specified
    ///     competing country.
    /// </summary>
    public int PointsAwardsInRange { get; init; }

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

    public static VotingCountryPointsInRangeRanking CreateExample() =>
        new()
        {
            Rank = 1,
            CountryCode = "AA",
            CountryName = "CountryName",
            PointsInRange = 0.75m,
            PointsAwardsInRange = 3,
            PointsAwards = 4,
            Broadcasts = 4,
            Contests = 2,
        };

    public bool Equals(VotingCountryPointsInRangeRanking? other)
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
            && PointsInRange == other.PointsInRange
            && PointsAwardsInRange == other.PointsAwardsInRange
            && PointsAwards == other.PointsAwards
            && Broadcasts == other.Broadcasts
            && Contests == other.Contests;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Rank,
            CountryCode,
            CountryName,
            PointsInRange,
            PointsAwardsInRange,
            PointsAwards,
            Broadcasts,
            Contests
        );
}

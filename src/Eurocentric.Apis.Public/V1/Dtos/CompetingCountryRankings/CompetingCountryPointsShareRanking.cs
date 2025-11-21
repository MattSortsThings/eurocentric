using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.CompetingCountryRankings;

/// <summary>
///     A single competing country points share rankings row.
/// </summary>
public sealed record CompetingCountryPointsShareRanking : IDtoSchemaExampleProvider<CompetingCountryPointsShareRanking>
{
    /// <summary>
    ///     The competing country's rank based on descending points share.
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
    ///     The sum total points the competing country received across broadcasts, as a fraction of the available points.
    /// </summary>
    public decimal PointsShare { get; init; }

    /// <summary>
    ///     The sum total points the competing country received across broadcasts.
    /// </summary>
    public int TotalPoints { get; init; }

    /// <summary>
    ///     The maximum available points the competing country could have received across broadcasts.
    /// </summary>
    public int AvailablePoints { get; init; }

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

    public static CompetingCountryPointsShareRanking CreateExample() =>
        new()
        {
            Rank = 1,
            CountryCode = "AA",
            CountryName = "CountryName",
            PointsShare = 0.75m,
            TotalPoints = 1800,
            AvailablePoints = 2400,
            PointsAwards = 200,
            Broadcasts = 4,
            Contests = 2,
            VotingCountries = 50,
        };

    public bool Equals(CompetingCountryPointsShareRanking? other)
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
            && PointsShare == other.PointsShare
            && TotalPoints == other.TotalPoints
            && AvailablePoints == other.AvailablePoints
            && PointsAwards == other.PointsAwards
            && Broadcasts == other.Broadcasts
            && Contests == other.Contests
            && VotingCountries == other.VotingCountries;
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Rank);
        hashCode.Add(CountryCode);
        hashCode.Add(CountryName);
        hashCode.Add(PointsShare);
        hashCode.Add(TotalPoints);
        hashCode.Add(AvailablePoints);
        hashCode.Add(PointsAwards);
        hashCode.Add(Broadcasts);
        hashCode.Add(Contests);
        hashCode.Add(VotingCountries);

        return hashCode.ToHashCode();
    }
}

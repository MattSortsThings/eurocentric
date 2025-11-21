using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;

/// <summary>
///     A single voting country points share rankings row.
/// </summary>
public sealed record VotingCountryPointsShareRanking : IDtoSchemaExampleProvider<VotingCountryPointsShareRanking>
{
    /// <summary>
    ///     The voting country's rank based on descending points share.
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
    ///     The sum total points the voting country gave to the specified competing country across broadcasts, as a
    ///     fraction of the available points.
    /// </summary>
    public decimal PointsShare { get; init; }

    /// <summary>
    ///     The sum total points the voting country gave to the specified competing country across broadcasts.
    /// </summary>
    public int TotalPoints { get; init; }

    /// <summary>
    ///     The maximum available points the voting country could have given to the specified competing country across
    ///     broadcasts.
    /// </summary>
    public int AvailablePoints { get; init; }

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

    public static VotingCountryPointsShareRanking CreateExample() =>
        new()
        {
            Rank = 1,
            CountryCode = "AA",
            CountryName = "CountryName",
            PointsShare = 0.75m,
            TotalPoints = 36,
            AvailablePoints = 48,
            PointsAwards = 4,
            Broadcasts = 4,
            Contests = 2,
        };

    public bool Equals(VotingCountryPointsShareRanking? other)
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
            && Contests == other.Contests;
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

        return hashCode.ToHashCode();
    }
}

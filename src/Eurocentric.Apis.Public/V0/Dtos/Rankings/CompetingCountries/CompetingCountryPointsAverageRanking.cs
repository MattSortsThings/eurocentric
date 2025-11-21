using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V0.Dtos.Rankings.CompetingCountries;

public sealed record CompetingCountryPointsAverageRanking
    : IDtoSchemaExampleProvider<CompetingCountryPointsAverageRanking>
{
    public int Rank { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public decimal PointsAverage { get; init; }

    public int TotalPoints { get; init; }

    public int PointsAwards { get; init; }

    public int Broadcasts { get; init; }

    public int Contests { get; init; }

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

    public bool Equals(CompetingCountryPointsAverageRanking? other)
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
            && Contests == other.Contests
            && VotingCountries == other.VotingCountries;
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Rank);
        hashCode.Add(CountryCode);
        hashCode.Add(CountryName);
        hashCode.Add(PointsAverage);
        hashCode.Add(TotalPoints);
        hashCode.Add(PointsAwards);
        hashCode.Add(Broadcasts);
        hashCode.Add(Contests);
        hashCode.Add(VotingCountries);

        return hashCode.ToHashCode();
    }
}

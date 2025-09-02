namespace Eurocentric.Domain.V0.Rankings.CompetingCountries;

public sealed record PointsInRangeRanking
{
    public int Rank { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public decimal PointsInRange { get; init; }

    public int PointsAwardsInRange { get; init; }

    public int PointsAwards { get; init; }

    public int Broadcasts { get; init; }

    public int Contests { get; init; }

    public int VotingCountries { get; init; }
}

namespace Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;

public sealed record VotingCountryPointsAverageRanking
{
    public int Rank { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public decimal PointsAverage { get; init; }

    public int TotalPoints { get; init; }

    public int PointsAwards { get; init; }

    public int Broadcasts { get; init; }

    public int Contests { get; init; }
}

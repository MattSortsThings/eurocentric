namespace Eurocentric.Domain.Queries.VotingCountryRankings;

public sealed record VotingCountryPointsShareRanking
{
    public required int Rank { get; init; }

    public required string CountryCode { get; init; }

    public required double PointsShare { get; init; }

    public required int TotalPoints { get; init; }

    public required int PossiblePoints { get; init; }

    public required string CountryName { get; init; }
}

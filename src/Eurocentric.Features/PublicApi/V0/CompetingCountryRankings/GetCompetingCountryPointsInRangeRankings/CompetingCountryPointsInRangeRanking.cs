namespace Eurocentric.Features.PublicApi.V0.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings;

public sealed record CompetingCountryPointsInRangeRanking
{
    public required int Rank { get; init; }

    public required string CountryCode { get; init; } = string.Empty;

    public required string CountryName { get; init; } = string.Empty;

    public required decimal PointsInRange { get; init; }

    public required int PointsAwardsInRange { get; init; }

    public required int PointsAwards { get; init; }

    public required int Broadcasts { get; init; }

    public required int Contests { get; init; }

    public required int VotingCountries { get; init; }
}

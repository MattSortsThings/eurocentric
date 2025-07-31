namespace Eurocentric.Features.PublicApi.V0.Rankings.GetCompetingCountryPointsAverageRankings;

public sealed record CompetingCountryPointsAverageRanking
{
    public required int Rank { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required double PointsAverage { get; init; }

    public required int PointsAwards { get; init; }

    public required int Broadcasts { get; init; }

    public required int Contests { get; init; }

    public required int VotingCountries { get; init; }
}

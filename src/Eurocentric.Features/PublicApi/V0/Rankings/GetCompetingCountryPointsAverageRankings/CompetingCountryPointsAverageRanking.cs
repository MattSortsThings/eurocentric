namespace Eurocentric.Features.PublicApi.V0.Rankings.GetCompetingCountryPointsAverageRankings;

public sealed record CompetingCountryPointsAverageRanking
{
    public int Rank { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public double PointsAverage { get; init; }

    public int PointsAwards { get; init; }

    public int Broadcasts { get; init; }

    public int Contests { get; init; }

    public int VotingCountries { get; init; }
}

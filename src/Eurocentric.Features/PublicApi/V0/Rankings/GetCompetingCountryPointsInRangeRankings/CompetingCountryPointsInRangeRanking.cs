namespace Eurocentric.Features.PublicApi.V0.Rankings.GetCompetingCountryPointsInRangeRankings;

public sealed record CompetingCountryPointsInRangeRanking
{
    public int Rank { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public double PointsInRange { get; init; }

    public int PointsAwards { get; init; }

    public int Broadcasts { get; init; }

    public int Contests { get; init; }

    public int VotingCountries { get; init; }
}

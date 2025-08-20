using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsInRangeRankings;

public sealed record CompetingCountryPointsInRangeRanking : IExampleProvider<CompetingCountryPointsInRangeRanking>
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

    public static CompetingCountryPointsInRangeRanking CreateExample() => new()
    {
        Rank = 1,
        CountryCode = "AT",
        CountryName = "Austria",
        PointsInRange = 0.8m,
        PointsAwardsInRange = 200,
        PointsAwards = 250,
        Broadcasts = 5,
        Contests = 8,
        VotingCountries = 40
    };
}

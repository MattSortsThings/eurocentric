using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings;

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
        PointsInRange = 0.75m,
        PointsAwardsInRange = 300,
        PointsAwards = 400,
        Broadcasts = 10,
        Contests = 5,
        VotingCountries = 40
    };
}

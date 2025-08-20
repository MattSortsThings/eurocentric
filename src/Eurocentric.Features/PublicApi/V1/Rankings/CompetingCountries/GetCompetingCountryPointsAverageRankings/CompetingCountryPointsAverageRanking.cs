using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsAverageRankings;

public sealed record CompetingCountryPointsAverageRanking : IExampleProvider<CompetingCountryPointsAverageRanking>
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

    public static CompetingCountryPointsAverageRanking CreateExample() => new()
    {
        Rank = 1,
        CountryCode = "AT",
        CountryName = "Austria",
        PointsAverage = 6.25m,
        TotalPoints = 1625,
        PointsAwards = 250,
        Broadcasts = 5,
        Contests = 8,
        VotingCountries = 40
    };
}

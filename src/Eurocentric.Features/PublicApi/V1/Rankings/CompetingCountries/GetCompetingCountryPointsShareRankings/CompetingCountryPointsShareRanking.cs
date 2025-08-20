using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsShareRankings;

public sealed record CompetingCountryPointsShareRanking : IExampleProvider<CompetingCountryPointsShareRanking>
{
    public int Rank { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public decimal PointsShare { get; init; }

    public int TotalPoints { get; init; }

    public int AvailablePoints { get; init; }

    public int PointsAwards { get; init; }

    public int Broadcasts { get; init; }

    public int Contests { get; init; }

    public int VotingCountries { get; init; }

    public static CompetingCountryPointsShareRanking CreateExample() => new()
    {
        Rank = 1,
        CountryCode = "AT",
        CountryName = "Austria",
        PointsShare = 0.541667m,
        TotalPoints = 1625,
        AvailablePoints = 3000,
        PointsAwards = 250,
        Broadcasts = 5,
        Contests = 8,
        VotingCountries = 40
    };
}

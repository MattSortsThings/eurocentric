using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsAverageRankings;

public sealed record VotingCountryPointsAverageRanking : IExampleProvider<VotingCountryPointsAverageRanking>
{
    public int Rank { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public decimal PointsAverage { get; init; }

    public int TotalPoints { get; init; }

    public int PointsAwards { get; init; }

    public int Broadcasts { get; init; }

    public int Contests { get; init; }

    public static VotingCountryPointsAverageRanking CreateExample() => new()
    {
        Rank = 1,
        CountryCode = "AT",
        CountryName = "Austria",
        PointsAverage = 6.25m,
        TotalPoints = 50,
        PointsAwards = 8,
        Broadcasts = 5,
        Contests = 8
    };
}

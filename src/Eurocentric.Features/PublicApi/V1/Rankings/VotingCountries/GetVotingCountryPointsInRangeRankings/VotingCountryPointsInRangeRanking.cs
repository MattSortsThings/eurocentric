using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsInRangeRankings;

public sealed record VotingCountryPointsInRangeRanking : IExampleProvider<VotingCountryPointsInRangeRanking>
{
    public int Rank { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public decimal PointsInRange { get; init; }

    public int PointsAwardsInRange { get; init; }

    public int PointsAwards { get; init; }

    public int Broadcasts { get; init; }

    public int Contests { get; init; }

    public static VotingCountryPointsInRangeRanking CreateExample() => new()
    {
        Rank = 1,
        CountryCode = "AT",
        CountryName = "Austria",
        PointsInRange = 0.8m,
        PointsAwardsInRange = 8,
        PointsAwards = 10,
        Broadcasts = 5,
        Contests = 8
    };
}

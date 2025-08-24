using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsShareRankings;

public sealed record VotingCountryPointsShareRanking : IExampleProvider<VotingCountryPointsShareRanking>
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

    public static VotingCountryPointsShareRanking CreateExample() => new()
    {
        Rank = 1,
        CountryCode = "AT",
        CountryName = "Austria",
        PointsShare = 0.5m,
        TotalPoints = 60,
        AvailablePoints = 120,
        PointsAwards = 10,
        Broadcasts = 5,
        Contests = 8
    };
}

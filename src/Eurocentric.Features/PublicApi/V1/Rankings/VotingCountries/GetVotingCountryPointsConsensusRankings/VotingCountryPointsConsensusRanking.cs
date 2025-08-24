using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsConsensusRankings;

public sealed record VotingCountryPointsConsensusRanking : IExampleProvider<VotingCountryPointsConsensusRanking>
{
    public int Rank { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public decimal PointsConsensus { get; init; }

    public decimal VectorDotProduct { get; init; }

    public decimal JuryVectorLength { get; init; }

    public decimal TelevoteVectorLength { get; init; }

    public int PointsAwardPairs { get; init; }

    public int Broadcasts { get; init; }

    public int Contests { get; init; }

    public int CompetingCountries { get; init; }

    public static VotingCountryPointsConsensusRanking CreateExample() => new()
    {
        Rank = 1,
        CountryCode = "AT",
        CountryName = "Austria",
        PointsConsensus = 0.125m,
        VectorDotProduct = 5000.0m,
        JuryVectorLength = 400.0m,
        TelevoteVectorLength = 100.0m,
        PointsAwardPairs = 200,
        Broadcasts = 5,
        Contests = 8,
        CompetingCountries = 40
    };
}

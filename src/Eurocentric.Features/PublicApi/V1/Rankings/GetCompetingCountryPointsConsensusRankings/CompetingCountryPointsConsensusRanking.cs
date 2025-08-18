using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsConsensusRankings;

public sealed record CompetingCountryPointsConsensusRanking : IExampleProvider<CompetingCountryPointsConsensusRanking>
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

    public int VotingCountries { get; init; }

    public static CompetingCountryPointsConsensusRanking CreateExample() => new()
    {
        Rank = 1,
        CountryCode = "AT",
        CountryName = "Austria",
        PointsConsensus = 0.125m,
        VectorDotProduct = 5000.0m,
        JuryVectorLength = 400.0m,
        TelevoteVectorLength = 100.0m,
        PointsAwardPairs = 125,
        Broadcasts = 5,
        Contests = 8,
        VotingCountries = 40
    };
}

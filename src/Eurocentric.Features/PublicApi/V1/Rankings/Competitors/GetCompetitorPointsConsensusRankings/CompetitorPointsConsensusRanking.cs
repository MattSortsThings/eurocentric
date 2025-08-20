using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsConsensusRankings;

public sealed record CompetitorPointsConsensusRanking : IExampleProvider<CompetitorPointsConsensusRanking>
{
    public int Rank { get; init; }

    public int ContestYear { get; init; }

    public ContestStage ContestStage { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public int RunningOrderPosition { get; init; }

    public int FinishingPosition { get; init; }

    public string ActName { get; init; } = string.Empty;

    public string SongTitle { get; init; } = string.Empty;

    public decimal PointsConsensus { get; init; }

    public decimal VectorDotProduct { get; init; }

    public decimal JuryVectorLength { get; init; }

    public decimal TelevoteVectorLength { get; init; }

    public int PointsAwardPairs { get; init; }

    public static CompetitorPointsConsensusRanking CreateExample() => new()
    {
        Rank = 1,
        ContestYear = 2025,
        ContestStage = ContestStage.GrandFinal,
        CountryCode = "AT",
        CountryName = "Austria",
        RunningOrderPosition = 9,
        FinishingPosition = 1,
        ActName = "JJ",
        SongTitle = "Wasted Love",
        PointsConsensus = 0.125m,
        VectorDotProduct = 5000.0m,
        JuryVectorLength = 400.0m,
        TelevoteVectorLength = 100.0m,
        PointsAwardPairs = 35
    };
}

using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetitorPointsAverageRankings;

public sealed record CompetitorPointsAverageRanking : IExampleProvider<CompetitorPointsAverageRanking>
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

    public decimal PointsAverage { get; init; }

    public int TotalPoints { get; init; }

    public int PointsAwards { get; init; }

    public static CompetitorPointsAverageRanking CreateExample() => new()
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
        PointsAverage = 4.55m,
        TotalPoints = 315,
        PointsAwards = 70
    };
}

using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetitorPointsShareRankings;

public sealed record CompetitorPointsShareRanking : IExampleProvider<CompetitorPointsShareRanking>
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

    public decimal PointsShare { get; init; }

    public int TotalPoints { get; init; }

    public int AvailablePoints { get; init; }

    public int PointsAwards { get; init; }


    public static CompetitorPointsShareRanking CreateExample() => new()
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
        PointsShare = 0.375m,
        TotalPoints = 315,
        AvailablePoints = 840,
        PointsAwards = 70
    };
}

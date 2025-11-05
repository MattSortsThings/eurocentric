using Eurocentric.Apis.Public.V1.Enums;

namespace Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;

public sealed record CompetitorPointsAverageRanking
{
    public int Rank { get; init; }

    public int ContestYear { get; init; }

    public ContestStage ContestStage { get; init; }

    public int RunningOrderSpot { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public int FinishingPosition { get; init; }

    public string ActName { get; init; } = string.Empty;

    public string SongTitle { get; init; } = string.Empty;

    public decimal PointsAverage { get; init; }

    public int TotalPoints { get; init; }

    public int PointsAwards { get; init; }

    public int VotingCountries { get; init; }
}

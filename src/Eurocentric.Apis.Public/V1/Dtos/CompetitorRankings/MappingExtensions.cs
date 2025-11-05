using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Domain.Analytics.Rankings.Competitors;

namespace Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;

internal static class MappingExtensions
{
    internal static CompetitorPointsAverageMetadata ToDto(this PointsAverageMetadata row)
    {
        return new CompetitorPointsAverageMetadata
        {
            MinYear = row.MinYear,
            MaxYear = row.MaxYear,
            ContestStage = row.ContestStage?.ToApiContestStageFilter(),
            VotingMethod = row.VotingMethod?.ToApiVotingMethodFilter(),
            PageIndex = row.PageIndex,
            PageSize = row.PageSize,
            Descending = row.Descending,
            TotalItems = row.TotalItems,
            TotalPages = row.TotalPages,
        };
    }

    internal static CompetitorPointsAverageRanking ToDto(this PointsAverageRanking row)
    {
        return new CompetitorPointsAverageRanking
        {
            Rank = row.Rank,
            ContestYear = row.ContestYear,
            ContestStage = row.ContestStage.ToApiContestStage(),
            RunningOrderSpot = row.RunningOrderSpot,
            CountryCode = row.CountryCode,
            CountryName = row.CountryName,
            FinishingPosition = row.FinishingPosition,
            ActName = row.ActName,
            SongTitle = row.SongTitle,
            PointsAverage = row.PointsAverage,
            TotalPoints = row.TotalPoints,
            PointsAwards = row.PointsAwards,
            VotingCountries = row.VotingCountries,
        };
    }
}

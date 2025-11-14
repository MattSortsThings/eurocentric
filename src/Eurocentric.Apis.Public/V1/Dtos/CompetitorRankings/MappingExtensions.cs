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
            CompetingCountryCode = row.CompetingCountryCode,
            VotingMethod = row.VotingMethod?.ToApiVotingMethodFilter(),
            PageIndex = row.PageIndex,
            PageSize = row.PageSize,
            Descending = row.Descending,
            TotalItems = row.TotalItems,
            TotalPages = row.TotalPages,
        };
    }

    internal static CompetitorPointsConsensusMetadata ToDto(this PointsConsensusMetadata row)
    {
        return new CompetitorPointsConsensusMetadata
        {
            MinYear = row.MinYear,
            MaxYear = row.MaxYear,
            ContestStage = row.ContestStage?.ToApiContestStageFilter(),
            CompetingCountryCode = row.CompetingCountryCode,
            PageIndex = row.PageIndex,
            PageSize = row.PageSize,
            Descending = row.Descending,
            TotalItems = row.TotalItems,
            TotalPages = row.TotalPages,
        };
    }

    internal static CompetitorPointsInRangeMetadata ToDto(this PointsInRangeMetadata row)
    {
        return new CompetitorPointsInRangeMetadata
        {
            MinPoints = row.MinPoints,
            MaxPoints = row.MaxPoints,
            MinYear = row.MinYear,
            MaxYear = row.MaxYear,
            ContestStage = row.ContestStage?.ToApiContestStageFilter(),
            CompetingCountryCode = row.CompetingCountryCode,
            VotingMethod = row.VotingMethod?.ToApiVotingMethodFilter(),
            PageIndex = row.PageIndex,
            PageSize = row.PageSize,
            Descending = row.Descending,
            TotalItems = row.TotalItems,
            TotalPages = row.TotalPages,
        };
    }

    internal static CompetitorPointsShareMetadata ToDto(this PointsShareMetadata row)
    {
        return new CompetitorPointsShareMetadata
        {
            MinYear = row.MinYear,
            MaxYear = row.MaxYear,
            ContestStage = row.ContestStage?.ToApiContestStageFilter(),
            CompetingCountryCode = row.CompetingCountryCode,
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

    internal static CompetitorPointsConsensusRanking ToDto(this PointsConsensusRanking row)
    {
        return new CompetitorPointsConsensusRanking
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
            PointsConsensus = row.PointsConsensus,
            VectorDimensions = row.VectorDimensions,
            JuryVectorLength = row.JuryVectorLength,
            TelevoteVectorLength = row.TelevoteVectorLength,
            VectorDotProduct = row.VectorDotProduct,
        };
    }

    internal static CompetitorPointsInRangeRanking ToDto(this PointsInRangeRanking row)
    {
        return new CompetitorPointsInRangeRanking
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
            PointsInRange = row.PointsInRange,
            PointsAwardsInRange = row.PointsAwardsInRange,
            PointsAwards = row.PointsAwards,
            VotingCountries = row.VotingCountries,
        };
    }

    internal static CompetitorPointsShareRanking ToDto(this PointsShareRanking row)
    {
        return new CompetitorPointsShareRanking
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
            PointsShare = row.PointsShare,
            TotalPoints = row.TotalPoints,
            AvailablePoints = row.AvailablePoints,
            PointsAwards = row.PointsAwards,
            VotingCountries = row.VotingCountries,
        };
    }
}

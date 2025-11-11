using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Domain.Analytics.Rankings.CompetingCountries;

namespace Eurocentric.Apis.Public.V1.Dtos.CompetingCountryRankings;

internal static class MappingExtensions
{
    internal static CompetingCountryPointsAverageMetadata ToDto(this PointsAverageMetadata row)
    {
        return new CompetingCountryPointsAverageMetadata
        {
            MinYear = row.MinYear,
            MaxYear = row.MaxYear,
            ContestStage = row.ContestStage?.ToApiContestStageFilter(),
            VotingCountryCode = row.VotingCountryCode,
            VotingMethod = row.VotingMethod?.ToApiVotingMethodFilter(),
            PageIndex = row.PageIndex,
            PageSize = row.PageSize,
            Descending = row.Descending,
            TotalItems = row.TotalItems,
            TotalPages = row.TotalPages,
        };
    }

    internal static CompetingCountryPointsConsensusMetadata ToDto(this PointsConsensusMetadata row)
    {
        return new CompetingCountryPointsConsensusMetadata
        {
            MinYear = row.MinYear,
            MaxYear = row.MaxYear,
            ContestStage = row.ContestStage?.ToApiContestStageFilter(),
            VotingCountryCode = row.VotingCountryCode,
            PageIndex = row.PageIndex,
            PageSize = row.PageSize,
            Descending = row.Descending,
            TotalItems = row.TotalItems,
            TotalPages = row.TotalPages,
        };
    }

    internal static CompetingCountryPointsShareMetadata ToDto(this PointsShareMetadata row)
    {
        return new CompetingCountryPointsShareMetadata
        {
            MinYear = row.MinYear,
            MaxYear = row.MaxYear,
            ContestStage = row.ContestStage?.ToApiContestStageFilter(),
            VotingCountryCode = row.VotingCountryCode,
            VotingMethod = row.VotingMethod?.ToApiVotingMethodFilter(),
            PageIndex = row.PageIndex,
            PageSize = row.PageSize,
            Descending = row.Descending,
            TotalItems = row.TotalItems,
            TotalPages = row.TotalPages,
        };
    }

    internal static CompetingCountryPointsAverageRanking ToDto(this PointsAverageRanking row)
    {
        return new CompetingCountryPointsAverageRanking
        {
            Rank = row.Rank,
            CountryCode = row.CountryCode,
            CountryName = row.CountryName,
            PointsAverage = row.PointsAverage,
            TotalPoints = row.TotalPoints,
            PointsAwards = row.PointsAwards,
            Broadcasts = row.Broadcasts,
            Contests = row.Contests,
            VotingCountries = row.VotingCountries,
        };
    }

    internal static CompetingCountryPointsConsensusRanking ToDto(this PointsConsensusRanking row)
    {
        return new CompetingCountryPointsConsensusRanking
        {
            Rank = row.Rank,
            CountryCode = row.CountryCode,
            CountryName = row.CountryName,
            PointsConsensus = row.PointsConsensus,
            VectorDimensions = row.VectorDimensions,
            JuryVectorLength = row.JuryVectorLength,
            TelevoteVectorLength = row.TelevoteVectorLength,
            VectorDotProduct = row.VectorDotProduct,
            Broadcasts = row.Broadcasts,
            Contests = row.Contests,
            VotingCountries = row.VotingCountries,
        };
    }

    internal static CompetingCountryPointsShareRanking ToDto(this PointsShareRanking row)
    {
        return new CompetingCountryPointsShareRanking
        {
            Rank = row.Rank,
            CountryCode = row.CountryCode,
            CountryName = row.CountryName,
            PointsShare = row.PointsShare,
            TotalPoints = row.TotalPoints,
            AvailablePoints = row.AvailablePoints,
            PointsAwards = row.PointsAwards,
            Broadcasts = row.Broadcasts,
            Contests = row.Contests,
            VotingCountries = row.VotingCountries,
        };
    }
}

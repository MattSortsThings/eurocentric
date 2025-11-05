using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Domain.Analytics.Rankings.VotingCountries;

namespace Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;

internal static class MappingExtensions
{
    internal static VotingCountryPointsAverageMetadata ToDto(this PointsAverageMetadata row)
    {
        return new VotingCountryPointsAverageMetadata
        {
            CompetingCountryCode = row.CompetingCountryCode,
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

    internal static VotingCountryPointsAverageRanking ToDto(this PointsAverageRanking row)
    {
        return new VotingCountryPointsAverageRanking
        {
            Rank = row.Rank,
            CountryCode = row.CountryCode,
            CountryName = row.CountryName,
            PointsAverage = row.PointsAverage,
            TotalPoints = row.TotalPoints,
            PointsAwards = row.PointsAwards,
            Broadcasts = row.Broadcasts,
            Contests = row.Contests,
        };
    }
}

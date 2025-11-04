using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Domain.Analytics.Rankings.CompetingCountries;

namespace Eurocentric.Apis.Public.V1.Dtos.Rankings.CompetingCountries;

internal static class MappingExtensions
{
    internal static CompetingCountryPointsAverageMetadata ToDto(this PointsAverageMetadata record)
    {
        return new CompetingCountryPointsAverageMetadata
        {
            MinYear = record.MinYear,
            MaxYear = record.MaxYear,
            ContestStage = record.ContestStage?.ToApiContestStageFilter(),
            VotingCountryCode = record.VotingCountryCode,
            VotingMethod = record.VotingMethod?.ToApiVotingMethodFilter(),
            PageIndex = record.PageIndex,
            PageSize = record.PageSize,
            Descending = record.Descending,
            TotalItems = record.TotalItems,
            TotalPages = record.TotalPages,
        };
    }

    internal static CompetingCountryPointsAverageRanking ToDto(this PointsAverageRanking record)
    {
        return new CompetingCountryPointsAverageRanking
        {
            Rank = record.Rank,
            CountryCode = record.CountryCode,
            CountryName = record.CountryName,
            PointsAverage = record.PointsAverage,
            TotalPoints = record.TotalPoints,
            PointsAwards = record.PointsAwards,
            Broadcasts = record.Broadcasts,
            Contests = record.Contests,
            VotingCountries = record.VotingCountries,
        };
    }
}

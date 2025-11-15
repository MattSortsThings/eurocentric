using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Domain.Analytics.Listings;
using CompetingCountryPointsMetadataDto = Eurocentric.Apis.Public.V1.Dtos.Listings.CompetingCountryPointsMetadata;
using CompetingCountryPointsMetadataRow = Eurocentric.Domain.Analytics.Listings.CompetingCountryPointsMetadata;

namespace Eurocentric.Apis.Public.V1.Dtos.Listings;

internal static class MappingExtensions
{
    internal static CompetingCountryPointsMetadataDto ToDto(this CompetingCountryPointsMetadataRow row)
    {
        return new CompetingCountryPointsMetadataDto
        {
            ContestYear = row.ContestYear,
            ContestStage = row.ContestStage.ToApiContestStage(),
            CompetingCountryCode = row.CompetingCountryCode,
        };
    }

    extension(CompetingCountryPointsListing row)
    {
        internal CompetingCountryJuryPointsListing ToJuryPointsDto()
        {
            return new CompetingCountryJuryPointsListing
            {
                PointsValue = row.PointsValue,
                VotingCountryCode = row.VotingCountryCode,
                VotingCountryName = row.VotingCountryName,
            };
        }

        internal CompetingCountryTelevotePointsListing ToTelevotePointsDto()
        {
            return new CompetingCountryTelevotePointsListing
            {
                PointsValue = row.PointsValue,
                VotingCountryCode = row.VotingCountryCode,
                VotingCountryName = row.VotingCountryName,
            };
        }
    }
}

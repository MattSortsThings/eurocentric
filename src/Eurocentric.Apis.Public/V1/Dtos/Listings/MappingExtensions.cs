using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Domain.Analytics.Listings;
using CompetingCountryPointsMetadataDto = Eurocentric.Apis.Public.V1.Dtos.Listings.CompetingCountryPointsMetadata;
using CompetingCountryPointsMetadataRow = Eurocentric.Domain.Analytics.Listings.CompetingCountryPointsMetadata;
using VotingCountryPointsMetadataDto = Eurocentric.Apis.Public.V1.Dtos.Listings.VotingCountryPointsMetadata;
using VotingCountryPointsMetadataRow = Eurocentric.Domain.Analytics.Listings.VotingCountryPointsMetadata;

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

    internal static VotingCountryPointsMetadataDto ToDto(this VotingCountryPointsMetadataRow row)
    {
        return new VotingCountryPointsMetadataDto
        {
            ContestYear = row.ContestYear,
            ContestStage = row.ContestStage.ToApiContestStage(),
            VotingCountryCode = row.VotingCountryCode,
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

    extension(VotingCountryPointsListing row)
    {
        internal VotingCountryJuryPointsListing ToJuryPointsDto()
        {
            return new VotingCountryJuryPointsListing
            {
                PointsValue = row.PointsValue,
                CompetingCountryCode = row.CompetingCountryCode,
                CompetingCountryName = row.CompetingCountryName,
            };
        }

        internal VotingCountryTelevotePointsListing ToTelevotePointsDto()
        {
            return new VotingCountryTelevotePointsListing
            {
                PointsValue = row.PointsValue,
                CompetingCountryCode = row.CompetingCountryCode,
                CompetingCountryName = row.CompetingCountryName,
            };
        }
    }
}

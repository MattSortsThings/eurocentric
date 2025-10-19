using Eurocentric.Apis.Public.V0.Enums;
using BroadcastResultListingDto = Eurocentric.Apis.Public.V0.Dtos.Listings.BroadcastResultListing;
using BroadcastResultListingRecord = Eurocentric.Domain.V0.Queries.Listings.BroadcastResultListing;
using BroadcastResultMetadataDto = Eurocentric.Apis.Public.V0.Dtos.Listings.BroadcastResultMetadata;
using BroadcastResultMetadataRecord = Eurocentric.Domain.V0.Queries.Listings.BroadcastResultMetadata;

namespace Eurocentric.Apis.Public.V0.Dtos.Listings;

internal static class MappingExtensions
{
    internal static BroadcastResultListingDto ToDto(this BroadcastResultListingRecord record)
    {
        return new BroadcastResultListingDto
        {
            RunningOrderSpot = record.RunningOrderSpot,
            CountryCode = record.CountryCode,
            CountryName = record.CountryName,
            ActName = record.ActName,
            SongTitle = record.SongTitle,
            JuryPoints = record.JuryPoints,
            TelevotePoints = record.TelevotePoints,
            OverallPoints = record.OverallPoints,
            JuryRank = record.JuryRank,
            TelevoteRank = record.TelevoteRank,
            FinishingPosition = record.FinishingPosition,
        };
    }

    internal static BroadcastResultMetadataDto ToDto(this BroadcastResultMetadataRecord record) =>
        new() { ContestYear = record.ContestYear, ContestStage = record.ContestStage.ToApiContestStage() };
}

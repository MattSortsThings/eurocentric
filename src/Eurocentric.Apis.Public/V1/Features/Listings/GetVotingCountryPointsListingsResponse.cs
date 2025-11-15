using Eurocentric.Apis.Public.V1.Dtos.Listings;

namespace Eurocentric.Apis.Public.V1.Features.Listings;

public sealed record GetVotingCountryPointsListingsResponse(
    VotingCountryJuryPointsListing[] JuryPointsListings,
    VotingCountryTelevotePointsListing[] TelevotePointsListings,
    VotingCountryPointsMetadata Metadata
);

using Eurocentric.Apis.Public.V1.Dtos.Listings;

namespace Eurocentric.Apis.Public.V1.Features.Listings;

public sealed record GetCompetingCountryResultListingsResponse(
    CompetingCountryResultListing[] Listings,
    CompetingCountryResultMetadata Metadata
);

using Eurocentric.Apis.Public.V0.Dtos.Listings;

namespace Eurocentric.Apis.Public.V0.Features.Listings;

public sealed record GetBroadcastResultListingsResponse(
    BroadcastResultListing[] Listings,
    BroadcastResultMetadata Metadata
);

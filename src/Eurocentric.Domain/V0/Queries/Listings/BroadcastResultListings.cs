namespace Eurocentric.Domain.V0.Queries.Listings;

public readonly record struct BroadcastResultListings(
    List<BroadcastResultListing> Listings,
    BroadcastResultMetadata Metadata
);

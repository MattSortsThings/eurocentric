namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     The result of a broadcast result listings query.
/// </summary>
/// <param name="ResultListings">The full result listings.</param>
/// <param name="Metadata">The query metadata.</param>
public readonly record struct BroadcastResultListings(
    List<BroadcastResultListing> ResultListings,
    BroadcastResultMetadata Metadata
);

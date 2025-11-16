namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     The result of a competing country result listings query.
/// </summary>
/// <param name="ResultListings">The full result listings.</param>
/// <param name="Metadata">The query metadata.</param>
public readonly record struct CompetingCountryResultListings(
    List<CompetingCountryResultListing> ResultListings,
    CompetingCountryResultMetadata Metadata
);

namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     The result of a competing country points listings query.
/// </summary>
/// <param name="JuryPointsListings">The full jury points listings.</param>
/// <param name="TelevotePointsListings">The full televote points listings.</param>
/// <param name="Metadata">The query metadata.</param>
public readonly record struct CompetingCountryPointsListings(
    List<CompetingCountryPointsListing> JuryPointsListings,
    List<CompetingCountryPointsListing> TelevotePointsListings,
    CompetingCountryPointsMetadata Metadata
);

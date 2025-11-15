namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     The result of a voting country points listings query.
/// </summary>
/// <param name="JuryPointsListings">The full jury points listings.</param>
/// <param name="TelevotePointsListings">The full televote points listings.</param>
/// <param name="Metadata">The query metadata.</param>
public readonly record struct VotingCountryPointsListings(
    List<VotingCountryPointsListing> JuryPointsListings,
    List<VotingCountryPointsListing> TelevotePointsListings,
    VotingCountryPointsMetadata Metadata
);

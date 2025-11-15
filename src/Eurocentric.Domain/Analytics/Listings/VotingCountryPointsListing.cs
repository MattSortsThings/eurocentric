namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     A single voting country points listings row.
/// </summary>
public sealed record VotingCountryPointsListing
{
    /// <summary>
    ///     The value of the points award the voting country gave to the competing country.
    /// </summary>
    public int PointsValue { get; init; }

    /// <summary>
    ///     The competing country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CompetingCountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The competing country's short UK English name.
    /// </summary>
    public string CompetingCountryName { get; init; } = string.Empty;
}

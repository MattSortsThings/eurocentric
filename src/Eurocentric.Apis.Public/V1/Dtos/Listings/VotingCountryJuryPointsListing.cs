using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.Listings;

/// <summary>
///     A single voting country jury points listings row.
/// </summary>
public sealed record VotingCountryJuryPointsListing : IDtoSchemaExampleProvider<VotingCountryJuryPointsListing>
{
    /// <summary>
    ///     The value of the jury points award the voting country gave to the competing country.
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

    public static VotingCountryJuryPointsListing CreateExample() =>
        new()
        {
            PointsValue = 10,
            CompetingCountryCode = "AA",
            CompetingCountryName = "CountryName",
        };
}

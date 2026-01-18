namespace Eurocentric.Apis.Public.V0.Common.Dtos.Countries;

/// <summary>
///     Represents a country.
/// </summary>
public sealed record Country
{
    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; }
}

using Eurocentric.Apis.Admin.V0.Dtos.Countries;

namespace Eurocentric.Apis.Admin.V0.Contracts.Countries;

public sealed record CreateCountryRequest
{
    /// <summary>
    ///     The country's type.
    /// </summary>
    public required CountryType CountryType { get; init; }

    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; } = string.Empty;
}

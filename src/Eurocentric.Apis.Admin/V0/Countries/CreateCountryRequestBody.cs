using Eurocentric.Apis.Admin.V0.Common.Enums;

namespace Eurocentric.Apis.Admin.V0.Countries;

public sealed record CreateCountryRequestBody
{
    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; }

    /// <summary>
    ///     The country's type.
    /// </summary>
    public required CountryType CountryType { get; init; }
}

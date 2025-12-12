using Eurocentric.Apis.Admin.V0.Common.Config;
using Eurocentric.Apis.Admin.V0.Common.Enums;

namespace Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;

/// <summary>
///     Request body for the <see cref="EndpointIds.Countries.CreateCountry" /> endpoint.
/// </summary>
public sealed record CreateCountryRequest
{
    /// <summary>
    ///     The type of country to be created.
    /// </summary>
    public required CountryType CountryType { get; init; }

    /// <summary>
    ///     The country's ISO 3166 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;
}

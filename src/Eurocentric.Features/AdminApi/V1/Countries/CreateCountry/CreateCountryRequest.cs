using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;

public sealed record CreateCountryRequest : IExampleProvider<CreateCountryRequest>
{
    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; } = string.Empty;

    public static CreateCountryRequest CreateExample() => new()
    {
        CountryCode = ExampleValues.CountryCode, CountryName = ExampleValues.CountryName
    };
}

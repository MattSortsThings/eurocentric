using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Countries;

public sealed record CreateCountryRequest : ISchemaExampleProvider<CreateCountryRequest>
{
    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; }

    public static CreateCountryRequest CreateExample() => new() { CountryCode = "AT", CountryName = "Austria" };
}

using Eurocentric.Apis.Admin.V0.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

public sealed record CreateCountryRequest : IDtoSchemaExampleProvider<CreateCountryRequest>
{
    public required CountryType CountryType { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public static CreateCountryRequest CreateExample() =>
        new()
        {
            CountryType = CountryType.Real,
            CountryCode = "AT",
            CountryName = "Austria",
        };
}

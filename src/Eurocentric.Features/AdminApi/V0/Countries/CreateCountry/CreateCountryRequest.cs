using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V0.Countries.CreateCountry;

public sealed record CreateCountryRequest : IExampleProvider<CreateCountryRequest>
{
    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required CountryType CountryType { get; init; }

    public static CreateCountryRequest CreateExample() => new()
    {
        CountryCode = "AT", CountryName = "Austria", CountryType = CountryType.Real
    };
}

using Eurocentric.Apis.Admin.V0.Enums;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

public sealed record CreateCountryRequest
{
    public required CountryType CountryType { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }
}

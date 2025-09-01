using Eurocentric.Features.AdminApi.V0.Common.Enums;

namespace Eurocentric.Features.AdminApi.V0.Countries.CreateCountry;

public sealed record CreateCountryRequest
{
    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required CountryType CountryType { get; init; }
}

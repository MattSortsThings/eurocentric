namespace Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableCountries;

public sealed record QueryableCountry
{
    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }
}

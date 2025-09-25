namespace Eurocentric.Apis.Public.V0.Contracts.Queryables.Dtos;

public sealed record QueryableCountry
{
    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;
}

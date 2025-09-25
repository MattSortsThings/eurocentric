namespace Eurocentric.Apis.Public.V0.Dtos.Queryables;

public sealed record QueryableCountry
{
    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;
}

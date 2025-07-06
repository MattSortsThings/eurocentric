namespace Eurocentric.Domain.PlaceholderEntities;

public sealed record QueryableCountry
{
    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }
}

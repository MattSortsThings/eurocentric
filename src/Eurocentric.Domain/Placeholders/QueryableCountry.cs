namespace Eurocentric.Domain.Placeholders;

public sealed record QueryableCountry
{
    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;
}

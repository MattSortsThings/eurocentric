namespace Eurocentric.Domain.V0.Views;

public sealed record QueryableCountry
{
    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;
}

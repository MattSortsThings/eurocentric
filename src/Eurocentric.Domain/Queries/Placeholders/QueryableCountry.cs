namespace Eurocentric.Domain.Queries.Placeholders;

/// <summary>
///     Represents a queryable country.
/// </summary>
public sealed record QueryableCountry
{
    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; }
}

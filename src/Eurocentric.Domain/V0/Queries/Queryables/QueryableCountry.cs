namespace Eurocentric.Domain.V0.Queries.Queryables;

/// <summary>
///     A queryable country.
/// </summary>
/// <remarks>A country is queryable if it has a role in one or more queryable contests.</remarks>
public sealed record QueryableCountry
{
    /// <summary>
    ///     Gets the country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;
}

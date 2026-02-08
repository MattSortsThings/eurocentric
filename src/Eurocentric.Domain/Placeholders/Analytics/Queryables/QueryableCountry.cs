namespace Eurocentric.Domain.Placeholders.Analytics.Queryables;

public sealed record QueryableCountry
{
    /// <summary>
    ///     Gets or initializes the country's ISO 3166 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    ///     Gets or initializes the country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; }

    /// <summary>
    ///     Gets or initializes an ordered list of the queryable contest years in which the country is active.
    /// </summary>
    public required List<int> ActiveContestYears { get; init; }
}

namespace Eurocentric.Apis.Public.V0.Common.Models.Queryables;

public sealed record QueryableCountry
{
    /// <summary>
    ///     The country's ISO 3166 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; }

    /// <summary>
    ///     An ordered array of the queryable contest years in which the country is active.
    /// </summary>
    public required int[] ActiveContestYears { get; init; }
}

using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0.Countries;

/// <summary>
///     Represents a country.
/// </summary>
public sealed class Country
{
    /// <summary>
    ///     Gets or initializes the aggregate's system identifier.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    ///     Gets or initializes the country's ISO 3166 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    ///     Gets or initializes the country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; }

    /// <summary>
    ///     Gets or initializes the country's type.
    /// </summary>
    public required CountryType CountryType { get; init; }

    /// <summary>
    ///     Gets or initializes an unordered list of the country's active contest IDs.
    /// </summary>
    public required List<Guid> ActiveContestIds { get; init; }
}

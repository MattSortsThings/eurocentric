namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a country or pseudo-country.
/// </summary>
public sealed record Country
{
    /// <summary>
    ///     Gets the country's ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Gets the country's ISO 3166 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets a list of all the country's contest roles.
    /// </summary>
    public List<ContestRole> ContestRoles { get; init; } = [];
}

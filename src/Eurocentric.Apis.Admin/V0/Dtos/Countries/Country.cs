namespace Eurocentric.Apis.Admin.V0.Dtos.Countries;

/// <summary>
///     Represents a country or pseudo-country.
/// </summary>
public sealed record Country
{
    /// <summary>
    ///     The country's ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     An array of all the country's contest roles.
    /// </summary>
    public ContestRole[] ContestRoles { get; init; } = [];
}

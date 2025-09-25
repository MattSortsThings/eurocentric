namespace Eurocentric.Apis.Admin.V0.Dtos.Countries;

/// <summary>
///     Represents a country aggregate.
/// </summary>
public sealed record Country
{
    /// <summary>
    ///     Gets the aggregate's system ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Gets the country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets a list of all the country's roles in contests.
    /// </summary>
    public ContestRole[] ContestRoles { get; init; } = [];
}

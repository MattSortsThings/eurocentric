using Eurocentric.Apis.Admin.V0.Common.Enums;

namespace Eurocentric.Apis.Admin.V0.Common.Dtos.Countries;

/// <summary>
///     Represents a country.
/// </summary>
public sealed record Country
{
    /// <summary>
    ///     The country's ID.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; }

    /// <summary>
    ///     The country's type.
    /// </summary>
    public required CountryType CountryType { get; init; }

    /// <summary>
    ///     An unordered array of the country's contest roles.
    /// </summary>
    public required ContestRole[] ContestRoles { get; init; }
}

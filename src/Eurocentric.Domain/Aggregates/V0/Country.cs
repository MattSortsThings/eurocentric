using CSharpFunctionalExtensions;
using Eurocentric.Domain.Abstractions.Errors;

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
    ///     Gets an unordered list of the country's contest roles.
    /// </summary>
    public List<ContestRole> ContestRoles { get; init; } = [];

    public static Result<Country, IDomainError> Create(string countryCode, string countryName)
    {
        return LegalCountryCodeValue(countryCode)
            ? new Country
            {
                Id = Guid.NewGuid(),
                CountryCode = countryCode,
                CountryName = countryName,
            }
            : CountryErrors.IllegalCountryCodeValue(countryCode);
    }

    private static bool LegalCountryCodeValue(string value) => value.Length == 2 && value.All(char.IsAsciiLetterUpper);
}

using CSharpFunctionalExtensions;
using Eurocentric.Domain.Functional;

namespace Eurocentric.Domain.V0.Aggregates.Countries;

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
    ///     Gets the country's ISO 3166-1 alpha-2 country code.
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

    public static Result<Country, IDomainError> Create(string countryCode, string countryName)
    {
        if (!LegalCountryCodeValue(countryCode))
        {
            return CountryErrors.IllegalCountryCodeValue(countryCode);
        }

        if (!LegalCountryNameValue(countryName))
        {
            return CountryErrors.IllegalCountryNameValue(countryName);
        }

        return new Country
        {
            Id = Guid.NewGuid(),
            CountryCode = countryCode,
            CountryName = countryName,
            ContestRoles = [],
        };
    }

    private static bool LegalCountryCodeValue(string countryCode) =>
        countryCode.Length == 2 && countryCode.All(char.IsAsciiLetterUpper);

    private static bool LegalCountryNameValue(string countryName) =>
        !string.IsNullOrWhiteSpace(countryName) && countryName.Length <= 200;
}

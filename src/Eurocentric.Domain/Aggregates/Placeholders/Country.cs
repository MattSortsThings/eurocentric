using CSharpFunctionalExtensions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Errors;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public sealed class Country
{
    public Guid Id { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public CountryType CountryType { get; init; }

    public List<ContestRole> ContestRoles { get; init; } = [];

    public static Result<Country, DomainError> TryCreateReal(string countryCode, string countryName)
    {
        return Result
            .Success<Country, DomainError>(
                new Country
                {
                    Id = Guid.NewGuid(),
                    CountryCode = countryCode,
                    CountryName = countryName,
                    CountryType = CountryType.Real,
                }
            )
            .Ensure(
                country => LegalCountryCodeValue(country.CountryCode),
                country => CountryErrors.IllegalCountryCodeValue(country.CountryCode)
            )
            .Ensure(
                country => LegalCountryNameValue(country.CountryName),
                country => CountryErrors.IllegalCountryNameValue(country.CountryName)
            );
    }

    public static Result<Country, DomainError> TryCreatePseudo(string countryCode, string countryName)
    {
        return Result
            .Success<Country, DomainError>(
                new Country
                {
                    Id = Guid.NewGuid(),
                    CountryCode = countryCode,
                    CountryName = countryName,
                    CountryType = CountryType.Pseudo,
                }
            )
            .Ensure(
                country => LegalCountryCodeValue(country.CountryCode),
                country => CountryErrors.IllegalCountryCodeValue(country.CountryCode)
            )
            .Ensure(
                country => LegalCountryNameValue(country.CountryName),
                country => CountryErrors.IllegalCountryNameValue(country.CountryName)
            );
    }

    private static bool LegalCountryCodeValue(string countryCode) =>
        countryCode.Length == 2 && countryCode.All(char.IsAsciiLetterUpper);

    private static bool LegalCountryNameValue(string countryName)
    {
        return !string.IsNullOrWhiteSpace(countryName) & countryName.Length <= 100
            && !countryName.StartsWith(' ')
            && !countryName.EndsWith(' ')
            && !countryName.Contains('\n')
            && !countryName.Contains('\r');
    }
}

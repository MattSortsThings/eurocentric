using ErrorOr;

namespace Eurocentric.Domain.Countries;

public static class InvariantEnforcement
{
    public static ErrorOr<Country> FailIfCountryCodeIsNotUnique(this ErrorOr<Country> errorsOrCountry,
        IQueryable<Country> existingCountries) => errorsOrCountry.IsError
        ? errorsOrCountry
        : errorsOrCountry.Value.CountryCode is { } code && existingCountries.Any(country => country.CountryCode == code)
            ? CountryErrors.CountryCodeConflictError(code)
            : errorsOrCountry;
}

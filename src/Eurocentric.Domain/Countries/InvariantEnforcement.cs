using ErrorOr;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Countries;

public static class InvariantEnforcement
{
    public static ErrorOr<Country> FailOnCountryCodeConflict(this ErrorOr<Country> errorsOrCountry,
        IQueryable<Country> existingCountries)
    {
        if (errorsOrCountry.IsError)
        {
            return errorsOrCountry;
        }

        CountryCode countryCode = errorsOrCountry.Value.CountryCode;

        return existingCountries.Any(country => country.CountryCode == countryCode)
            ? CountryErrors.CountryCodeConflict(countryCode)
            : errorsOrCountry;
    }
}

using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Static methods for enforcing country aggregate invariants.
/// </summary>
public static class CountryInvariants
{
    public static Func<Country, UnitResult<IDomainError>> HasUniqueCountryCode(IQueryable<Country> existingCountries)
    {
        IQueryable<Country> countries = existingCountries;

        return country =>
        {
            CountryCode countryCode = country.CountryCode;

            return countries.Any(existingCountry => existingCountry.CountryCode.Equals(countryCode))
                ? CountryErrors.CountryCodeConflict(countryCode)
                : UnitResult.Success<IDomainError>();
        };
    }
}

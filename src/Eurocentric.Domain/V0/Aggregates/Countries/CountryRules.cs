using CSharpFunctionalExtensions;
using Eurocentric.Domain.Functional;

namespace Eurocentric.Domain.V0.Aggregates.Countries;

public static class CountryRules
{
    public static UnitResult<IDomainError> CanBeDeleted(Country country) =>
        country.ContestRoles.Count > 0
            ? CountryErrors.CountryDeletionNotAllowed(country.Id)
            : UnitResult.Success<IDomainError>();

    public static Func<Country, UnitResult<IDomainError>> HasUniqueCountryCode(IQueryable<Country> existingCountries)
    {
        IQueryable<Country> aggregates = existingCountries;

        return country =>
        {
            string countryCode = country.CountryCode;

            return aggregates.Any(aggregate => aggregate.CountryCode == countryCode)
                ? CountryErrors.CountryCodeConflict(countryCode)
                : UnitResult.Success<IDomainError>();
        };
    }
}

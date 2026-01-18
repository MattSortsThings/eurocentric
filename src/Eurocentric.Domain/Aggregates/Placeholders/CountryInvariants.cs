using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public static class CountryInvariants
{
    public static Func<Country, UnitResult<DomainError>> CountryCodeIsUnique(IQueryable<Country> existingCountries)
    {
        IQueryable<Country> queryable = existingCountries;

        return country =>
            queryable.Any(existingCountry => existingCountry.CountryCode == country.CountryCode)
                ? CountryErrors.CountryCodeConflict(country.CountryCode)
                : UnitResult.Success<DomainError>();
    }

    public static UnitResult<DomainError> DeletionAllowed(Country country)
    {
        return country.ContestRoles.Count == 0
            ? UnitResult.Success<DomainError>()
            : UnitResult.Failure(CountryErrors.CountryDeletionDisallowed(country.Id));
    }
}

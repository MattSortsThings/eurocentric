using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.DomainErrors;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Rules.External.DbCheckers;

namespace Eurocentric.Domain.Rules.External;

public static class CountryValidity
{
    public static ErrorOr<Country> EnforceExternalRules(this ErrorOr<Country> errorsOrCountry,
        ICountryDbChecker countryDbChecker) =>
        errorsOrCountry.FailIf(countryDbChecker.CountryCodeIsNotUnique, Errors.Countries.CountryCodeConflict);
}

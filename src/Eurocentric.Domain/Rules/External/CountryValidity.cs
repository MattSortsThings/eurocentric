using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Errors;
using Eurocentric.Domain.Rules.DataCheckers;

namespace Eurocentric.Domain.Rules.External;

public static class CountryValidity
{
    public static ErrorOr<Country> EnforceExternalRules(this ErrorOr<Country> errorsOrCountry, IDataChecker dataChecker) =>
        errorsOrCountry.FailIfCountryCodeIsNotUnique(dataChecker);

    private static ErrorOr<Country> FailIfCountryCodeIsNotUnique(this ErrorOr<Country> subject, IDataChecker dataChecker) =>
        subject.FailIf(country => dataChecker.CountryExistsWithCountryCode(country.CountryCode),
            DomainErrors.Countries.CountryCodeConflict);
}

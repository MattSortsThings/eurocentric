using ErrorOr;
using Eurocentric.Domain.DomainErrors;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Rules.Internal;

internal static class CountryCodeValidity
{
    internal static ErrorOr<CountryCode> EnforceInternalRules(this CountryCode countryCode) => countryCode.ToErrorOr()
        .FailIf(ValueLengthIsNot2Chars, Errors.Countries.InvalidCountryCode(countryCode))
        .FailIf(ValueIsNotAllUpperCaseLetters, Errors.Countries.InvalidCountryCode(countryCode));

    private static bool ValueLengthIsNot2Chars(CountryCode countryCode) =>
        countryCode.Value.Length != DomainConstants.CountryCode.RequiredLengthInChars;

    private static bool ValueIsNotAllUpperCaseLetters(CountryCode countryCode) => !countryCode.Value.All(char.IsUpper);
}

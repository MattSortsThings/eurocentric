using ErrorOr;
using Eurocentric.Domain.DomainErrors;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Rules.Internal;

internal static class CountryNameValidity
{
    internal static ErrorOr<CountryName> EnforceInternalRules(this CountryName countryName) => countryName.ToErrorOr()
        .FailIf(ValueLengthIsGreaterThan200Chars, Errors.Countries.InvalidCountryName(countryName))
        .FailIf(ValueIsEmptyOrAllWhiteSpace, Errors.Countries.InvalidCountryName(countryName));

    private static bool ValueLengthIsGreaterThan200Chars(CountryName countryName) =>
        countryName.Value.Length > DomainConstants.CountryName.MaxPermittedLengthInChars;

    private static bool ValueIsEmptyOrAllWhiteSpace(CountryName countryName) =>
        string.IsNullOrWhiteSpace(countryName.Value);
}

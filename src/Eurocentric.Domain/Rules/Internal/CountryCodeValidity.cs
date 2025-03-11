using ErrorOr;
using Eurocentric.Domain.Constants;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Errors;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Rules.Internal;

internal static class CountryCodeValidity
{
    internal static ErrorOr<CountryCode> EnforceInternalRules(this ErrorOr<CountryCode> subject) =>
        subject.FailIfValueHasInvalidLength()
            .FailIfValueIsNotAllUpperCaseLetters();

    private static ErrorOr<CountryCode> FailIfValueHasInvalidLength(this ErrorOr<CountryCode> subject) =>
        subject.FailIf(c => c.Value.Length != DomainConstants.ValueObjects.CountryCodes.RequiredLengthInChars,
            DomainErrors.ValueObjects.CountryCodes.InvalidCountryCode);

    private static ErrorOr<CountryCode> FailIfValueIsNotAllUpperCaseLetters(this ErrorOr<CountryCode> subject) =>
        subject.FailIf(c => !c.Value.All(char.IsUpper),
            DomainErrors.ValueObjects.CountryCodes.InvalidCountryCode);
}

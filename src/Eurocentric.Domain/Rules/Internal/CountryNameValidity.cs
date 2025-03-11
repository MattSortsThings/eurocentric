using ErrorOr;
using Eurocentric.Domain.Constants;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Errors;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Rules.Internal;

internal static class CountryNameValidity
{
    internal static ErrorOr<CountryName> EnforceInternalRules(this ErrorOr<CountryName> subject) =>
        subject.FailIfValueHasInvalidLength()
            .FailIfValueIsEmptyOrWhitespace();

    private static ErrorOr<CountryName> FailIfValueHasInvalidLength(this ErrorOr<CountryName> subject) =>
        subject.FailIf(c => c.Value.Length > DomainConstants.ValueObjects.CountryNames.MaxPermittedLengthInChars,
            DomainErrors.ValueObjects.CountryNames.InvalidCountryName);

    private static ErrorOr<CountryName> FailIfValueIsEmptyOrWhitespace(this ErrorOr<CountryName> subject) =>
        subject.FailIf(c => string.IsNullOrWhiteSpace(c.Value),
            DomainErrors.ValueObjects.CountryNames.InvalidCountryName);
}

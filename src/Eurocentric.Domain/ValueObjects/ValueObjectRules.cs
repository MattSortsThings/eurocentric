using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Static methods for enforcing domain value object rules.
/// </summary>
public static class ValueObjectRules
{
    /// <summary>
    ///     Checks that the provided value is a legal <see cref="CountryCode" /> value.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <returns>An error if the rule was not satisfied; otherwise, a successful result with no return value.</returns>
    public static UnitResult<IDomainError> LegalCountryCodeValue(string value)
    {
        if (value.Length == 2 && value.All(char.IsAsciiLetterUpper))
        {
            return UnitResult.Success<IDomainError>();
        }

        return ValueObjectErrors.IllegalCountryCodeValue(value);
    }

    /// <summary>
    ///     Checks that the provided value is a legal <see cref="CountryName" /> value.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <returns>An error if the rule was not satisfied; otherwise, a successful result with no return value.</returns>
    public static UnitResult<IDomainError> LegalCountryNameValue(string value)
    {
        if (!string.IsNullOrWhiteSpace(value) && value.Length is > 0 and <= 200)
        {
            return UnitResult.Success<IDomainError>();
        }

        return ValueObjectErrors.IllegalCountryNameValue(value);
    }
}

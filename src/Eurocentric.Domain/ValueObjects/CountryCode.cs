using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     A country's ISO 3166-1 alpha-2 country code.
/// </summary>
public sealed class CountryCode : StringAtomicValueObject
{
    private CountryCode(string value)
        : base(value) { }

    /// <summary>
    ///     Creates and returns a new <see cref="CountryCode" /> instance with the provided
    ///     <see cref="StringAtomicValueObject.Value" />.
    /// </summary>
    /// <param name="value">A string of 2 upper-case letters. The underlying value of the instance to be created.</param>
    /// <returns><i>Either</i> a new <see cref="CountryCode" /> instance <i>or</i> an error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static Result<CountryCode, IDomainError> FromValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return Result
            .Success<string, IDomainError>(value)
            .Ensure(ValueObjectRules.LegalCountryCodeValue)
            .Map(v => new CountryCode(v));
    }
}

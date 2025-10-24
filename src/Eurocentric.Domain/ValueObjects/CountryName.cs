using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     A country's short UK English name.
/// </summary>
public sealed class CountryName : StringAtomicValueObject
{
    private CountryName(string value)
        : base(value) { }

    /// <summary>
    ///     Creates and returns a new <see cref="CountryName" /> instance with the provided
    ///     <see cref="StringAtomicValueObject.Value" />.
    /// </summary>
    /// <param name="value">
    ///     A non-empty, non-whitespace string of no more than 200 characters. The underlying value of the
    ///     instance to be created.
    /// </param>
    /// <returns><i>Either</i> a new <see cref="CountryName" /> instance <i>or</i> an error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static Result<CountryName, IDomainError> FromValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return Result
            .Success<string, IDomainError>(value)
            .Ensure(ValueObjectRules.LegalCountryNameValue)
            .Map(v => new CountryName(v));
    }
}

using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     An act's official performing name.
/// </summary>
public sealed class ActName : StringAtomicValueObject
{
    private ActName(string value)
        : base(value) { }

    /// <summary>
    ///     Creates and returns a new <see cref="ActName" /> instance with the provided
    ///     <see cref="StringAtomicValueObject.Value" />.
    /// </summary>
    /// <param name="value">
    ///     A non-empty, non-whitespace string of no more than 200 characters. The underlying value of the
    ///     instance to be created.
    /// </param>
    /// <returns><i>Either</i> a new <see cref="ActName" /> instance <i>or</i> an error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static Result<ActName, IDomainError> FromValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return Result
            .Success<string, IDomainError>(value)
            .Ensure(ValueObjectInvariants.LegalActNameValue)
            .Map(v => new ActName(v));
    }
}

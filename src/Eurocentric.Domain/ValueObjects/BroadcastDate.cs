using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     The date on which a broadcast is televised.
/// </summary>
public sealed class BroadcastDate : DateOnlyAtomicValueObject
{
    private BroadcastDate(DateOnly value)
        : base(value) { }

    /// <summary>
    ///     Creates and returns a new <see cref="BroadcastDate" /> instance with the provided
    ///     <see cref="DateOnlyAtomicValueObject.Value" />.
    /// </summary>
    /// <param name="value">
    ///     A date with a<see cref="DateOnly.Year" /> value between 2016 and 2050. The underlying value of the instance
    ///     to be created.
    /// </param>
    /// <returns><i>Either</i> a new <see cref="BroadcastDate" /> instance <i>or</i> an error.</returns>
    public static Result<BroadcastDate, IDomainError> FromValue(DateOnly value)
    {
        return Result
            .Success<DateOnly, IDomainError>(value)
            .Ensure(ValueObjectInvariants.LegalBroadcastDateValue)
            .Map(dateValue => new BroadcastDate(dateValue));
    }
}

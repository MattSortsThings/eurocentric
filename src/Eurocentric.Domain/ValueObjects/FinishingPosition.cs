using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     A competitor's finishing position in a broadcast.
/// </summary>
public sealed class FinishingPosition : Int32AtomicValueObject
{
    private FinishingPosition(int value)
        : base(value) { }

    /// <summary>
    ///     Creates and returns a new <see cref="FinishingPosition" /> instance with the provided
    ///     <see cref="Int32AtomicValueObject.Value" />.
    /// </summary>
    /// <param name="value">
    ///     An integer greater than or equal to 1. The underlying value of the instance to be created.
    /// </param>
    /// <returns><i>Either</i> a new <see cref="FinishingPosition" /> instance <i>or</i> an error.</returns>
    public static Result<FinishingPosition, IDomainError> FromValue(int value)
    {
        return Result
            .Success<int, IDomainError>(value)
            .Ensure(ValueObjectInvariants.LegalFinishingPositionValue)
            .Map(intValue => new FinishingPosition(intValue));
    }

    /// <summary>
    ///     Creates a sequence of consecutive <see cref="FinishingPosition" /> instances starting at
    ///     <see cref="FinishingPosition.Value" /> = 1.
    /// </summary>
    /// <param name="count">A non-negative integer. The length of the sequence to be generated.</param>
    /// <returns>A finite sequence of new <see cref="FinishingPosition" /> instances.</returns>
    public static IEnumerable<FinishingPosition> CreateSequence(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        return Enumerable.Range(1, count).Select(value => new FinishingPosition(value));
    }
}

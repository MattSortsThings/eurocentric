using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     A competitor's running order spot in a broadcast.
/// </summary>
public sealed class RunningOrderSpot : Int32AtomicValueObject
{
    private RunningOrderSpot(int value)
        : base(value) { }

    /// <summary>
    ///     Creates and returns a new <see cref="RunningOrderSpot" /> instance with the provided
    ///     <see cref="Int32AtomicValueObject.Value" />.
    /// </summary>
    /// <param name="value">
    ///     An integer greater than or equal to 1. The underlying value of the instance to be created.
    /// </param>
    /// <returns><i>Either</i> a new <see cref="RunningOrderSpot" /> instance <i>or</i> an error.</returns>
    public static Result<RunningOrderSpot, IDomainError> FromValue(int value)
    {
        return Result
            .Success<int, IDomainError>(value)
            .Ensure(ValueObjectInvariants.LegalRunningOrderSpotValue)
            .Map(intValue => new RunningOrderSpot(intValue));
    }

    /// <summary>
    ///     Creates a sequence of consecutive <see cref="RunningOrderSpot" /> instances starting at
    ///     <see cref="RunningOrderSpot.Value" /> = 1.
    /// </summary>
    /// <param name="count">A non-negative integer. The length of the sequence to be generated.</param>
    /// <returns>A finite sequence of new <see cref="RunningOrderSpot" /> instances.</returns>
    public static IEnumerable<RunningOrderSpot> CreateSequence(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        return Enumerable.Range(1, count).Select(value => new RunningOrderSpot(value));
    }
}

using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     The year in which a contest is held.
/// </summary>
public sealed class ContestYear : Int32AtomicValueObject
{
    private ContestYear(int value)
        : base(value) { }

    /// <summary>
    ///     Creates and returns a new <see cref="ContestYear" /> instance with the provided
    ///     <see cref="Int32AtomicValueObject.Value" />.
    /// </summary>
    /// <param name="value">
    ///     An integer between 2016 and 2050. The underlying value of the instance to be created.
    /// </param>
    /// <returns><i>Either</i> a new <see cref="ContestYear" /> instance <i>or</i> an error.</returns>
    public static Result<ContestYear, IDomainError> FromValue(int value)
    {
        return Result
            .Success<int, IDomainError>(value)
            .Ensure(ValueObjectInvariants.LegalContestYearValue)
            .Map(v => new ContestYear(v));
    }
}

using ErrorOr;
using Eurocentric.Domain.Abstractions;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Contains the year in which a contest is held.
/// </summary>
public sealed class ContestYear : ValueObject, IComparable<ContestYear>
{
    private const int MinLegalValue = 2016;
    private const int MaxLegalValue = 2050;

    private ContestYear(int value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying integer value of this instance.
    /// </summary>
    public int Value { get; }

    /// <inheritdoc />
    public int CompareTo(ContestYear? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null ? 1 : Value.CompareTo(other.Value);
    }

    /// <summary>
    ///     Creates and returns a new <see cref="ContestYear" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <remarks>
    ///     A <see cref="ContestYear" /> instance created using this method is guaranteed to be a legal contest year in the
    ///     system. A legal country name value is an integer between 2016 and 2050.
    /// </remarks>
    /// <param name="value">
    ///     An integer between 2016 and 2050. The underlying value of the instance to be created.
    /// </param>
    /// <returns>
    ///     A new <see cref="ContestYear" /> instance if the <paramref name="value" /> parameter is a legal contest year
    ///     value; otherwise, a list of <see cref="Error" /> values.
    /// </returns>
    public static ErrorOr<ContestYear> FromValue(int value) =>
        LegalContestYearValue(value)
            ? new ContestYear(value)
            : ValueObjectErrors.IllegalContestYearValue(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    private static bool LegalContestYearValue(int value) => value is >= MinLegalValue and <= MaxLegalValue;
}

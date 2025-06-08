using ErrorOr;
using Eurocentric.Domain.Abstractions;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Represents the date on which a broadcast is televised.
/// </summary>
public sealed class BroadcastDate : ValueObject, IComparable<BroadcastDate>
{
    private static readonly DateOnly MinLegalValue = DateOnly.ParseExact("2016-01-01", "yyyy-MM-dd");
    private static readonly DateOnly MaxLegalValue = DateOnly.ParseExact("2050-12-31", "yyyy-MM-dd");

    private BroadcastDate(DateOnly value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying integer value of this instance.
    /// </summary>
    public DateOnly Value { get; }

    /// <inheritdoc />
    public int CompareTo(BroadcastDate? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null
            ? 1
            : Value.CompareTo(other.Value);
    }

    /// <summary>
    ///     Creates and returns a new <see cref="BroadcastDate" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <remarks>
    ///     A <see cref="BroadcastDate" /> instance created using this method is guaranteed to be a legal broadcast date in the
    ///     system. A legal broadcast date is between 1 January 2016 and 31 December 2050.
    /// </remarks>
    /// <param name="value">
    ///     A date between 1 January 2016 and 31 December 2050. The underlying value of the instance to be created.
    /// </param>
    /// <returns>
    ///     A new <see cref="BroadcastDate" /> instance if the <paramref name="value" /> parameter is a legal broadcast date
    ///     value; otherwise, a list of <see cref="Error" /> values.
    /// </returns>
    public static ErrorOr<BroadcastDate> FromValue(DateOnly value) =>
        LegalBroadcastDateValue(value)
            ? new BroadcastDate(value)
            : ValueObjectErrors.IllegalBroadcastDateValue(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    private static bool LegalBroadcastDateValue(DateOnly value) => value >= MinLegalValue && value <= MaxLegalValue;
}

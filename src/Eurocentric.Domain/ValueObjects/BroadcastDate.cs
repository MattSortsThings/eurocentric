using ErrorOr;
using Eurocentric.Domain.Abstractions;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Represents the date on which a broadcast is televised.
/// </summary>
public sealed class BroadcastDate : ValueObject, IComparable<BroadcastDate>
{
    private const int MinLegalValue = 2016;
    private const int MaxLegalValue = 2050;

    private BroadcastDate(DateOnly value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying <see cref="DateOnly" /> value of this instance.
    /// </summary>
    public DateOnly Value { get; }

    /// <inheritdoc />
    public int CompareTo(BroadcastDate? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null ? 1 : Value.CompareTo(other.Value);
    }

    private protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    /// <summary>
    ///     Creates and returns a new <see cref="BroadcastDate" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <remarks>
    ///     A <see cref="BroadcastDate" /> instance created using this method is guaranteed to be a legal broadcast date in the
    ///     domain. A legal broadcast date value has a year between 2016 and 2050.
    /// </remarks>
    /// <param name="value">
    ///     A date only value. The underlying value of the instance to be created.
    /// </param>
    /// <returns>
    ///     A new <see cref="BroadcastDate" /> instance if the <paramref name="value" /> parameter is a legal broadcast date
    ///     value; otherwise, a list of <see cref="Error" /> values.
    /// </returns>
    public static ErrorOr<BroadcastDate> FromValue(DateOnly value) => LegalBroadcastDateValue(value)
        ? new BroadcastDate(value)
        : ValueObjectErrors.IllegalBroadcastDateValue(value);

    private static bool LegalBroadcastDateValue(DateOnly value) => value.Year is >= MinLegalValue and <= MaxLegalValue;
}

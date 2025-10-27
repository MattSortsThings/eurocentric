namespace Eurocentric.Domain.Core;

/// <summary>
///     Abstract base class for a domain value object with a single <see cref="DateOnly" /> value.
/// </summary>
public abstract class DateOnlyAtomicValueObject : ValueObject, IComparable<DateOnlyAtomicValueObject>
{
    private protected DateOnlyAtomicValueObject(DateOnly value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying <see cref="DateOnly" /> value of the instance.
    /// </summary>
    public DateOnly Value { get; }

    /// <inheritdoc />
    /// <remarks><see cref="DateOnlyAtomicValueObject" /> instances are compared by their <see cref="Value" /> properties.</remarks>
    public int CompareTo(DateOnlyAtomicValueObject? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null ? 1 : Value.CompareTo(other.Value);
    }

    /// <inheritdoc />
    /// <remarks><see cref="DateOnlyAtomicValueObject" /> instances are compared by their <see cref="Value" /> properties.</remarks>
    public override bool Equals(ValueObject? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is DateOnlyAtomicValueObject d && d.GetType() == GetType() && d.Value == Value;
    }

    /// <inheritdoc />
    public override int GetHashCode() => Value.GetHashCode();
}

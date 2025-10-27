namespace Eurocentric.Domain.Core;

/// <summary>
///     Abstract base class for a domain value object with a single <see cref="int" /> value.
/// </summary>
public abstract class Int32AtomicValueObject : ValueObject, IComparable<Int32AtomicValueObject>
{
    private protected Int32AtomicValueObject(int value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying <see cref="int" /> value of the instance.
    /// </summary>
    public int Value { get; }

    /// <inheritdoc />
    /// <remarks><see cref="Int32AtomicValueObject" /> instances are compared by their <see cref="Value" /> properties.</remarks>
    public int CompareTo(Int32AtomicValueObject? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null ? 1 : Value.CompareTo(other.Value);
    }

    /// <inheritdoc />
    /// <remarks><see cref="Int32AtomicValueObject" /> instances are compared by their <see cref="Value" /> properties.</remarks>
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

        return other is Int32AtomicValueObject i && i.GetType() == GetType() && i.Value == Value;
    }

    /// <inheritdoc />
    public override int GetHashCode() => Value.GetHashCode();
}

namespace Eurocentric.Domain.Core;

/// <summary>
///     Abstract base class for a domain value object with a single <see cref="Guid" /> value.
/// </summary>
public abstract class GuidAtomicValueObject : ValueObject, IComparable<GuidAtomicValueObject>
{
    private protected GuidAtomicValueObject(Guid value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying <see cref="Guid" /> value of the instance.
    /// </summary>
    public Guid Value { get; }

    /// <inheritdoc />
    /// <remarks><see cref="GuidAtomicValueObject" /> instances are compared by their <see cref="Value" /> properties.</remarks>
    public int CompareTo(GuidAtomicValueObject? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null ? 1 : Value.CompareTo(other.Value);
    }

    /// <inheritdoc />
    /// <remarks><see cref="GuidAtomicValueObject" /> instances are compared by their <see cref="Value" /> properties.</remarks>
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

        return other is GuidAtomicValueObject g && g.GetType() == GetType() && g.Value == Value;
    }

    /// <inheritdoc />
    public override int GetHashCode() => Value.GetHashCode();
}

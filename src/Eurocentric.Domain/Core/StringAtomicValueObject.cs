namespace Eurocentric.Domain.Core;

/// <summary>
///     Abstract base class for a domain value object with a single <see cref="string" /> value.
/// </summary>
public abstract class StringAtomicValueObject : ValueObject, IComparable<StringAtomicValueObject>
{
    private protected StringAtomicValueObject(string value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying <see cref="string" /> value of the instance.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    /// <remarks>
    ///     <see cref="StringAtomicValueObject" /> instances are compared by their <see cref="Value" /> properties using
    ///     <see cref="StringComparison.Ordinal" /> string comparison rules.
    /// </remarks>
    public int CompareTo(StringAtomicValueObject? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null ? 1 : string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    /// <remarks>
    ///     <see cref="StringAtomicValueObject" /> instances are compared by their <see cref="Value" /> properties using
    ///     <see cref="StringComparison.Ordinal" /> string comparison rules.
    /// </remarks>
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

        return other is StringAtomicValueObject stringValueObject
            && stringValueObject.GetType() == GetType()
            && string.Equals(Value, stringValueObject.Value, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    /// <remarks>
    ///     <see cref="StringAtomicValueObject" /> instances are compared by their <see cref="Value" /> properties using
    ///     <see cref="StringComparison.Ordinal" /> string comparison rules.
    /// </remarks>
    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);
}

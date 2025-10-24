namespace Eurocentric.Domain.Core;

/// <summary>
///     Abstract base class for a domain value object with two or more values.
/// </summary>
public abstract class CompoundValueObject : ValueObject
{
    /// <inheritdoc />
    /// <remarks><see cref="StringAtomicValueObject" /> instances are compared by their atomic values.</remarks>
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

        return other is CompoundValueObject compoundValueObject
            && compoundValueObject.GetType() == GetType()
            && compoundValueObject.GetAtomicValues().SequenceEqual(GetAtomicValues());
    }

    /// <inheritdoc />
    /// <remarks><see cref="StringAtomicValueObject" /> instances are compared by their atomic values.</remarks>
    public override int GetHashCode() =>
        GetAtomicValues().Aggregate(0, (hashcode, value) => HashCode.Combine(hashcode, value.GetHashCode()));

    /// <summary>
    ///     Generates the sequence of atomic values for comparison.
    /// </summary>
    /// <returns>A sequence of values in a fixed order.</returns>
    private protected abstract IEnumerable<object> GetAtomicValues();
}

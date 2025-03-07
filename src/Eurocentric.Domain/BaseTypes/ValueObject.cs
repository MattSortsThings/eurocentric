namespace Eurocentric.Domain.BaseTypes;

/// <summary>
///     Abstract base class for a domain value object type.
/// </summary>
/// <remarks>
///     This class type is adapted from a very helpful
///     <a href="https://www.milanjovanovic.tech/blog/value-objects-in-dotnet-ddd-fundamentals">blog post</a> by Milan
///     Jovanovic.
/// </remarks>
public abstract class ValueObject : IEquatable<ValueObject>
{
    public virtual bool Equals(ValueObject? other) => other is not null && AtomicValuesAreEqual(other);

    public static bool operator ==(ValueObject? a, ValueObject? b) =>
        (a is null && b is null) || (a is not null && b is not null && a.Equals(b));

    public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);

    public override bool Equals(object? obj) => obj is ValueObject valueObject && AtomicValuesAreEqual(valueObject);

    public override int GetHashCode() =>
        GetAtomicValues().Aggregate(0, (hashcode, value) => HashCode.Combine(hashcode, value.GetHashCode()));

    protected abstract IEnumerable<object> GetAtomicValues();

    private bool AtomicValuesAreEqual(ValueObject valueObject) => GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
}

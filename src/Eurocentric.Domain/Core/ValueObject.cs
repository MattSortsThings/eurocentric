namespace Eurocentric.Domain.Core;

/// <summary>
///     Abstract base class for a domain value object.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <inheritdoc />
    public abstract bool Equals(ValueObject? other);

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is ValueObject valueObject && Equals(valueObject);
    }

    /// <inheritdoc />
    public abstract override int GetHashCode();

    /// <summary>
    ///     Determines whether two value objects have the same value.
    /// </summary>
    /// <param name="left">The first value object to compare, or <see langword="null" />.</param>
    /// <param name="right">The first value object to compare, or <see langword="null" />.</param>
    /// <returns>
    ///     <see langword="true" /> if the value of <paramref name="left" /> is the same as the value of
    ///     <paramref name="right" />; otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator ==(ValueObject? left, ValueObject? right) => Equals(left, right);

    /// <summary>
    ///     Determines whether two value objects have different values.
    /// </summary>
    /// <param name="left">The first value object to compare, or <see langword="null" />.</param>
    /// <param name="right">The first value object to compare, or <see langword="null" />.</param>
    /// <returns>
    ///     <see langword="true" /> if the value of <paramref name="left" /> is the different from the value of
    ///     <paramref name="right" />; otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator !=(ValueObject? left, ValueObject? right) => !Equals(left, right);
}

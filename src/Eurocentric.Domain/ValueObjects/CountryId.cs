using Eurocentric.Domain.Abstractions;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Identifies a country aggregate in the system.
/// </summary>
public sealed class CountryId : ValueObject, IComparable<CountryId>
{
    private CountryId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    ///     The underlying <see cref="Guid" /> value of the country ID.
    /// </summary>
    public Guid Value { get; }

    /// <inheritdoc />
    public int CompareTo(CountryId? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null ? 1 : Value.CompareTo(other.Value);
    }

    /// <summary>
    ///     Creates and returns a new <see cref="CountryId" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <param name="value">The underlying <see cref="Guid" /> value of the country ID.</param>
    /// <returns>A new <see cref="CountryId" /> instance.</returns>
    public static CountryId FromValue(Guid value) => new(value);

    private protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

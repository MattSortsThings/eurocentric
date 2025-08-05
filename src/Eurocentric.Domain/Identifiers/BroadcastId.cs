using Eurocentric.Domain.Abstractions;

namespace Eurocentric.Domain.Identifiers;

/// <summary>
///     Identifies a broadcast aggregate in the system.
/// </summary>
public sealed class BroadcastId : ValueObject, IComparable<BroadcastId>
{
    private BroadcastId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    ///     The underlying <see cref="Guid" /> value of the country ID.
    /// </summary>
    public Guid Value { get; }

    /// <inheritdoc />
    public int CompareTo(BroadcastId? other)
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
    ///     Creates and returns a new <see cref="BroadcastId" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <param name="value">The underlying <see cref="Guid" /> value of the country ID.</param>
    /// <returns>A new <see cref="BroadcastId" /> instance.</returns>
    public static BroadcastId FromValue(Guid value) => new(value);
}

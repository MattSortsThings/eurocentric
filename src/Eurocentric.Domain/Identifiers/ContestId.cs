using Eurocentric.Domain.Abstractions;

namespace Eurocentric.Domain.Identifiers;

/// <summary>
///     Identifies a contest aggregate in the system.
/// </summary>
public sealed class ContestId : ValueObject, IComparable<ContestId>
{
    private ContestId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    ///     The underlying <see cref="Guid" /> value of the country ID.
    /// </summary>
    public Guid Value { get; }

    /// <inheritdoc />
    public int CompareTo(ContestId? other)
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
    ///     Creates and returns a new <see cref="ContestId" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <param name="value">The underlying <see cref="Guid" /> value of the country ID.</param>
    /// <returns>A new <see cref="ContestId" /> instance.</returns>
    public static ContestId FromValue(Guid value) => new(value);
}

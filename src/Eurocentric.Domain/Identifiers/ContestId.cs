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
    ///     Gets the underlying value of this instance.
    /// </summary>
    public Guid Value { get; }

    /// <inheritdoc />
    public int CompareTo(ContestId? other) =>
        ReferenceEquals(this, other)
            ? 0
            : other is null
                ? 1
                : Value.CompareTo(other.Value);

    /// <summary>
    ///     Creates and returns a new <see cref="ContestId" /> instance with a random <see cref="Value" />.
    /// </summary>
    /// <remarks>
    ///     The <see cref="Guid" /> value is generated using according to RFC 9562, following the Version 7 format, using
    ///     the provided date-time offset value.
    /// </remarks>
    /// <param name="dateTimeOffset">The point in time at which the <see cref="ContestId" /> is requested.</param>
    /// <returns>A new <see cref="ContestId" /> instance.</returns>
    public static ContestId Create(DateTimeOffset dateTimeOffset) => new(Guid.CreateVersion7(dateTimeOffset));

    /// <summary>
    ///     Creates and returns a new <see cref="ContestId" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <param name="value">The underlying value of the instance to be created.</param>
    /// <returns>A new <see cref="ContestId" /> instance.</returns>
    public static ContestId FromValue(Guid value) => new(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

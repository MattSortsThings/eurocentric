using Eurocentric.Domain.BaseTypes;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Identifies a contest aggregate.
/// </summary>
public sealed class ContestId : ValueObject
{
    private ContestId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying value of this instance.
    /// </summary>
    public Guid Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    /// <summary>
    ///     Creates and returns a new <see cref="ContestId" /> instance with a randomly generated <see cref="Value" />.
    /// </summary>
    /// <remarks>
    ///     The <see cref="Guid" /> value is generated according to the RFC 9562 specification, following the Version 7 format.
    /// </remarks>
    /// <param name="dateTimeOffset">Used to generate the <see cref="Guid" />.</param>
    /// <returns>A new <see cref="ContestId" /> instance.</returns>
    public static ContestId Create(DateTimeOffset dateTimeOffset) => new(Guid.CreateVersion7(dateTimeOffset));

    /// <summary>
    ///     Creates and returns a new <see cref="ContestId" /> instance with the specified <see cref="Value" />.
    /// </summary>
    /// <param name="value">The underlying value of the instance to be created.</param>
    /// <returns>A new <see cref="ContestId" /> instance.</returns>
    public static ContestId FromValue(Guid value) => new(value);
}

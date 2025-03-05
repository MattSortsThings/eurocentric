using Eurocentric.Domain.BaseTypes;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Identifies a country aggregate.
/// </summary>
public sealed class CountryId : ValueObject
{
    private CountryId(Guid value)
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
    ///     Creates and returns a new <see cref="CountryId" /> instance with a randomly generated <see cref="Value" />.
    /// </summary>
    /// <remarks>
    ///     The <see cref="Guid" /> value is generated according to the RFC 9562 specification, following the Version 7 format.
    /// </remarks>
    /// <param name="dateTimeOffset">Used to generate the <see cref="Guid" />.</param>
    /// <returns>A new <see cref="CountryId" /> instance.</returns>
    public static CountryId Create(DateTimeOffset dateTimeOffset) => new(Guid.CreateVersion7(dateTimeOffset));

    /// <summary>
    ///     Creates and returns a new <see cref="CountryId" /> instance with the specified <see cref="Value" />.
    /// </summary>
    /// <param name="value">The underlying value of the instance to be created.</param>
    /// <returns>A new <see cref="CountryId" /> instance.</returns>
    public static CountryId FromValue(Guid value) => new(value);
}

using Eurocentric.Domain.Abstractions;

namespace Eurocentric.Domain.Identifiers;

/// <summary>
///     Identifies a country aggregate in the system.
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

    /// <summary>
    ///     Creates and returns a new <see cref="CountryId" /> instance with a random <see cref="Value" />.
    /// </summary>
    /// <remarks>
    ///     The <see cref="Guid" /> value is generated using according to RFC 9562, following the Version 7 format, using
    ///     the provided date-time offset value.
    /// </remarks>
    /// <param name="dateTimeOffset">The point in time at which the <see cref="CountryId" /> is requested.</param>
    /// <returns>A new <see cref="CountryId" /> instance.</returns>
    public static CountryId Create(DateTimeOffset dateTimeOffset) => new(Guid.CreateVersion7(dateTimeOffset));

    /// <summary>
    ///     Creates and returns a new <see cref="CountryId" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <param name="value">The underlying value of the instance to be created.</param>
    /// <returns>A new <see cref="CountryId" /> instance.</returns>
    public static CountryId FromValue(Guid value) => new(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

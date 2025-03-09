using ErrorOr;
using Eurocentric.Domain.BaseTypes;
using Eurocentric.Domain.Rules.Internal;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     A country's short UK English name.
/// </summary>
public sealed class CountryName : ValueObject
{
    private CountryName(string value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying value of this instance.
    /// </summary>
    public string Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    /// <summary>
    ///     Creates and returns a valid <see cref="CountryName" /> instance with the specified <see cref="Value" />.
    /// </summary>
    /// <param name="value">
    ///     A non-empty, non-white-space string of no more than 200 characters. The country's short UK English name.
    /// </param>
    /// <returns>Either a new <see cref="CountryName" /> instance or a list of errors.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static ErrorOr<CountryName> Create(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return FromValue(value).EnforceInternalRules();
    }

    /// <summary>
    ///     Creates and returns a <see cref="CountryName" /> instance with the specified <see cref="Value" />.
    /// </summary>
    /// <remarks>This method does not guarantee a valid instance. It should be used for database value conversion only.</remarks>
    /// <param name="value">The underlying value of the instance.</param>
    /// <returns>A new <see cref="Eurocentric.Domain.ValueObjects.CountryName" /> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static CountryName FromValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new CountryName(value);
    }
}

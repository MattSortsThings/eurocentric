using ErrorOr;
using Eurocentric.Domain.BaseTypes;
using Eurocentric.Domain.DomainErrors;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     A country's ISO 3166-1 alpha-2 country code.
/// </summary>
public sealed class CountryCode : ValueObject, IComparable<CountryCode>
{
    public const int RequiredLengthInChars = 2;

    private CountryCode(string value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying value of this instance.
    /// </summary>
    public string Value { get; }

    public int CompareTo(CountryCode? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null ? 1 : string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    /// <summary>
    ///     Creates and returns a valid <see cref="CountryCode" /> instance with the specified <see cref="Value" />.
    /// </summary>
    /// <param name="value">A string of 2 upper-case letters. The country's ISO 3166-1 alpha-2 country code.</param>
    /// <returns>Either a new <see cref="CountryCode" /> instance or a list of errors.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static ErrorOr<CountryCode> Create(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return Valid(value) ? new CountryCode(value) : Errors.Countries.InvalidCountryCode(value);
    }

    /// <summary>
    ///     Creates and returns a <see cref="CountryCode" /> instance with the specified <see cref="Value" />.
    /// </summary>
    /// <remarks>This method does not guarantee a valid instance. It should be used for database value conversion only.</remarks>
    /// <param name="value">The underlying value of the instance..</param>
    /// <returns>A new <see cref="CountryCode" /> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static CountryCode FromValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new CountryCode(value);
    }

    private static bool Valid(string value) => value.Length == RequiredLengthInChars && value.All(char.IsAsciiLetterUpper);
}

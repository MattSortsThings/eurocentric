using ErrorOr;
using Eurocentric.Domain.Abstractions;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Contains a country's ISO 3166-1 alpha-2 code.
/// </summary>
public sealed class CountryCode : ValueObject, IComparable<CountryCode>
{
    private const int FixedLengthInChars = 2;

    private CountryCode(string value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying string value of this instance.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public int CompareTo(CountryCode? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null ? 1 : string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    /// <summary>
    ///     Creates and returns a new <see cref="CountryCode" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <remarks>
    ///     A <see cref="CountryCode" /> instance created using this method is guaranteed to be a legal country code in the
    ///     system. A legal country code value is a string of 2 upper-case letters.
    /// </remarks>
    /// <param name="value">A string of 2 upper-case letters. The underlying value of the instance to be created.</param>
    /// <returns>
    ///     A new <see cref="CountryCode" /> instance if the <paramref name="value" /> parameter is a legal country code
    ///     value; otherwise, a list of <see cref="Error" /> values.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static ErrorOr<CountryCode> FromValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return LegalCountryCodeValue(value) ? new CountryCode(value) : ValueObjectErrors.IllegalCountryCodeValue(value);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    private static bool LegalCountryCodeValue(string value) =>
        value.Length == FixedLengthInChars && value.All(char.IsAsciiLetterUpper);
}

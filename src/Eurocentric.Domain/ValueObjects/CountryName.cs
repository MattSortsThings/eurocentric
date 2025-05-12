using ErrorOr;
using Eurocentric.Domain.Abstractions;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Contains a country's short UK English name.
/// </summary>
public sealed class CountryName : ValueObject
{
    private const int MaxPermittedLengthInChars = 200;

    private CountryName(string value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying string value of this instance.
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///     Creates and returns a new <see cref="CountryName" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <remarks>
    ///     A <see cref="CountryName" /> instance created using this method is guaranteed to be a legal country name in the
    ///     system. A legal country name value is a non-empty, non-whitespace string of no more than 200 characters.
    /// </remarks>
    /// <param name="value">
    ///     A non-empty, non-whitespace string of no more than 200 characters. The underlying value of the instance to be
    ///     created.
    /// </param>
    /// <returns>
    ///     A new <see cref="CountryName" /> instance if the <paramref name="value" /> parameter is a legal country name
    ///     value; otherwise, a list of <see cref="Error" /> values.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static ErrorOr<CountryName> FromValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return LegalCountryNameValue(value) ? new CountryName(value) : ValueObjectErrors.IllegalCountryNameValue(value);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    private static bool LegalCountryNameValue(string value) =>
        !string.IsNullOrWhiteSpace(value) && value.Length <= MaxPermittedLengthInChars;
}

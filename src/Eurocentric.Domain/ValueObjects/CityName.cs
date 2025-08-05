using ErrorOr;
using Eurocentric.Domain.Abstractions;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Represents a city's short UK English name.
/// </summary>
public sealed class CityName : ValueObject, IComparable<CityName>
{
    private const int MaxPermittedLengthInChars = 200;

    private CityName(string value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying string value of this instance.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public int CompareTo(CityName? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null ? 1 : string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    /// <summary>
    ///     Creates and returns a new <see cref="CityName" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <remarks>
    ///     A <see cref="CityName" /> instance created using this method is guaranteed to be a legal city name in the
    ///     domain. A legal city name value is a non-empty, non-whitespace string of no more than 200 characters.
    /// </remarks>
    /// <param name="value">
    ///     A non-empty, non-whitespace string of no more than 200 characters. The underlying value of the instance to be
    ///     created.
    /// </param>
    /// <returns>
    ///     A new <see cref="CityName" /> instance if the <paramref name="value" /> parameter is a legal city name
    ///     value; otherwise, a list of <see cref="Error" /> values.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static ErrorOr<CityName> FromValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return LegalCityNameValue(value) ? new CityName(value) : ValueObjectErrors.IllegalCityNameValue(value);
    }

    private protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    private static bool LegalCityNameValue(string value) =>
        !string.IsNullOrWhiteSpace(value) && value.Length <= MaxPermittedLengthInChars;
}

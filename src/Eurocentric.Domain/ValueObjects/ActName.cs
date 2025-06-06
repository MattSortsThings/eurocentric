using ErrorOr;
using Eurocentric.Domain.Abstractions;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Contains an act's performing name.
/// </summary>
public sealed class ActName : ValueObject, IComparable<ActName>
{
    private const int MaxPermittedLengthInChars = 200;

    private ActName(string value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying string value of this instance.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public int CompareTo(ActName? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null ? 1 : string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    /// <summary>
    ///     Creates and returns a new <see cref="ActName" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <remarks>
    ///     A <see cref="ActName" /> instance created using this method is guaranteed to be a legal act name in the
    ///     system. A legal act name value is a non-empty, non-whitespace string of no more than 200 characters.
    /// </remarks>
    /// <param name="value">
    ///     A non-empty, non-whitespace string of no more than 200 characters. The underlying value of the instance to be
    ///     created.
    /// </param>
    /// <returns>
    ///     A new <see cref="ActName" /> instance if the <paramref name="value" /> parameter is a legal act name
    ///     value; otherwise, a list of <see cref="Error" /> values.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static ErrorOr<ActName> FromValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return LegalActNameValue(value) ? new ActName(value) : ValueObjectErrors.IllegalActNameValue(value);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    private static bool LegalActNameValue(string value) =>
        !string.IsNullOrWhiteSpace(value) && value.Length <= MaxPermittedLengthInChars;
}

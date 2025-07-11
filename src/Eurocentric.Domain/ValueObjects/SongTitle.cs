using ErrorOr;
using Eurocentric.Domain.Abstractions;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Represents a song's title.
/// </summary>
public sealed class SongTitle : ValueObject, IComparable<SongTitle>
{
    private const int MaxPermittedLengthInChars = 200;

    private SongTitle(string value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the underlying string value of this instance.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public int CompareTo(SongTitle? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return other is null ? 1 : string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    /// <summary>
    ///     Creates and returns a new <see cref="SongTitle" /> instance with the provided <see cref="Value" />.
    /// </summary>
    /// <remarks>
    ///     A <see cref="SongTitle" /> instance created using this method is guaranteed to be a legal song title in the
    ///     domain. A legal song title value is a non-empty, non-whitespace string of no more than 200 characters.
    /// </remarks>
    /// <param name="value">
    ///     A non-empty, non-whitespace string of no more than 200 characters. The underlying value of the instance to be
    ///     created.
    /// </param>
    /// <returns>
    ///     A new <see cref="SongTitle" /> instance if the <paramref name="value" /> parameter is a legal song title
    ///     value; otherwise, a list of <see cref="Error" /> values.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static ErrorOr<SongTitle> FromValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return LegalSongTitleValue(value) ? new SongTitle(value) : ValueObjectErrors.IllegalSongTitleValue(value);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    private static bool LegalSongTitleValue(string value) =>
        !string.IsNullOrWhiteSpace(value) && value.Length <= MaxPermittedLengthInChars;
}

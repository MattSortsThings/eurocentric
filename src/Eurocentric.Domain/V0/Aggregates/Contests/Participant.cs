using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Aggregates.Contests;

/// <summary>
///     Represents a participant in a contest.
/// </summary>
public sealed record Participant
{
    /// <summary>
    ///     Gets the ID of the participating country.
    /// </summary>
    public Guid ParticipatingCountryId { get; init; }

    /// <summary>
    ///     Gets the participant's semi-final draw in their contest.
    /// </summary>
    public SemiFinalDraw SemiFinalDraw { get; init; }

    /// <summary>
    ///     Gets the participant's act name.
    /// </summary>
    public string ActName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the participant's song title.
    /// </summary>
    public string SongTitle { get; init; } = string.Empty;
}

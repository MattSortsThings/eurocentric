using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0.Contests;

/// <summary>
///     Represents a participant in a contest.
/// </summary>
public sealed class Participant
{
    /// <summary>
    ///     Gets or initializes the participant's participating country ID.
    /// </summary>
    public required Guid ParticipatingCountryId { get; init; }

    /// <summary>
    ///     Gets or initializes the participant's Semi-Final draw.
    /// </summary>
    public required SemiFinalDraw SemiFinalDraw { get; init; }

    /// <summary>
    ///     Gets or initializes the participant's act name.
    /// </summary>
    public required string ActName { get; init; }

    /// <summary>
    ///     Gets or initializes the participant's song title.
    /// </summary>
    public required string SongTitle { get; init; }
}

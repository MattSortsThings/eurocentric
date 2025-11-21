using Eurocentric.Apis.Admin.V1.Enums;

namespace Eurocentric.Apis.Admin.V1.Dtos.Contests;

/// <summary>
///     Represents a participant in a contest.
/// </summary>
public sealed record Participant
{
    /// <summary>
    ///     The ID of the participating country.
    /// </summary>
    public Guid ParticipatingCountryId { get; init; }

    /// <summary>
    ///     The participant's semi-final draw.
    /// </summary>
    public SemiFinalDraw SemiFinalDraw { get; init; }

    /// <summary>
    ///     The participant's act name.
    /// </summary>
    public string ActName { get; init; } = string.Empty;

    /// <summary>
    ///     The participant's song title.
    /// </summary>
    public string SongTitle { get; init; } = string.Empty;
}

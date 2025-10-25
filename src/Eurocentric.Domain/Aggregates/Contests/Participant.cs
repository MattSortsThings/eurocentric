using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a participant in a contest.
/// </summary>
public sealed class Participant : Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private Participant() { }

    internal Participant(
        CountryId participatingCountryId,
        SemiFinalDraw semiFinalDraw,
        ActName actName,
        SongTitle songTitle
    )
    {
        ParticipatingCountryId = participatingCountryId;
        SemiFinalDraw = semiFinalDraw;
        ActName = actName;
        SongTitle = songTitle;
    }

    /// <summary>
    ///     Gets the ID of the participating country.
    /// </summary>
    public CountryId ParticipatingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets the participant's semi-final draw.
    /// </summary>
    public SemiFinalDraw SemiFinalDraw { get; private init; }

    /// <summary>
    ///     Gets the participant's act name.
    /// </summary>
    public ActName ActName { get; private init; } = null!;

    /// <summary>
    ///     Gets the participant's song title.
    /// </summary>
    public SongTitle SongTitle { get; private init; } = null!;
}

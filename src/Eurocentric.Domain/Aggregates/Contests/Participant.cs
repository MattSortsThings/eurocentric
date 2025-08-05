using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a country participating in a contest.
/// </summary>
public sealed class Participant : Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private Participant()
    {
    }

    public Participant(CountryId participatingCountryId,
        ParticipantGroup participantGroup = ParticipantGroup.Zero,
        ActName? actName = null,
        SongTitle? songTitle = null)
    {
        ParticipatingCountryId = participatingCountryId;
        ParticipantGroup = participantGroup;
        ActName = actName;
        SongTitle = songTitle;
    }

    /// <summary>
    ///     Gets the ID of the country aggregate the participant represents.
    /// </summary>
    public CountryId ParticipatingCountryId { get; init; } = null!;

    /// <summary>
    ///     Gets the participant's group in its contest.
    /// </summary>
    public ParticipantGroup ParticipantGroup { get; init; }

    /// <summary>
    ///     Gets the act name for the participant.
    /// </summary>
    public ActName? ActName { get; init; }

    /// <summary>
    ///     Gets the song title for the participant.
    /// </summary>
    public SongTitle? SongTitle { get; init; }
}

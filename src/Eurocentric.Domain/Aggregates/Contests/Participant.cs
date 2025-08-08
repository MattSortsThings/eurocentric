using ErrorOr;
using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
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

    internal Competitor CreateCompetitor(int runningOrderPosition) => new(ParticipatingCountryId, runningOrderPosition);

    internal Jury CreateJury() => new(ParticipatingCountryId);

    internal Televote CreateTelevote() => new(ParticipatingCountryId);

    internal static Participant CreateInGroup0(CountryId participatingCountryId) => new(participatingCountryId);

    internal static ErrorOr<Participant> CreateInGroup1(CountryId participatingCountryId,
        ErrorOr<ActName> errorsOrActName,
        ErrorOr<SongTitle> errorsOrSongTitle) => Tuple.Create(errorsOrActName, errorsOrSongTitle)
        .Combine()
        .Then(tuple => new Participant(participatingCountryId, ParticipantGroup.One, tuple.Item1, tuple.Item2));

    internal static ErrorOr<Participant> CreateInGroup2(CountryId participatingCountryId,
        ErrorOr<ActName> errorsOrActName,
        ErrorOr<SongTitle> errorsOrSongTitle) => Tuple.Create(errorsOrActName, errorsOrSongTitle)
        .Combine()
        .Then(tuple => new Participant(participatingCountryId, ParticipantGroup.Two, tuple.Item1, tuple.Item2));
}

using ErrorOr;
using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

/// <summary>
///     Represents a single participant in a contest aggregate.
/// </summary>
public sealed class Participant : Entity
{
    private Participant() { }

    private Participant(CountryId countryId,
        ParticipantGroup participantGroup = ParticipantGroup.Zero,
        ActName? actName = null,
        SongTitle? songTitle = null)
    {
        ParticipatingCountryId = countryId;
        ParticipantGroup = participantGroup;
        ActName = actName;
        SongTitle = songTitle;
    }

    /// <summary>
    ///     Gets the ID of the country aggregate that the participant represents.
    /// </summary>
    public CountryId ParticipatingCountryId { get; } = null!;

    /// <summary>
    ///     Gets the participant's group in the contest.
    /// </summary>
    public ParticipantGroup ParticipantGroup { get; private init; }

    /// <summary>
    ///     Gets the participant's act name.
    /// </summary>
    /// <remarks>
    ///     The value of this property is <see langword="null" /> when the value of
    ///     <see cref="Participant.ParticipantGroup" /> is <see cref="ParticipantGroup.Zero" />, otherwise it is not
    ///     <see langword="null" />.
    /// </remarks>
    public ActName? ActName { get; private init; }

    /// <summary>
    ///     Gets the participant's song title.
    /// </summary>
    /// <remarks>
    ///     The value of this property is <see langword="null" /> when the value of
    ///     <see cref="Participant.ParticipantGroup" /> is <see cref="ParticipantGroup.Zero" />, otherwise it is not
    ///     <see langword="null" />.
    /// </remarks>
    public SongTitle? SongTitle { get; private init; }

    internal Competitor CreateCompetitor(int runningOrderPosition) => new(ParticipatingCountryId, runningOrderPosition);

    internal Jury CreateJury() => new(ParticipatingCountryId);

    internal Televote CreateTelevote() => new(ParticipatingCountryId);

    internal static Participant CreateInGroup0(CountryId countryId) => new(countryId);

    internal static ErrorOr<Participant> CreateInGroup1(CountryId countryId,
        ErrorOr<ActName> errorsOrActName,
        ErrorOr<SongTitle> errorsOrSongTitle) => Tuple.Create(errorsOrActName, errorsOrSongTitle)
        .Combine()
        .Then(tuple => new Participant(countryId, ParticipantGroup.One, tuple.Item1, tuple.Item2));

    internal static ErrorOr<Participant> CreateInGroup2(CountryId countryId,
        ErrorOr<ActName> errorsOrActName,
        ErrorOr<SongTitle> errorsOrSongTitle) => Tuple.Create(errorsOrActName, errorsOrSongTitle)
        .Combine()
        .Then(tuple => new Participant(countryId, ParticipantGroup.Two, tuple.Item1, tuple.Item2));
}

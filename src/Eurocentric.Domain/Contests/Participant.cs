using ErrorOr;
using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

/// <summary>
///     Represents a participant in a contest.
/// </summary>
public sealed class Participant : Entity
{
    private Participant()
    {
    }

    private Participant(CountryId participatingCountryId,
        ParticipantGroup group,
        ActName? actName = null,
        SongTitle? songTitle = null)
    {
        ParticipatingCountryId = participatingCountryId;
        Group = group;
        ActName = actName;
        SongTitle = songTitle;
    }

    /// <summary>
    ///     Gets the ID of the country aggregate represented by the participant.
    /// </summary>
    public CountryId ParticipatingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets the participant's group in its contest aggregate.
    /// </summary>
    public ParticipantGroup Group { get; private init; }

    /// <summary>
    ///     Gets the participant's act name.
    /// </summary>
    /// <remarks>
    ///     The value of this property is <see langword="null" /> when the value of <see cref="Group" /> is
    ///     <see cref="ParticipantGroup.Zero" />; otherwise, it is not <see langword="null" />.
    /// </remarks>
    public ActName? ActName { get; private init; }

    /// <summary>
    ///     Gets the participant's song title.
    /// </summary>
    /// <remarks>
    ///     The value of this property is <see langword="null" /> when the value of <see cref="Group" /> is
    ///     <see cref="ParticipantGroup.Zero" />; otherwise, it is not <see langword="null" />.
    /// </remarks>
    public SongTitle? SongTitle { get; private init; }

    /// <summary>
    ///     Creates and returns a new <see cref="Participant" /> instance in participant group
    ///     <see cref="ParticipantGroup.Zero" />.
    /// </summary>
    /// <param name="countryId">The participating country ID.</param>
    /// <exception cref="ArgumentNullException"><paramref name="countryId" /> is <see langword="null" />.</exception>
    /// <returns>A new <see cref="Participant" /> instance.</returns>
    public static Participant CreateInGroupZero(CountryId countryId)
    {
        ArgumentNullException.ThrowIfNull(countryId);

        return new Participant(countryId, ParticipantGroup.Zero);
    }

    /// <summary>
    ///     Creates and returns a new <see cref="Participant" /> instance in participant group
    ///     <see cref="ParticipantGroup.One" />.
    /// </summary>
    /// <param name="countryId">The participating country ID.</param>
    /// <param name="errorsOrActName">
    ///     The discriminated union of a legal <see cref="ActName" /> value or a list of <see cref="Error" /> values.
    /// </param>
    /// <param name="errorsOrSongTitle">
    ///     The discriminated union of a legal <see cref="SongTitle" /> value or a list of <see cref="Error" /> values.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="countryId" /> is <see langword="null" />.</exception>
    /// <returns>A new <see cref="Participant" /> instance; or a list of <see cref="Error" /> values.</returns>
    public static ErrorOr<Participant> CreateInGroupOne(CountryId countryId, ErrorOr<ActName> errorsOrActName,
        ErrorOr<SongTitle> errorsOrSongTitle)
    {
        ArgumentNullException.ThrowIfNull(countryId);

        return (errorsOrActName, errorsOrSongTitle)
            .Combine()
            .Then(tuple => new Participant(countryId, ParticipantGroup.One, tuple.First, tuple.Second));
    }

    /// <summary>
    ///     Creates and returns a new <see cref="Participant" /> instance in participant group
    ///     <see cref="ParticipantGroup.Two" />.
    /// </summary>
    /// <param name="countryId">The participating country ID.</param>
    /// <param name="errorsOrActName">
    ///     The discriminated union of a legal <see cref="ActName" /> value or a list of <see cref="Error" /> values.
    /// </param>
    /// <param name="errorsOrSongTitle">
    ///     The discriminated union of a legal <see cref="SongTitle" /> value or a list of <see cref="Error" /> values.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="countryId" /> is <see langword="null" />.</exception>
    /// <returns>A new <see cref="Participant" /> instance; or a list of <see cref="Error" /> values.</returns>
    public static ErrorOr<Participant> CreateInGroupTwo(CountryId countryId, ErrorOr<ActName> errorsOrActName,
        ErrorOr<SongTitle> errorsOrSongTitle)
    {
        ArgumentNullException.ThrowIfNull(countryId);

        return (errorsOrActName, errorsOrSongTitle)
            .Combine()
            .Then(tuple => new Participant(countryId, ParticipantGroup.Two, tuple.First, tuple.Second));
    }
}

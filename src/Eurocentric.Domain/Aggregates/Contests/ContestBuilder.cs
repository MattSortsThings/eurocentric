using ErrorOr;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Fluent builder for the <see cref="Contest" /> class.
/// </summary>
/// <remarks>
///     The client will work with a specific concrete derivative of this abstract class depending on the
///     <see cref="Contest" /> derivative type to be built.
/// </remarks>
public abstract class ContestBuilder
{
    private protected ErrorOr<ContestYear> ErrorsOrContestYear { get; private set; } = ContestErrors.ContestYearNotSet();

    private protected ErrorOr<CityName> ErrorsOrCityName { get; private set; } = ContestErrors.CityNameNotSet();

    private protected List<ErrorOr<Participant>> ErrorsOrParticipants { get; } = new(7);

    /// <summary>
    ///     Sets the <see cref="Contest.ContestYear" /> value of the <see cref="Contest" /> to be built.
    /// </summary>
    /// <param name="errorsOrContestYear">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects
    ///     <i>OR</i> a <see cref="ContestYear" /> value object.
    /// </param>
    /// <returns>The same fluent builder instance, so that method invocations can be chained.</returns>
    public ContestBuilder WithContestYear(ErrorOr<ContestYear> errorsOrContestYear)
    {
        ErrorsOrContestYear = errorsOrContestYear;

        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Contest.CityName" /> value of the <see cref="Contest" /> to be built.
    /// </summary>
    /// <param name="errorsOrCityName">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects
    ///     <i>OR</i> a <see cref="CityName" /> value object.
    /// </param>
    /// <returns>The same fluent builder instance, so that method invocations can be chained.</returns>
    public ContestBuilder WithCityName(ErrorOr<CityName> errorsOrCityName)
    {
        ErrorsOrCityName = errorsOrCityName;

        return this;
    }

    /// <summary>
    ///     Adds a <see cref="Participant" /> in participant group <see cref="ParticipantGroup.Zero" /> to the
    ///     <see cref="Contest.Participants" /> collection of the <see cref="Contest" /> to be built.
    /// </summary>
    /// <param name="participatingCountryId">The ID of the participating country.</param>
    /// <returns>The same fluent builder instance, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="participatingCountryId" /> is <see langword="null" />.</exception>
    public ContestBuilder AddGroup0Participant(CountryId participatingCountryId)
    {
        ArgumentNullException.ThrowIfNull(participatingCountryId);

        ErrorsOrParticipants.Add(Participant.CreateInGroup0(participatingCountryId));

        return this;
    }

    /// <summary>
    ///     Adds a <see cref="Participant" /> in participant group <see cref="ParticipantGroup.One" /> to the
    ///     <see cref="Contest.Participants" /> collection of the <see cref="Contest" /> to be built.
    /// </summary>
    /// <param name="participatingCountryId">The ID of the participating country.</param>
    /// <param name="errorsOrActName">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> an <see cref="ActName" />
    ///     value object. The act name of the participant.
    /// </param>
    /// <param name="errorsOrSongTitle">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a
    ///     <see cref="SongTitle" /> value object. The song title of the participant.
    /// </param>
    /// <returns>The same fluent builder instance, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="participatingCountryId" /> is <see langword="null" />.</exception>
    public ContestBuilder AddGroup1Participant(CountryId participatingCountryId, ErrorOr<ActName> errorsOrActName,
        ErrorOr<SongTitle> errorsOrSongTitle)
    {
        ArgumentNullException.ThrowIfNull(participatingCountryId);

        ErrorsOrParticipants.Add(Participant.CreateInGroup1(participatingCountryId, errorsOrActName, errorsOrSongTitle));

        return this;
    }

    /// <summary>
    ///     Adds a <see cref="Participant" /> in participant group <see cref="ParticipantGroup.Two" /> to the
    ///     <see cref="Contest.Participants" /> collection of the <see cref="Contest" /> to be built.
    /// </summary>
    /// <param name="participatingCountryId">The ID of the participating country.</param>
    /// <param name="errorsOrActName">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> an <see cref="ActName" />
    ///     value object. The act name of the participant.
    /// </param>
    /// <param name="errorsOrSongTitle">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a
    ///     <see cref="SongTitle" /> value object. The song title of the participant.
    /// </param>
    /// <returns>The same fluent builder instance, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="participatingCountryId" /> is <see langword="null" />.</exception>
    public ContestBuilder AddGroup2Participant(CountryId participatingCountryId, ErrorOr<ActName> errorsOrActName,
        ErrorOr<SongTitle> errorsOrSongTitle)
    {
        ArgumentNullException.ThrowIfNull(participatingCountryId);

        ErrorsOrParticipants.Add(Participant.CreateInGroup2(participatingCountryId, errorsOrActName, errorsOrSongTitle));

        return this;
    }

    /// <summary>
    ///     Creates and returns a new <see cref="Contest" /> instance as specified by the previous fluent builder method
    ///     invocations.
    /// </summary>
    /// <param name="idProvider">
    ///     Provides the <see cref="Contest.Id" /> of the new <see cref="Contest" /> if it is successfully instantiated.
    /// </param>
    /// <returns>
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a new
    ///     <see cref="Contest" /> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="idProvider" /> is <see langword="null" />.</exception>
    public abstract ErrorOr<Contest> Build(Func<ContestId> idProvider);

    private protected static bool DuplicateParticipatingCountryIds(IList<Participant> participants) =>
        participants.GroupBy(participant => participant.ParticipatingCountryId)
            .Any(grouping => grouping.Count() > 1);
}

using ErrorOr;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Extension methods to enforce invariants across all existing <see cref="Contest" /> aggregates.
/// </summary>
public static class InvariantEnforcement
{
    /// <summary>
    ///     Fails if the newly instantiated <see cref="Contest" /> aggregate has the same <see cref="Contest.ContestYear" />
    ///     value as an existing <see cref="Contest" /> aggregate.
    /// </summary>
    /// <param name="errorsOrContest">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i>
    ///     a <see cref="Contest" /> object.
    /// </param>
    /// <param name="existingContests">All the existing <see cref="Contest" /> aggregates in the system.</param>
    /// <returns>
    ///     A new list of <see cref="Error" /> objects if the <paramref name="errorsOrContest" /> argument is a
    ///     <see cref="Contest" /> and its <see cref="Contest.ContestYear" /> is not unique; otherwise, the
    ///     <paramref name="errorsOrContest" /> argument is returned.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="existingContests" /> is <see langword="null" />.</exception>
    public static ErrorOr<Contest> FailOnContestYearConflict(this ErrorOr<Contest> errorsOrContest,
        IQueryable<Contest> existingContests)
    {
        ArgumentNullException.ThrowIfNull(existingContests);

        if (errorsOrContest.IsError)
        {
            return errorsOrContest;
        }

        ContestYear contestYear = errorsOrContest.Value.ContestYear;

        return existingContests.Any(contest => contest.ContestYear == contestYear)
            ? ContestErrors.ContestYearConflict(contestYear)
            : errorsOrContest;
    }

    /// <summary>
    ///     Fails if the newly instantiated <see cref="Contest" /> aggregate has a <see cref="Participant" /> with a
    ///     <see cref="Participant.ParticipatingCountryId" /> value with no matching existing <see cref="Country" /> aggregate.
    /// </summary>
    /// <param name="errorsOrContest">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i>
    ///     a <see cref="Contest" /> object.
    /// </param>
    /// <param name="existingCountries">All the existing <see cref="Country" /> aggregates in the system.</param>
    /// <returns>
    ///     A new list of <see cref="Error" /> objects if the <paramref name="errorsOrContest" /> argument is a
    ///     <see cref="Contest" /> and any of its <see cref="Contest.Participants" /> has an orphan
    ///     <see cref="Participant.ParticipatingCountryId" />; otherwise, the <paramref name="errorsOrContest" /> argument is
    ///     returned.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="existingCountries" /> is <see langword="null" />.</exception>
    public static ErrorOr<Contest> FailOnOrphanParticipatingCountryId(this ErrorOr<Contest> errorsOrContest,
        IQueryable<Country> existingCountries)
    {
        ArgumentNullException.ThrowIfNull(existingCountries);

        if (errorsOrContest.IsError)
        {
            return errorsOrContest;
        }

        IEnumerable<CountryId> participatingCountryIds =
            errorsOrContest.Value.Participants.Select(participant => participant.ParticipatingCountryId);

        CountryId[] orphanIds = participatingCountryIds.Except(existingCountries.Select(country => country.Id)).ToArray();

        return orphanIds.Length > 0
            ? orphanIds.Select(ContestErrors.OrphanParticipatingCountryId).ToList()
            : errorsOrContest;
    }
}

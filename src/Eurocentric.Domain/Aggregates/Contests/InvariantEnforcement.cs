using ErrorOr;
using Eurocentric.Domain.Aggregates.Countries;
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
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a <see cref="Contest" />
    ///     object.
    /// </param>
    /// <param name="existingContests">All the existing <see cref="Contest" /> aggregates in the system.</param>
    /// <returns>
    ///     A new list of <see cref="Error" /> objects if the <paramref name="errorsOrContest" /> argument is a
    ///     <see cref="Contest" /> and its <see cref="Contest.ContestYear" /> has the same value as a <see cref="Contest" /> in
    ///     <paramref name="existingContests" />; otherwise, the <paramref name="errorsOrContest" /> argument is returned.
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
    ///     Fails if the newly instantiated <see cref="Contest" /> aggregate has one or more
    ///     <see cref="Contest.Participants" /> with a <see cref="Participant.ParticipatingCountryId" /> value that does not
    ///     reference an existing <see cref="Country" /> aggregate.
    /// </summary>
    /// <param name="errorsOrContest">
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a <see cref="Contest" />
    ///     object.
    /// </param>
    /// <param name="existingCountries">All the existing <see cref="Country" /> aggregates in the system.</param>
    /// <returns>
    ///     A new list of <see cref="Error" /> objects if the <paramref name="errorsOrContest" /> argument is a
    ///     <see cref="Contest" /> and any of its <see cref="Contest.Participants" /> has no matching <see cref="Country" /> in
    ///     <paramref name="existingCountries" />; otherwise, the <paramref name="errorsOrContest" /> argument is returned.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="existingCountries" /> is <see langword="null" />.</exception>
    public static ErrorOr<Contest> FailOnParticipatingCountryNotFound(this ErrorOr<Contest> errorsOrContest,
        IQueryable<Country> existingCountries)
    {
        ArgumentNullException.ThrowIfNull(existingCountries);

        if (errorsOrContest.IsError)
        {
            return errorsOrContest;
        }

        List<Error> errors = errorsOrContest.Value.Participants.Select(participant => participant.ParticipatingCountryId)
            .Where(id => !existingCountries.Any(existingCountry => existingCountry.Id == id))
            .Select(CountryErrors.CountryNotFound)
            .ToList();

        return errors.Count != 0 ? errors : errorsOrContest;
    }
}

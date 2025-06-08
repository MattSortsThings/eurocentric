using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

public static class InvariantEnforcement
{
    public static ErrorOr<Contest> FailOnContestYearConflict(this ErrorOr<Contest> errorsOrContest,
        IQueryable<Contest> existingContests)
    {
        if (errorsOrContest.IsError)
        {
            return errorsOrContest;
        }

        ContestYear contestYear = errorsOrContest.Value.ContestYear;

        return existingContests.Any(contest => contest.ContestYear == contestYear)
            ? ContestErrors.ContestYearConflict(contestYear)
            : errorsOrContest;
    }

    public static ErrorOr<Contest> FailOnOrphanParticipant(this ErrorOr<Contest> errorsOrContest,
        IQueryable<Country> existingCountries)
    {
        if (errorsOrContest.IsError)
        {
            return errorsOrContest;
        }

        CountryId? orphanCountryId = errorsOrContest.Value.Participants
            .Select(participant => participant.ParticipatingCountryId)
            .Except(existingCountries.Select(existingCountry => existingCountry.Id))
            .FirstOrDefault();

        return orphanCountryId is not null
            ? ContestErrors.OrphanParticipant(orphanCountryId)
            : errorsOrContest;
    }
}

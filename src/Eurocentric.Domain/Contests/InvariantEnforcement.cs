using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Contests;

public static class InvariantEnforcement
{
    public static ErrorOr<Contest> FailIfContestYearIsNotUnique(this ErrorOr<Contest> errorsOrContest,
        IQueryable<Contest> existingContests)
    {
        if (errorsOrContest.IsError)
        {
            return errorsOrContest;
        }

        if (errorsOrContest.Value.Year is { } contestYear && existingContests.Any(x => x.Year == contestYear))
        {
            return ContestErrors.ContestYearConflict(contestYear);
        }

        return errorsOrContest;
    }

    public static ErrorOr<Contest> FailIfOrphanParticipatingCountryIds(this ErrorOr<Contest> errorsOrContest,
        IQueryable<Country> existingCountries)
    {
        if (errorsOrContest.IsError)
        {
            return errorsOrContest;
        }

        CountryId[] orphanCountryIds = errorsOrContest.Value.Participants
            .Select(participant => participant.ParticipatingCountryId)
            .Except(existingCountries.Select(x => x.Id))
            .ToArray();

        return orphanCountryIds.Length != 0 ? ContestErrors.OrphanParticipatingCountryIds(orphanCountryIds) : errorsOrContest;
    }
}

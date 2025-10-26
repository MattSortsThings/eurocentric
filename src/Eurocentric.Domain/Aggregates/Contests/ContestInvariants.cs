using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Static methods for enforcing country aggregate invariants.
/// </summary>
public static class ContestInvariants
{
    public static Func<Contest, UnitResult<IDomainError>> HasNoOrphanContestCountries(
        IQueryable<Country> existingCountries
    )
    {
        IQueryable<Country> countries = existingCountries;

        return contest =>
        {
            CountryId? orphanCountryId = ExtractAllCountryIds(contest)
                .FirstOrDefault(countryId => !countries.Any(country => country.Id.Equals(countryId)));

            return orphanCountryId is not null
                ? ContestErrors.OrphanContestCountry(orphanCountryId)
                : UnitResult.Success<IDomainError>();
        };
    }

    public static Func<Contest, UnitResult<IDomainError>> HasUniqueContestYear(IQueryable<Contest> existingContests)
    {
        IQueryable<Contest> contests = existingContests;

        return contest =>
        {
            ContestYear contestYear = contest.ContestYear;

            return contests.Any(existingContest => existingContest.ContestYear.Equals(contestYear))
                ? ContestErrors.ContestYearConflict(contestYear)
                : UnitResult.Success<IDomainError>();
        };
    }

    public static UnitResult<IDomainError> HasLegalContestCountries(Contest contest)
    {
        return ExtractAllCountryIds(contest).GroupBy(id => id).Any(grouping => grouping.Count() > 1)
            ? ContestErrors.IllegalContestCountries()
            : UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> HasLegalGlobalTelevote(LiverpoolRulesContest contest)
    {
        return contest.GlobalTelevote is null
            ? ContestErrors.IllegalGlobalTelevote()
            : UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> HasLegalGlobalTelevote(StockholmRulesContest contest)
    {
        return contest.GlobalTelevote is not null
            ? ContestErrors.IllegalGlobalTelevote()
            : UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> HasLegalParticipantCounts(Contest contest)
    {
        Dictionary<SemiFinalDraw, int> semiFinalDrawCounts = contest
            .ParticipantsCollection.GroupBy(participant => participant.SemiFinalDraw)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());

        return
            semiFinalDrawCounts.Count < Enum.GetValues<SemiFinalDraw>().Length
            || semiFinalDrawCounts.Values.Any(count => count < 3)
            ? ContestErrors.IllegalParticipantCounts()
            : UnitResult.Success<IDomainError>();
    }

    private static IEnumerable<CountryId> ExtractAllCountryIds(Contest contest)
    {
        return contest.GlobalTelevote is not { } globalTelevote
            ? contest.Participants.Select(participant => participant.ParticipatingCountryId)
            : contest
                .Participants.Select(participant => participant.ParticipatingCountryId)
                .Append(globalTelevote.VotingCountryId);
    }
}

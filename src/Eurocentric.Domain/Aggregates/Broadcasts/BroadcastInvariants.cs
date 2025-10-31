using System.ComponentModel;
using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

public static class BroadcastInvariants
{
    public static UnitResult<IDomainError> LegalCompetitorsCount(Broadcast broadcast)
    {
        return broadcast.CompetitorsCollection.Count < 2
            ? BroadcastErrors.IllegalCompetitorsCount()
            : UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> LegalCompetingCountries(Broadcast broadcast)
    {
        return broadcast
            .CompetitorsCollection.GroupBy(competitor => competitor.CompetingCountryId)
            .Any(grouping => grouping.Count() > 1)
            ? BroadcastErrors.IllegalCompetingCountries()
            : UnitResult.Success<IDomainError>();
    }

    public static Func<Broadcast, UnitResult<IDomainError>> HasUniqueBroadcastDate(
        IQueryable<Broadcast> existingBroadcasts
    )
    {
        IQueryable<Broadcast> broadcasts = existingBroadcasts;

        return broadcast =>
        {
            BroadcastDate broadcastDate = broadcast.BroadcastDate;

            return broadcasts.Any(existingBroadcast => existingBroadcast.BroadcastDate.Equals(broadcastDate))
                ? BroadcastErrors.BroadcastDateConflict(broadcastDate)
                : UnitResult.Success<IDomainError>();
        };
    }

    public static Func<Broadcast, UnitResult<IDomainError>> HasUniqueContestStageForParentContest(Contest contest)
    {
        Contest parentContest = contest;

        return broadcast =>
        {
            ContestStage contestStage = broadcast.ContestStage;

            return contest.ChildBroadcasts.Any(existingBroadcast => existingBroadcast.ContestStage == contestStage)
                ? BroadcastErrors.ParentContestChildBroadcastsConflict(parentContest.Id, contestStage)
                : UnitResult.Success<IDomainError>();
        };
    }

    public static Func<Broadcast, UnitResult<IDomainError>> BroadcastDateMatchesParentContestYear(Contest contest)
    {
        Contest parentContest = contest;

        return broadcast =>
        {
            BroadcastDate broadcastDate = broadcast.BroadcastDate;

            return broadcastDate.IsIn(parentContest.ContestYear)
                ? UnitResult.Success<IDomainError>()
                : BroadcastErrors.ParentContestYearConflict(parentContest.Id, broadcastDate);
        };
    }

    public static Func<Broadcast, UnitResult<IDomainError>> EveryCompetitorMatchesEligibleParticipantInParentContest(
        Contest contest
    )
    {
        Contest parentContest = contest;

        return broadcast =>
        {
            ContestStage contestStage = broadcast.ContestStage;
            Func<Participant, bool> eligibleToCompete = contestStage.ToEligibilityPredicate();

            CountryId? illegalCountryId = broadcast
                .CompetitorsCollection.Select(competitor => competitor.CompetingCountryId)
                .Except(
                    parentContest
                        .ParticipantsCollection.Where(eligibleToCompete)
                        .Select(participant => participant.ParticipatingCountryId)
                )
                .FirstOrDefault();

            return illegalCountryId is not null
                ? BroadcastErrors.ParentContestParticipantsConflict(parentContest.Id, contestStage, illegalCountryId)
                : UnitResult.Success<IDomainError>();
        };
    }

    private static Func<Participant, bool> ToEligibilityPredicate(this ContestStage contestStage)
    {
        return contestStage switch
        {
            ContestStage.SemiFinal1 => participant => participant.SemiFinalDraw == SemiFinalDraw.SemiFinal1,
            ContestStage.SemiFinal2 => participant => participant.SemiFinalDraw == SemiFinalDraw.SemiFinal2,
            ContestStage.GrandFinal => _ => true,
            _ => throw new InvalidEnumArgumentException(nameof(contestStage), (int)contestStage, typeof(ContestStage)),
        };
    }
}

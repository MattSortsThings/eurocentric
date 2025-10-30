using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest using the "Stockholm" contest rules.
/// </summary>
public sealed class StockholmRulesContest : Contest
{
    [UsedImplicitly(Reason = "EF Core")]
    private StockholmRulesContest() { }

    private StockholmRulesContest(
        ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants,
        GlobalTelevote? globalTelevote = null
    )
        : base(id, contestYear, cityName, participants, globalTelevote) { }

    public override ContestRules ContestRules { get; private protected init; } = ContestRules.Stockholm;

    public override IBroadcastBuilder CreateSemiFinal1Broadcast() => new SemiFinal1ChildBroadcastBuilder(this);

    public override IBroadcastBuilder CreateSemiFinal2Broadcast() => new SemiFinal2ChildBroadcastBuilder(this);

    public override IBroadcastBuilder CreateGrandFinalBroadcast() => new GrandFinalChildBroadcastBuilder(this);

    public static IContestBuilder Create() => new Builder();

    private sealed class Builder : ContestBuilder
    {
        public override Result<Contest, IDomainError> Build(Func<ContestId> idProvider)
        {
            ArgumentNullException.ThrowIfNull(idProvider);

            return ValueTuple
                .Create(ErrorOrContestYear, ErrorOrCityName, ErrorsOrParticipants.Collect())
                .Combine()
                .Map(InitializeWithDummyId)
                .Ensure(ContestInvariants.HasLegalGlobalTelevote)
                .Ensure(ContestInvariants.HasLegalContestCountries)
                .Ensure(ContestInvariants.HasLegalParticipantCounts)
                .Tap(contest => contest.Id = idProvider())
                .Map(Contest (contest) => contest);
        }

        private StockholmRulesContest InitializeWithDummyId(
            ValueTuple<ContestYear, CityName, List<Participant>> tuple
        ) => new(ContestId.FromValue(Guid.Empty), tuple.Item1, tuple.Item2, tuple.Item3, GlobalTelevote);
    }

    private sealed class SemiFinal1ChildBroadcastBuilder(Contest contest) : Broadcast.Builder
    {
        private protected override Contest ParentContest { get; } = contest;

        private protected override ContestStage ContestStage => ContestStage.SemiFinal1;

        private protected override List<Jury> CreateJuries()
        {
            return ParentContest
                .ParticipantsCollection.Where(participant => participant.SemiFinalDraw == SemiFinalDraw.SemiFinal1)
                .Select(participant => participant.CreateJury())
                .ToList();
        }

        private protected override List<Televote> CreateTelevotes()
        {
            return ParentContest
                .ParticipantsCollection.Where(participant => participant.SemiFinalDraw == SemiFinalDraw.SemiFinal1)
                .Select(participant => participant.CreateTelevote())
                .ToList();
        }
    }

    private sealed class SemiFinal2ChildBroadcastBuilder(Contest contest) : Broadcast.Builder
    {
        private protected override Contest ParentContest { get; } = contest;

        private protected override ContestStage ContestStage => ContestStage.SemiFinal2;

        private protected override List<Jury> CreateJuries()
        {
            return ParentContest
                .ParticipantsCollection.Where(participant => participant.SemiFinalDraw == SemiFinalDraw.SemiFinal2)
                .Select(participant => participant.CreateJury())
                .ToList();
        }

        private protected override List<Televote> CreateTelevotes()
        {
            return ParentContest
                .ParticipantsCollection.Where(participant => participant.SemiFinalDraw == SemiFinalDraw.SemiFinal2)
                .Select(participant => participant.CreateTelevote())
                .ToList();
        }
    }

    private sealed class GrandFinalChildBroadcastBuilder(Contest contest) : Broadcast.Builder
    {
        private protected override Contest ParentContest { get; } = contest;

        private protected override ContestStage ContestStage => ContestStage.GrandFinal;

        private protected override List<Jury> CreateJuries() =>
            ParentContest.ParticipantsCollection.Select(participant => participant.CreateJury()).ToList();

        private protected override List<Televote> CreateTelevotes() =>
            ParentContest.ParticipantsCollection.Select(participant => participant.CreateTelevote()).ToList();
    }
}

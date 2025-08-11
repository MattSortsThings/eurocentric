using ErrorOr;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest aggregate using the <see cref="Enums.ContestFormat.Liverpool" /> contest format.
/// </summary>
public sealed class LiverpoolFormatContest : Contest
{
    [UsedImplicitly(Reason = "EF Core")]
    private LiverpoolFormatContest()
    {
    }

    public LiverpoolFormatContest(ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants) : base(id, contestYear, cityName, participants)
    {
    }

    /// <inheritdoc />
    public override ContestFormat ContestFormat { get; private protected init; } = ContestFormat.Liverpool;

    private protected override ChildBroadcastBuilder InitializeSemiFinal1ChildBroadcastBuilder(Contest parentContest) =>
        new SemiFinal1Builder(parentContest);

    private protected override ChildBroadcastBuilder InitializeSemiFinal2ChildBroadcastBuilder(Contest parentContest) =>
        new SemiFinal2Builder(parentContest);

    private protected override ChildBroadcastBuilder InitializeGrandFinalChildBroadcastBuilder(Contest parentContest) =>
        new GrandFinalBuilder(parentContest);

    /// <summary>
    ///     Starts the process of creating a new <see cref="LiverpoolFormatContest" /> instance using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="ContestBuilder" /> instance.</returns>
    public static ContestBuilder Create() => new Builder();

    private sealed class Builder : ContestBuilder
    {
        public override ErrorOr<Contest> Build(Func<ContestId> idProvider)
        {
            ArgumentNullException.ThrowIfNull(idProvider);

            return Tuple.Create(ErrorsOrContestYear,
                    ErrorsOrCityName,
                    ErrorsOrParticipants.Collect()
                        .FailIf(DuplicateParticipatingCountryIds, ContestErrors.DuplicateParticipatingCountryIds())
                        .FailIf(IllegalParticipantGroupSizes, ContestErrors.IllegalLiverpoolFormatParticipantGroups()))
                .Combine()
                .Then(Contest (tuple) => new LiverpoolFormatContest(idProvider(), tuple.Item1, tuple.Item2, tuple.Item3))
                .ThenDo(contest => contest.AddDomainEvent(new ContestCreatedEvent(contest)))
                .Then(contest => contest);
        }

        private static bool IllegalParticipantGroupSizes(IList<Participant> participants)
        {
            Dictionary<ParticipantGroup, int> groupSizes = participants.GroupBy(participant => participant.ParticipantGroup)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());

            return groupSizes.TryGetValue(ParticipantGroup.Zero, out int group0Size) is false || group0Size != 1
                || groupSizes.TryGetValue(ParticipantGroup.One, out int group1Size) is false || group1Size < 3
                || groupSizes.TryGetValue(ParticipantGroup.Two, out int group2Size) is false || group2Size < 3;
        }
    }

    private sealed class SemiFinal1Builder : ChildBroadcastBuilder
    {
        public SemiFinal1Builder(Contest parentContest) : base(parentContest)
        {
            ContestStage = ContestStage.SemiFinal1;
        }

        private protected override ContestStage ContestStage { get; }

        private protected override bool MayCompete(Participant participant) =>
            participant.ParticipantGroup == ParticipantGroup.One;

        private protected override bool HasJury(Participant participant) => false;

        private protected override bool HasTelevote(Participant participant) =>
            participant.ParticipantGroup is ParticipantGroup.Zero or ParticipantGroup.One;
    }

    private sealed class SemiFinal2Builder : ChildBroadcastBuilder
    {
        public SemiFinal2Builder(Contest parentContest) : base(parentContest)
        {
            ContestStage = ContestStage.SemiFinal2;
        }

        private protected override ContestStage ContestStage { get; }

        private protected override bool MayCompete(Participant participant) =>
            participant.ParticipantGroup == ParticipantGroup.Two;

        private protected override bool HasJury(Participant participant) => false;

        private protected override bool HasTelevote(Participant participant) =>
            participant.ParticipantGroup is ParticipantGroup.Zero or ParticipantGroup.Two;
    }

    private sealed class GrandFinalBuilder : ChildBroadcastBuilder
    {
        public GrandFinalBuilder(Contest parentContest) : base(parentContest)
        {
            ContestStage = ContestStage.GrandFinal;
        }

        private protected override ContestStage ContestStage { get; }

        private protected override bool MayCompete(Participant participant) =>
            participant.ParticipantGroup is ParticipantGroup.One or ParticipantGroup.Two;

        private protected override bool HasJury(Participant participant) =>
            participant.ParticipantGroup is ParticipantGroup.One or ParticipantGroup.Two;

        private protected override bool HasTelevote(Participant participant) => true;
    }
}

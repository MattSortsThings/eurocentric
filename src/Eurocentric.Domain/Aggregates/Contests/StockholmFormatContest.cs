using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest aggregate using the <see cref="Enums.ContestFormat.Stockholm" /> contest format.
/// </summary>
public sealed class StockholmFormatContest : Contest
{
    private StockholmFormatContest()
    {
    }

    private StockholmFormatContest(ContestId id, List<Participant> participants, ContestYear contestYear, CityName cityName) :
        base(id, participants, contestYear, cityName)
    {
    }

    /// <inheritdoc />
    public override ContestFormat ContestFormat { get; private protected init; } = ContestFormat.Stockholm;

    private protected override IBroadcastEligibilityRulesSet GetBroadcastEligibilityRules(ContestStage contestStage) =>
        contestStage switch
        {
            ContestStage.SemiFinal1 => new SemiFinal1BroadcastEligibilityRules(),
            ContestStage.SemiFinal2 => new SemiFinal2BroadcastEligibilityRules(),
            ContestStage.GrandFinal => new GrandFinalBroadcastEligibilityRules(),
            _ => throw new InvalidEnumArgumentException(nameof(contestStage), (int)contestStage, typeof(ContestStage))
        };

    /// <summary>
    ///     Starts the process of creating a new <see cref="StockholmFormatContest" /> instance using the fluent builder.
    /// </summary>
    /// <returns>
    ///     A new <see cref="ContestBuilder" /> instance configured to build an instance of the
    ///     <see cref="StockholmFormatContest" /> derivative.
    /// </returns>
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
                        .FailIf(IllegalParticipantGroupSizes, ContestErrors.IllegalStockholmFormatParticipantGroups()))
                .Combine()
                .Then(Contest (tuple) => new StockholmFormatContest(idProvider(), tuple.Item3, tuple.Item1, tuple.Item2));
        }

        private static bool IllegalParticipantGroupSizes(IList<Participant> participants)
        {
            Dictionary<ParticipantGroup, int> groupSizes = participants.GroupBy(participant => participant.ParticipantGroup)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());

            return groupSizes.ContainsKey(ParticipantGroup.Zero)
                   || groupSizes.TryGetValue(ParticipantGroup.One, out int group1Size) is false || group1Size < 3
                   || groupSizes.TryGetValue(ParticipantGroup.Two, out int group2Size) is false || group2Size < 3;
        }
    }

    private sealed class SemiFinal1BroadcastEligibilityRules : IBroadcastEligibilityRulesSet
    {
        public bool MayCompete(Participant participant) => participant.ParticipantGroup == ParticipantGroup.One;

        public bool HasJury(Participant participant) => participant.ParticipantGroup == ParticipantGroup.One;

        public bool HasTelevote(Participant participant) => participant.ParticipantGroup == ParticipantGroup.One;
    }

    private sealed class SemiFinal2BroadcastEligibilityRules : IBroadcastEligibilityRulesSet
    {
        public bool MayCompete(Participant participant) => participant.ParticipantGroup == ParticipantGroup.Two;

        public bool HasJury(Participant participant) => participant.ParticipantGroup == ParticipantGroup.Two;

        public bool HasTelevote(Participant participant) => participant.ParticipantGroup == ParticipantGroup.Two;
    }

    private sealed class GrandFinalBroadcastEligibilityRules : IBroadcastEligibilityRulesSet
    {
        public bool MayCompete(Participant participant) =>
            participant.ParticipantGroup is ParticipantGroup.One or ParticipantGroup.Two;

        public bool HasJury(Participant participant) =>
            participant.ParticipantGroup is ParticipantGroup.One or ParticipantGroup.Two;

        public bool HasTelevote(Participant participant) =>
            participant.ParticipantGroup is ParticipantGroup.One or ParticipantGroup.Two;
    }
}

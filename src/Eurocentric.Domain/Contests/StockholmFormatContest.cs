using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

/// <summary>
///     Represents a contest aggregate using the Stockholm contest format.
/// </summary>
public sealed class StockholmFormatContest : Contest
{
    private StockholmFormatContest()
    {
    }

    private StockholmFormatContest(ContestId id, ContestYear year, CityName cityName, List<Participant> participants) :
        base(id, year, cityName, participants)
    {
    }

    /// <inheritdoc />
    public override ContestFormat Format { get; private protected init; } = ContestFormat.Stockholm;

    public static ContestBuilder Create() => new Builder();

    private protected override BroadcastBuilder CreateSemiFinal1BroadcastBuilder() => new SemiFinal1BroadcastBuilder(Id);

    private protected override BroadcastBuilder CreateSemiFinal2BroadcastBuilder() => new SemiFinal2BroadcastBuilder(Id);

    private protected override BroadcastBuilder CreateGrandFinalBroadcastBuilder() => new GrandFinalBroadcastBuilder(Id);

    private sealed class Builder : ContestBuilder
    {
        private const int RequiredGroup0Size = 0;
        private const int MinLegalGroup1Size = 3;
        private const int MinLegalGroup2Size = 3;

        public override ErrorOr<Contest> Build(Func<ContestId> idProvider)
        {
            ArgumentNullException.ThrowIfNull(idProvider);

            ErrorOr<List<Participant>> errorsOrParticipants = ErrorsOrParticipants.Collect()
                .FailIf(DuplicateParticipatingCountryIds, ContestErrors.DuplicateParticipatingCountryIds())
                .FailIf(IllegalGroupSizes, ContestErrors.IllegalStockholmFormatGroupSizes());

            return (ErrorsOrYear, ErrorsOrCityName, errorsOrParticipants)
                .Combine()
                .Then(Contest (tuple) => new StockholmFormatContest(idProvider(), tuple.First, tuple.Second, tuple.Third));
        }

        private static bool DuplicateParticipatingCountryIds(IList<Participant> participants) =>
            participants.GroupBy(p => p.ParticipatingCountryId)
                .Any(grouping => grouping.Count() > 1);

        private static bool IllegalGroupSizes(IList<Participant> participants)
        {
            Dictionary<ParticipantGroup, int> groupSizes = participants.Select(p => p.Group)
                .Concat(Enum.GetValues<ParticipantGroup>())
                .GroupBy(group => group)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Count() - 1);

            return groupSizes[ParticipantGroup.Zero] != RequiredGroup0Size
                   || groupSizes[ParticipantGroup.One] < MinLegalGroup1Size
                   || groupSizes[ParticipantGroup.Two] < MinLegalGroup2Size;
        }
    }

    private sealed class SemiFinal1BroadcastBuilder : BroadcastBuilder
    {
        public SemiFinal1BroadcastBuilder(ContestId contestId) : base(contestId)
        {
        }

        private protected override ContestStage ContestStage => ContestStage.SemiFinal1;

        private protected override bool MayCompete(Participant participant) => participant.Group == ParticipantGroup.One;

        private protected override bool HasJury(Participant participant) => participant.Group == ParticipantGroup.One;

        private protected override bool HasTelevote(Participant participant) => participant.Group == ParticipantGroup.One;
    }

    private sealed class SemiFinal2BroadcastBuilder : BroadcastBuilder
    {
        public SemiFinal2BroadcastBuilder(ContestId contestId) : base(contestId)
        {
        }

        private protected override ContestStage ContestStage => ContestStage.SemiFinal2;

        private protected override bool MayCompete(Participant participant) => participant.Group == ParticipantGroup.Two;

        private protected override bool HasJury(Participant participant) => participant.Group is ParticipantGroup.Two;

        private protected override bool HasTelevote(Participant participant) => participant.Group is ParticipantGroup.Two;
    }

    private sealed class GrandFinalBroadcastBuilder : BroadcastBuilder
    {
        public GrandFinalBroadcastBuilder(ContestId contestId) : base(contestId)
        {
        }

        private protected override ContestStage ContestStage => ContestStage.GrandFinal;

        private protected override bool MayCompete(Participant participant) => participant.Group is not ParticipantGroup.Zero;

        private protected override bool HasJury(Participant participant) => participant.Group is not ParticipantGroup.Zero;

        private protected override bool HasTelevote(Participant participant) => participant.Group is not ParticipantGroup.Zero;
    }
}

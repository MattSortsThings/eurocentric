using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

/// <summary>
///     Represents a contest aggregate using the <see cref="Enums.ContestFormat.Stockholm" /> contest format.
/// </summary>
/// <remarks>
///     Create an instance of this class using the <see cref="Contest.CreateStockholmFormat" /> static method on the
///     <see cref="Contest" /> class.
/// </remarks>
public sealed class StockholmFormatContest : Contest
{
    private const int RequiredParticipantGroupZeroSize = 0;
    private const int MinParticipantGroupOneSize = 3;
    private const int MinParticipantGroupTwoSize = 3;

    private StockholmFormatContest()
    {
    }

    private StockholmFormatContest(ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants) :
        base(id, contestYear, cityName, participants)
    {
    }

    /// <inheritdoc />
    public override ContestFormat ContestFormat { get; private protected init; } = ContestFormat.Stockholm;

    private protected override Func<Participant, bool> GetJuryEligibility(ContestStage contestStage) => contestStage switch
    {
        ContestStage.SemiFinal1 => participant => participant.ParticipantGroup is ParticipantGroup.One,
        ContestStage.SemiFinal2 => participant => participant.ParticipantGroup is ParticipantGroup.Two,
        ContestStage.GrandFinal => participant => participant.ParticipantGroup is ParticipantGroup.One or ParticipantGroup.Two,
        _ => throw new InvalidEnumArgumentException(nameof(contestStage), (int)contestStage, typeof(ContestStage))
    };

    private protected override Func<Participant, bool> GetTelevoteEligibility(ContestStage contestStage) => contestStage switch
    {
        ContestStage.SemiFinal1 => participant => participant.ParticipantGroup is ParticipantGroup.One,
        ContestStage.SemiFinal2 => participant => participant.ParticipantGroup is ParticipantGroup.Two,
        ContestStage.GrandFinal => participant => participant.ParticipantGroup is ParticipantGroup.One or ParticipantGroup.Two,
        _ => throw new InvalidEnumArgumentException(nameof(contestStage), (int)contestStage, typeof(ContestStage))
    };

    private protected override Func<Participant, bool> GetCompetitorEligibility(ContestStage contestStage) => contestStage switch
    {
        ContestStage.SemiFinal1 => participant => participant.ParticipantGroup is ParticipantGroup.One,
        ContestStage.SemiFinal2 => participant => participant.ParticipantGroup is ParticipantGroup.Two,
        ContestStage.GrandFinal => participant => participant.ParticipantGroup is ParticipantGroup.One or ParticipantGroup.Two,
        _ => throw new InvalidEnumArgumentException(nameof(contestStage), (int)contestStage, typeof(ContestStage))
    };

    internal sealed class Builder : ContestBuilder
    {
        private protected override ErrorOr<List<Participant>> CollectAndCheck(
            IReadOnlyList<ErrorOr<Participant>> errorsOrParticipants) => errorsOrParticipants.Collect()
            .FailIf(DuplicateParticipatingCountries, ContestErrors.DuplicateParticipatingCountries())
            .FailIf(IllegalParticipantGroupSizes, ContestErrors.IllegalStockholmFormatGroupSizes());

        private protected override Contest InitializeContest(ContestId id,
            ContestYear contestYear,
            CityName cityName,
            List<Participant> participants) => new StockholmFormatContest(id, contestYear, cityName, participants);

        private static bool DuplicateParticipatingCountries(IEnumerable<Participant> participants) =>
            participants.GroupBy(participant => participant.ParticipatingCountryId)
                .Any(grouping => grouping.Count() > 1);

        private static bool IllegalParticipantGroupSizes(IEnumerable<Participant> participants)
        {
            Dictionary<ParticipantGroup, int> groupSizes = Enum.GetValues<ParticipantGroup>()
                .Concat(participants.Select(participant => participant.ParticipantGroup))
                .GroupBy(value => value)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Count() - 1);

            return groupSizes[ParticipantGroup.Zero] != RequiredParticipantGroupZeroSize
                   || groupSizes[ParticipantGroup.One] < MinParticipantGroupOneSize
                   || groupSizes[ParticipantGroup.Two] < MinParticipantGroupTwoSize;
        }
    }
}

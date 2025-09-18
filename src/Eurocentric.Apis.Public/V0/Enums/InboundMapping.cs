using System.ComponentModel;
using ApiContestStage = Eurocentric.Apis.Public.V0.Enums.ContestStage;
using DomainContestStage = Eurocentric.Domain.Enums.ContestStage;
using ApiVotingMethod = Eurocentric.Apis.Public.V0.Enums.VotingMethod;
using DomainVotingMethod = Eurocentric.Domain.V0Analytics.Rankings.Common.VotingMethod;

namespace Eurocentric.Apis.Public.V0.Enums;

internal static class InboundMapping
{
    internal static DomainContestStage MapToDomainContestStage(this ApiContestStage contestStage) => contestStage switch
    {
        ApiContestStage.SemiFinal1 => DomainContestStage.SemiFinal1,
        ApiContestStage.SemiFinal2 => DomainContestStage.SemiFinal2,
        ApiContestStage.GrandFinal => DomainContestStage.GrandFinal,
        _ => throw new InvalidEnumArgumentException(nameof(contestStage), (int)contestStage, typeof(ApiContestStage))
    };

    internal static DomainContestStage[]? MapToNullableDomainContestStages(this ApiContestStage[]? contestStages) =>
        contestStages is null || contestStages.Length == 0
            ? null
            : contestStages.Select(contestStage => contestStage.MapToDomainContestStage())
                .OrderBy(contestStage => contestStage)
                .Distinct()
                .ToArray();


    internal static DomainVotingMethod MapToDomainVotingMethod(this ApiVotingMethod votingMethod) => votingMethod switch
    {
        ApiVotingMethod.Any => DomainVotingMethod.Any,
        ApiVotingMethod.Jury => DomainVotingMethod.Jury,
        ApiVotingMethod.Televote => DomainVotingMethod.Televote,
        _ => throw new InvalidEnumArgumentException(nameof(votingMethod), (int)votingMethod, typeof(ApiVotingMethod))
    };
}

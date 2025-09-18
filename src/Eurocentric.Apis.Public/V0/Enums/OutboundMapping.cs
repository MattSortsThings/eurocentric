using ApiContestStage = Eurocentric.Apis.Public.V0.Enums.ContestStage;
using DomainContestStage = Eurocentric.Domain.Enums.ContestStage;
using ApiVotingMethod = Eurocentric.Apis.Public.V0.Enums.VotingMethod;
using DomainVotingMethod = Eurocentric.Domain.V0Analytics.Rankings.Common.VotingMethod;

namespace Eurocentric.Apis.Public.V0.Enums;

internal static class OutboundMapping
{
    internal static ApiContestStage MapToApiContestStage(this DomainContestStage contestStage) =>
        (ApiContestStage)(int)contestStage;

    internal static ApiVotingMethod MapToApiVotingMethod(this DomainVotingMethod method) => (ApiVotingMethod)(int)method;
}

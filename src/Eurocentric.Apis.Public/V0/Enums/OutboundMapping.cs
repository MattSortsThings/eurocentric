using ApiContestStage = Eurocentric.Apis.Public.V0.Enums.ContestStage;
using DomainContestStage = Eurocentric.Domain.Enums.ContestStage;

namespace Eurocentric.Apis.Public.V0.Enums;

internal static class OutboundMapping
{
    internal static ApiContestStage ToApiContestStage(this DomainContestStage contestStage) =>
        (ApiContestStage)(int)contestStage;
}

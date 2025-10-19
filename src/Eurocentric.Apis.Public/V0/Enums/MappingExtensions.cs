using ApiContestStage = Eurocentric.Apis.Public.V0.Enums.ContestStage;
using DomainContestStage = Eurocentric.Domain.Enums.ContestStage;

namespace Eurocentric.Apis.Public.V0.Enums;

internal static class MappingExtensions
{
    internal static ApiContestStage ToApiContestStage(this DomainContestStage value) => (ApiContestStage)(int)value;
}

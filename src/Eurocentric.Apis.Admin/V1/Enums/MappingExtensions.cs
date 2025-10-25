using ApiContestRoleType = Eurocentric.Apis.Admin.V1.Enums.ContestRoleType;
using ApiContestRules = Eurocentric.Apis.Admin.V1.Enums.ContestRules;
using ApiContestStage = Eurocentric.Apis.Admin.V1.Enums.ContestStage;
using ApiSemiFinalDraw = Eurocentric.Apis.Admin.V1.Enums.SemiFinalDraw;
using DomainContestRoleType = Eurocentric.Domain.Enums.ContestRoleType;
using DomainContestRules = Eurocentric.Domain.Enums.ContestRules;
using DomainContestStage = Eurocentric.Domain.Enums.ContestStage;
using DomainSemiFinalDraw = Eurocentric.Domain.Enums.SemiFinalDraw;

namespace Eurocentric.Apis.Admin.V1.Enums;

internal static class MappingExtensions
{
    internal static ApiContestRoleType ToApiContestRoleType(this DomainContestRoleType value) =>
        (ApiContestRoleType)(int)value;

    internal static ApiContestRules ToApiContestRules(this DomainContestRules value) => (ApiContestRules)(int)value;

    internal static ApiContestStage ToApiContestStage(this DomainContestStage value) => (ApiContestStage)(int)value;

    internal static ApiSemiFinalDraw ToApiSemiFinalDraw(this DomainSemiFinalDraw value) => (ApiSemiFinalDraw)(int)value;
}

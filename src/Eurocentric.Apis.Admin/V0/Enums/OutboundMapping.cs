using ApiContestRoleType = Eurocentric.Apis.Admin.V0.Enums.ContestRoleType;
using DomainContestRoleType = Eurocentric.Domain.Enums.ContestRoleType;

namespace Eurocentric.Apis.Admin.V0.Enums;

internal static class OutboundMapping
{
    internal static ApiContestRoleType ToApiContestRoleType(this DomainContestRoleType roleType) =>
        (ApiContestRoleType)(int)roleType;
}

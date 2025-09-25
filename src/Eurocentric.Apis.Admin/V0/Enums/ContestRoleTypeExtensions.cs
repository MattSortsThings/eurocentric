using ApiContestRoleType = Eurocentric.Apis.Admin.V0.Enums.ContestRoleType;
using DomainContestRoleType = Eurocentric.Domain.Enums.ContestRoleType;

namespace Eurocentric.Apis.Admin.V0.Enums;

internal static class ContestRoleTypeExtensions
{
    internal static ApiContestRoleType ToApiContestRoleType(this DomainContestRoleType roleType) =>
        Enum.Parse<ApiContestRoleType>(roleType.ToString());
}

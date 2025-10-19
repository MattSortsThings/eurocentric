using ApiContestRoleType = Eurocentric.Apis.Admin.V0.Enums.ContestRoleType;
using DomainContestRoleType = Eurocentric.Domain.Enums.ContestRoleType;

namespace Eurocentric.Apis.Admin.V0.Enums;

internal static class MappingExtensions
{
    internal static ApiContestRoleType ToApiContestRoleType(this DomainContestRoleType value) =>
        (ApiContestRoleType)(int)value;
}

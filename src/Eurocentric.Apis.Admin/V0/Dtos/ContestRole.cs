using Eurocentric.Apis.Admin.V0.Enums;

namespace Eurocentric.Apis.Admin.V0.Dtos;

public sealed record ContestRole
{
    public Guid ContestId { get; init; }

    public ContestRoleType ContestRoleType { get; init; }
}

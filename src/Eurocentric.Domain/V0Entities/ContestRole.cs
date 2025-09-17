using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public sealed record ContestRole
{
    public Guid ContestId { get; init; }

    public ContestRoleType ContestRoleType { get; init; }
}

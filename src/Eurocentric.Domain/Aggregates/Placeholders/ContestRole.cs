using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public sealed class ContestRole
{
    public Guid ContestId { get; init; }

    public ContestRoleType ContestRoleType { get; init; }
}

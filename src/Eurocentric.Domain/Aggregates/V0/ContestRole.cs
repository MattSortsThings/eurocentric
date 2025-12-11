using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a role in a contest.
/// </summary>
public sealed record ContestRole
{
    /// <summary>
    ///     Gets the ID of the contest.
    /// </summary>
    public Guid ContestId { get; init; }

    /// <summary>
    ///     Gets the contest role's type.
    /// </summary>
    public ContestRoleType ContestRoleType { get; init; }
}

using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Aggregates.Countries;

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
    ///     Gets the type of the contest role.
    /// </summary>
    public ContestRoleType ContestRoleType { get; init; }
}

using Eurocentric.Apis.Admin.V1.Enums;

namespace Eurocentric.Apis.Admin.V1.Dtos.Countries;

/// <summary>
///     Represents a role in a contest.
/// </summary>
public sealed record ContestRole
{
    /// <summary>
    ///     The ID of the contest.
    /// </summary>
    public Guid ContestId { get; init; }

    /// <summary>
    ///     The type of the contest role.
    /// </summary>
    public ContestRoleType ContestRoleType { get; init; }
}

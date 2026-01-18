using Eurocentric.Apis.Admin.V0.Common.Enums;

namespace Eurocentric.Apis.Admin.V0.Common.Dtos.Countries;

/// <summary>
///     Represents a role in a contest.
/// </summary>
public sealed record ContestRole
{
    /// <summary>
    ///     The ID of the contest.
    /// </summary>
    public required Guid ContestId { get; init; }

    /// <summary>
    ///     The contest role's type.
    /// </summary>
    public required ContestRoleType ContestRoleType { get; init; }
}

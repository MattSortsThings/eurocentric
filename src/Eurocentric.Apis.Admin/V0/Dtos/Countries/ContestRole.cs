using Eurocentric.Apis.Admin.V0.Enums;

namespace Eurocentric.Apis.Admin.V0.Dtos.Countries;

/// <summary>
///     Represent's a country's role in a contest.
/// </summary>
public sealed record ContestRole
{
    /// <summary>
    ///     Gets the ID of the contest.
    /// </summary>
    public Guid ContestId { get; init; }

    /// <summary>
    ///     Gets the contest role type.
    /// </summary>
    public ContestRoleType ContestRoleType { get; init; }
}

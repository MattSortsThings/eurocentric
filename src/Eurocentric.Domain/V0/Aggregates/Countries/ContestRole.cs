using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Aggregates.Countries;

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

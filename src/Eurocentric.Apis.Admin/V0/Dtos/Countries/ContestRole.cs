using Eurocentric.Apis.Admin.V0.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V0.Dtos.Countries;

/// <summary>
///     Represents a role in a contest.
/// </summary>
public sealed record ContestRole : ISchemaExampleProvider<ContestRole>
{
    /// <summary>
    ///     The ID of the contest.
    /// </summary>
    public Guid ContestId { get; init; }

    /// <summary>
    ///     The type of the contest role.
    /// </summary>
    public ContestRoleType ContestRoleType { get; init; }

    public static ContestRole CreateExample() =>
        new()
        {
            ContestId = Guid.Parse("30c1393d-1817-4c73-b4e3-ea20580af79b"),
            ContestRoleType = ContestRoleType.Participant,
        };
}

using Eurocentric.Apis.Admin.V0.Config;
using Eurocentric.Apis.Admin.V0.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V0.Dtos.Countries;

/// <summary>
///     Represents a role in a contest.
/// </summary>
public sealed record ContestRole : IDtoSchemaExampleProvider<ContestRole>
{
    /// <summary>
    ///     The ID of the contest.
    /// </summary>
    public Guid ContestId { get; init; }

    /// <summary>
    ///     The type of the contest role.
    /// </summary>
    public ContestRoleType ContestRoleType { get; init; }

    public static ContestRole CreateExample() => new() { ContestId = V0ExampleIds.Contest };
}

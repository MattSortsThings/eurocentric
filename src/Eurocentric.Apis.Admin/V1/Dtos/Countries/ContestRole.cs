using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Countries;

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

    public static ContestRole CreateExample() => new() { ContestId = V1ExampleIds.Contest };
}

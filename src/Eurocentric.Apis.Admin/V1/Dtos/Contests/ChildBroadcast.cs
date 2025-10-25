using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Contests;

/// <summary>
///     Summarizes a broadcast from the perspective of its parent contest.
/// </summary>
public sealed record ChildBroadcast : ISchemaExampleProvider<ChildBroadcast>
{
    /// <summary>
    ///     The ID of the child broadcast.
    /// </summary>
    public Guid ChildBroadcastId { get; init; }

    /// <summary>
    ///     The child broadcast's stage in the contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     A boolean value indicating whether all the points have been awarded in the child broadcast.
    /// </summary>
    public bool AllPointsAwarded { get; init; }

    public static ChildBroadcast CreateExample() =>
        new()
        {
            ChildBroadcastId = Guid.Parse("b90a2fb9-262a-4af9-beb2-e3d43cdf135b"),
            ContestStage = ContestStage.GrandFinal,
            AllPointsAwarded = true,
        };
}

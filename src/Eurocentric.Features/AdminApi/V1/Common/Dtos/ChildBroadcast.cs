using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

/// <summary>
///     Represents a broadcast from the perspective of its parent contest.
/// </summary>
public sealed record ChildBroadcast : IExampleProvider<ChildBroadcast>
{
    /// <summary>
    ///     The ID of the broadcast.
    /// </summary>
    public Guid BroadcastId { get; init; }

    /// <summary>
    ///     The broadcast's stage in its parent contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     Indicates whether the broadcast is completed in the system.
    /// </summary>
    public bool Completed { get; init; }

    public static ChildBroadcast CreateExample() => new()
    {
        BroadcastId = ExampleValues.BroadcastId, ContestStage = ContestStage.GrandFinal, Completed = false
    };
}

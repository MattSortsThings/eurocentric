using Eurocentric.Apis.Public.V0.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V0.Dtos.Listings;

public sealed record BroadcastResultMetadata : ISchemaExampleProvider<BroadcastResultMetadata>
{
    /// <summary>
    ///     The year in which the contest is held.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The broadcast's stage in its parent contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    public static BroadcastResultMetadata CreateExample() =>
        new() { ContestYear = 2025, ContestStage = ContestStage.GrandFinal };
}

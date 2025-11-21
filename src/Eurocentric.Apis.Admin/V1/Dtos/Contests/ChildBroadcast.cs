using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Contests;

/// <summary>
///     Summarizes a broadcast from the perspective of its parent contest.
/// </summary>
public sealed record ChildBroadcast : IDtoSchemaExampleProvider<ChildBroadcast>
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
    ///     A boolean value indicating whether the child broadcast is completed.
    /// </summary>
    public bool Completed { get; init; }

    public static ChildBroadcast CreateExample() =>
        new() { ChildBroadcastId = V1ExampleIds.Broadcast, ContestStage = ContestStage.GrandFinal };

    public bool Equals(ChildBroadcast? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ChildBroadcastId.Equals(other.ChildBroadcastId)
            && ContestStage == other.ContestStage
            && Completed == other.Completed;
    }

    public override int GetHashCode() => HashCode.Combine(ChildBroadcastId, (int)ContestStage, Completed);
}

using Eurocentric.Apis.Public.V0.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V0.Dtos.Listings;

public sealed record BroadcastResultMetadata : IDtoSchemaExampleProvider<BroadcastResultMetadata>
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

    public bool Equals(BroadcastResultMetadata? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ContestYear == other.ContestYear && ContestStage == other.ContestStage;
    }

    public override int GetHashCode() => HashCode.Combine(ContestYear, (int)ContestStage);
}

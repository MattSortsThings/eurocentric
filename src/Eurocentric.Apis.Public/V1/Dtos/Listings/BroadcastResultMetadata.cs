using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.Listings;

/// <summary>
///     Metadata describing an executed broadcast result listings query.
/// </summary>
public sealed record BroadcastResultMetadata : IDtoSchemaExampleProvider<BroadcastResultMetadata>
{
    /// <summary>
    ///     The required contest year filter value.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The required contest stage filter value.
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

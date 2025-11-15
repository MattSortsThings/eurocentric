using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     Parameters for a broadcast result listings query.
/// </summary>
public abstract record BroadcastResultQuery : IRequiredBroadcastFiltering
{
    /// <inheritdoc />
    public required int ContestYear { get; init; }

    /// <inheritdoc />
    public required ContestStage ContestStage { get; init; }
}

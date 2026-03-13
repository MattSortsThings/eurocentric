using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0.Broadcasts;

/// <summary>
///     Represents a competitor in a broadcast.
/// </summary>
public sealed class Competitor
{
    /// <summary>
    ///     Gets or initializes the competitor's competing country ID.
    /// </summary>
    public required Guid CompetingCountryId { get; init; }

    /// <summary>
    ///     Gets or initializes the competitor's spot in the broadcast performing order.
    /// </summary>
    public required int PerformingSpot { get; init; }

    /// <summary>
    ///     Gets or initializes the half of the broadcast in which the competitor performs.
    /// </summary>
    public required BroadcastHalf BroadcastHalf { get; init; }

    /// <summary>
    ///     Gets or sets the competitor's spot in the broadcast finishing order.
    /// </summary>
    public required int FinishingSpot { get; set; }

    /// <summary>
    ///     Gets or initializes an unordered list of the competitor's points awards.
    /// </summary>
    public required List<PointsAward> PointsAwards { get; init; }
}

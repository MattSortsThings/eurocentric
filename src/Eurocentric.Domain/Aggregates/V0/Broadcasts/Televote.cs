namespace Eurocentric.Domain.Aggregates.V0.Broadcasts;

/// <summary>
///     Represents a televote in a broadcast.
/// </summary>
public sealed class Televote
{
    /// <summary>
    ///     Gets or initializes the televote's voting country ID.
    /// </summary>
    public required Guid VotingCountryId { get; init; }

    /// <summary>
    ///     Gets or sets a boolean value denoting whether the televote has awarded its points.
    /// </summary>
    public required bool PointsAwarded { get; set; }
}

namespace Eurocentric.Domain.Aggregates.V0.Broadcasts;

/// <summary>
///     Represents a jury in a broadcast.
/// </summary>
public sealed class Jury
{
    /// <summary>
    ///     Gets or initializes the jury's voting country ID.
    /// </summary>
    public required Guid VotingCountryId { get; init; }

    /// <summary>
    ///     Gets or sets a boolean value denoting whether the jury has awarded its points.
    /// </summary>
    public required bool PointsAwarded { get; set; }
}

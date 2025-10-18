namespace Eurocentric.Domain.V0.Aggregates.Broadcasts;

/// <summary>
///     Represents a competitor in a broadcast.
/// </summary>
public sealed record Competitor
{
    /// <summary>
    ///     Gets the competing country's ID.
    /// </summary>
    public Guid CompetingCountryId { get; init; }

    /// <summary>
    ///     Gets the competitor's running order spot in the broadcast.
    /// </summary>
    public int RunningOrderSpot { get; init; }

    /// <summary>
    ///     Gets the competitor's finishing position in the broadcast.
    /// </summary>
    public int FinishingPosition { get; init; }

    /// <summary>
    ///     Gets a list of all the televote awards the competitor has received.
    /// </summary>
    public List<TelevoteAward> TelevoteAwards { get; init; } = [];

    /// <summary>
    ///     Gets a list of all the jury awards the competitor has received.
    /// </summary>
    public List<JuryAward> JuryAwards { get; init; } = [];
}

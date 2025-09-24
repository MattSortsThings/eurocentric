namespace Eurocentric.Domain.V0.Aggregates.Broadcasts;

/// <summary>
///     Represents a competitor in a broadcast.
/// </summary>
public sealed record Competitor
{
    /// <summary>
    ///     Gets the ID of the competing country.
    /// </summary>
    public Guid CompetingCountryId { get; init; }

    /// <summary>
    ///     Gets the competitor's running order spot in their broadcast.
    /// </summary>
    public int RunningOrderSpot { get; init; }

    /// <summary>
    ///     Gets the competitor's finishing position in their broadcast.
    /// </summary>
    public int FinishingPosition { get; init; }

    /// <summary>
    ///     Gets a list of all the jury awards received by the competitor.
    /// </summary>
    public List<JuryAward> JuryAwards { get; init; } = [];

    /// <summary>
    ///     Gets a list of all the televote awards received by the competitor.
    /// </summary>
    public List<TelevoteAward> TelevoteAwards { get; init; } = [];
}

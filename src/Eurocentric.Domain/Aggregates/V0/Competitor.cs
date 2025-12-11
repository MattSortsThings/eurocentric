namespace Eurocentric.Domain.Aggregates.V0;

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
    ///     Gets an unordered list of the competitor's televote awards.
    /// </summary>
    public List<TelevoteAward> TelevoteAwards { get; init; } = [];

    /// <summary>
    ///     Gets an unordered list of the competitor's jury awards.
    /// </summary>
    public List<JuryAward> JuryAwards { get; init; } = [];
}

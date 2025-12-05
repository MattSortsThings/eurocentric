namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a country competing in a broadcast.
/// </summary>
public sealed class Competitor
{
    /// <summary>
    ///     Gets the ID of the competing country.
    /// </summary>
    public int CompetingCountryId { get; init; }

    /// <summary>
    ///     Gets the competitor's running order spot.
    /// </summary>
    public int RunningOrderSpot { get; init; }

    /// <summary>
    ///     Gets the competitor's finishing position.
    /// </summary>
    public int FinishingPosition { get; init; }

    /// <summary>
    ///     Gets an unordered list of the televote points awards received by the competitor.
    /// </summary>
    public List<TelevoteAward> TelevoteAwards { get; init; } = [];

    /// <summary>
    ///     Gets an unordered list of the jury points awards received by the competitor.
    /// </summary>
    public List<JuryAward> JuryAwards { get; init; } = [];
}

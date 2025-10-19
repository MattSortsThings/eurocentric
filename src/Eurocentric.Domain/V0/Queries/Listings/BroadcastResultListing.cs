namespace Eurocentric.Domain.V0.Queries.Listings;

/// <summary>
///     Describes the result achieved by a single competitor in a specified broadcast.
/// </summary>
public sealed record BroadcastResultListing
{
    /// <summary>
    ///     Gets the competitor's finishing position.
    /// </summary>
    public int FinishingPosition { get; init; }

    /// <summary>
    ///     Gets the competitor's running order spot.
    /// </summary>
    public int RunningOrderSpot { get; init; }

    /// <summary>
    ///     Gets the competing country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the competing country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the competitor's act name.
    /// </summary>
    public string ActName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the competitor's song title.
    /// </summary>
    public string SongTitle { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the total jury points received by the competitor, if any.
    /// </summary>
    public int? JuryPoints { get; init; }

    /// <summary>
    ///     Gets the total televote points received by the competitor.
    /// </summary>
    public int TelevotePoints { get; init; }

    /// <summary>
    ///     Gets the overall total points received by the competitor.
    /// </summary>
    public int OverallPoints { get; init; }

    /// <summary>
    ///     Gets the competitor's rank based on total jury points received, if any.
    /// </summary>
    public int? JuryRank { get; init; }

    /// <summary>
    ///     Gets the competitor's rank based on total televote points received.
    /// </summary>
    public int TelevoteRank { get; init; }
}

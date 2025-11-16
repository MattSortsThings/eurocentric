using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     A single competing country result listings row.
/// </summary>
public sealed record CompetingCountryResultListing
{
    /// <summary>
    ///     The contest year of the broadcast.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The broadcast's stage in the contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     The competitor's running order spot in the broadcast.
    /// </summary>
    public int RunningOrderSpot { get; init; }

    /// <summary>
    ///     The competitor's act name.
    /// </summary>
    public string ActName { get; init; } = string.Empty;

    /// <summary>
    ///     The competitor's song title.
    /// </summary>
    public string SongTitle { get; init; } = string.Empty;

    /// <summary>
    ///     The total jury points the competitor received, or <see langword="null" /> in a televote-only broadcast.
    /// </summary>
    public int? JuryPoints { get; init; }

    /// <summary>
    ///     The competitor's rank based on total jury points, or <see langword="null" /> in a televote-only broadcast.
    /// </summary>
    public int? JuryRank { get; init; }

    /// <summary>
    ///     The total televote points the competitor received.
    /// </summary>
    public int TelevotePoints { get; init; }

    /// <summary>
    ///     The competitor's rank based on total televote points.
    /// </summary>
    public int TelevoteRank { get; init; }

    /// <summary>
    ///     The total overall points the competitor received.
    /// </summary>
    public int OverallPoints { get; init; }

    /// <summary>
    ///     The competitor's finishing position in the broadcast.
    /// </summary>
    public int FinishingPosition { get; init; }

    /// <summary>
    ///     The number of competitors in the broadcast.
    /// </summary>
    public int Competitors { get; init; }
}

using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.Listings;

/// <summary>
///     A single broadcast result listings row.
/// </summary>
public sealed record BroadcastResultListing : IDtoSchemaExampleProvider<BroadcastResultListing>
{
    /// <summary>
    ///     The competitor's running order spot in the broadcast.
    /// </summary>
    public int RunningOrderSpot { get; init; }

    /// <summary>
    ///     The competing country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The competing country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

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

    public static BroadcastResultListing CreateExample() =>
        new()
        {
            RunningOrderSpot = 1,
            CountryCode = "AA",
            CountryName = "CountryName",
            ActName = "ActName",
            SongTitle = "SongTitle",
            JuryPoints = 100,
            TelevotePoints = 100,
            OverallPoints = 200,
            JuryRank = 1,
            TelevoteRank = 1,
            FinishingPosition = 1,
        };
}

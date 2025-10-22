using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V0.Dtos.Listings;

/// <summary>
///     Describes the result achieved by a single competitor in a specified broadcast.
/// </summary>
public sealed record BroadcastResultListing : ISchemaExampleProvider<BroadcastResultListing>
{
    /// <summary>
    ///     The competitor's finishing position.
    /// </summary>
    public int FinishingPosition { get; init; }

    /// <summary>
    ///     The competitor's running order spot.
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
    ///     The total jury points received by the competitor, if any.
    /// </summary>
    public int? JuryPoints { get; init; }

    /// <summary>
    ///     The total televote points received by the competitor.
    /// </summary>
    public int TelevotePoints { get; init; }

    /// <summary>
    ///     The overall total points received by the competitor.
    /// </summary>
    public int OverallPoints { get; init; }

    /// <summary>
    ///     The competitor's rank based on total jury points received, if any.
    /// </summary>
    public int? JuryRank { get; init; }

    /// <summary>
    ///     The competitor's rank based on total televote points received.
    /// </summary>
    public int TelevoteRank { get; init; }

    public static BroadcastResultListing CreateExample() =>
        new()
        {
            FinishingPosition = 1,
            RunningOrderSpot = 9,
            CountryCode = "AT",
            CountryName = "Austria",
            ActName = "JJ",
            SongTitle = "Wasted Love",
            JuryPoints = 258,
            TelevotePoints = 178,
            OverallPoints = 436,
            JuryRank = 1,
            TelevoteRank = 4,
        };
}

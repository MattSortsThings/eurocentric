using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V0.Dtos.Listings;

/// <summary>
///     Describes the result achieved by a single competitor in a specified broadcast.
/// </summary>
public sealed record BroadcastResultListing : IDtoSchemaExampleProvider<BroadcastResultListing>
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

    public bool Equals(BroadcastResultListing? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return FinishingPosition == other.FinishingPosition
            && RunningOrderSpot == other.RunningOrderSpot
            && CountryCode == other.CountryCode
            && CountryName == other.CountryName
            && ActName == other.ActName
            && SongTitle == other.SongTitle
            && JuryPoints == other.JuryPoints
            && TelevotePoints == other.TelevotePoints
            && OverallPoints == other.OverallPoints
            && JuryRank == other.JuryRank
            && TelevoteRank == other.TelevoteRank;
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(FinishingPosition);
        hashCode.Add(RunningOrderSpot);
        hashCode.Add(CountryCode);
        hashCode.Add(CountryName);
        hashCode.Add(ActName);
        hashCode.Add(SongTitle);
        hashCode.Add(JuryPoints);
        hashCode.Add(TelevotePoints);
        hashCode.Add(OverallPoints);
        hashCode.Add(JuryRank);
        hashCode.Add(TelevoteRank);

        return hashCode.ToHashCode();
    }
}

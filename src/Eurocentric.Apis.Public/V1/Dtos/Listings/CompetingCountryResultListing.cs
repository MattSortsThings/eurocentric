using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.Listings;

/// <summary>
///     A single competing country result listings row.
/// </summary>
public sealed record CompetingCountryResultListing : IDtoSchemaExampleProvider<CompetingCountryResultListing>
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

    public static CompetingCountryResultListing CreateExample() =>
        new()
        {
            ContestYear = 2025,
            ContestStage = ContestStage.GrandFinal,
            RunningOrderSpot = 1,
            ActName = "ActName",
            SongTitle = "SongTitle",
            JuryPoints = 100,
            TelevotePoints = 100,
            OverallPoints = 200,
            JuryRank = 1,
            TelevoteRank = 1,
            FinishingPosition = 1,
        };

    public bool Equals(CompetingCountryResultListing? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ContestYear == other.ContestYear
            && ContestStage == other.ContestStage
            && RunningOrderSpot == other.RunningOrderSpot
            && ActName == other.ActName
            && SongTitle == other.SongTitle
            && JuryPoints == other.JuryPoints
            && JuryRank == other.JuryRank
            && TelevotePoints == other.TelevotePoints
            && TelevoteRank == other.TelevoteRank
            && OverallPoints == other.OverallPoints
            && FinishingPosition == other.FinishingPosition
            && Competitors == other.Competitors;
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(ContestYear);
        hashCode.Add((int)ContestStage);
        hashCode.Add(RunningOrderSpot);
        hashCode.Add(ActName);
        hashCode.Add(SongTitle);
        hashCode.Add(JuryPoints);
        hashCode.Add(JuryRank);
        hashCode.Add(TelevotePoints);
        hashCode.Add(TelevoteRank);
        hashCode.Add(OverallPoints);
        hashCode.Add(FinishingPosition);
        hashCode.Add(Competitors);

        return hashCode.ToHashCode();
    }
}

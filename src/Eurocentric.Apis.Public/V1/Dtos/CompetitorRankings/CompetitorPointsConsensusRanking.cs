using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;

/// <summary>
///     A single competitor points consensus rankings row.
/// </summary>
public sealed record CompetitorPointsConsensusRanking : IDtoSchemaExampleProvider<CompetitorPointsConsensusRanking>
{
    /// <summary>
    ///     The competitor's rank based on descending points consensus.
    /// </summary>
    public int Rank { get; init; }

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
    ///     The competing country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The competing country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     The competitor's finishing position in the broadcast.
    /// </summary>
    public int FinishingPosition { get; init; }

    /// <summary>
    ///     The competitor's act name.
    /// </summary>
    public string ActName { get; init; } = string.Empty;

    /// <summary>
    ///     The competitor's song title.
    /// </summary>
    public string SongTitle { get; init; } = string.Empty;

    /// <summary>
    ///     The cosine similarity between the normalized jury and televote points the competitor received in their
    ///     broadcast, using each voting country in the broadcast as a vector dimension in the comparison.
    /// </summary>
    public decimal PointsConsensus { get; init; }

    /// <summary>
    ///     The number of vector dimensions in the queried filtered voting data for the competitor.
    /// </summary>
    public int VectorDimensions { get; init; }

    /// <summary>
    ///     The length of the normalized jury points vector.
    /// </summary>
    public decimal JuryVectorLength { get; init; }

    /// <summary>
    ///     The length of the normalized televote points vector.
    /// </summary>
    public decimal TelevoteVectorLength { get; init; }

    /// <summary>
    ///     The dot product of the normalized jury points vector and the normalized televote points vector.
    /// </summary>
    public decimal VectorDotProduct { get; init; }

    public static CompetitorPointsConsensusRanking CreateExample() =>
        new()
        {
            Rank = 1,
            ContestYear = 2025,
            ContestStage = ContestStage.GrandFinal,
            CountryCode = "AA",
            CountryName = "CountryName",
            ActName = "ActName",
            SongTitle = "SongTitle",
            PointsConsensus = 0.8m,
            VectorDotProduct = 25.0m,
            JuryVectorLength = 5.0m,
            TelevoteVectorLength = 6.25m,
            VectorDimensions = 37,
        };

    public bool Equals(CompetitorPointsConsensusRanking? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Rank == other.Rank
            && ContestYear == other.ContestYear
            && ContestStage == other.ContestStage
            && RunningOrderSpot == other.RunningOrderSpot
            && CountryCode == other.CountryCode
            && CountryName == other.CountryName
            && FinishingPosition == other.FinishingPosition
            && ActName == other.ActName
            && SongTitle == other.SongTitle
            && PointsConsensus == other.PointsConsensus
            && VectorDimensions == other.VectorDimensions
            && JuryVectorLength == other.JuryVectorLength
            && TelevoteVectorLength == other.TelevoteVectorLength
            && VectorDotProduct == other.VectorDotProduct;
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Rank);
        hashCode.Add(ContestYear);
        hashCode.Add((int)ContestStage);
        hashCode.Add(RunningOrderSpot);
        hashCode.Add(CountryCode);
        hashCode.Add(CountryName);
        hashCode.Add(FinishingPosition);
        hashCode.Add(ActName);
        hashCode.Add(SongTitle);
        hashCode.Add(PointsConsensus);
        hashCode.Add(VectorDimensions);
        hashCode.Add(JuryVectorLength);
        hashCode.Add(TelevoteVectorLength);
        hashCode.Add(VectorDotProduct);

        return hashCode.ToHashCode();
    }
}

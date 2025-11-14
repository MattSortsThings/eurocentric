using Eurocentric.Apis.Public.V1.Enums;

namespace Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;

/// <summary>
///     A single competitor points consensus rankings row.
/// </summary>
public sealed record CompetitorPointsConsensusRanking
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
}

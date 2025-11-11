namespace Eurocentric.Domain.Analytics.Rankings.CompetingCountries;

/// <summary>
///     The result of a competing country points consensus rankings query.
/// </summary>
/// <param name="Rankings">The page of rankings.</param>
/// <param name="Metadata">The query metadata.</param>
public readonly record struct PointsConsensusRankings(
    List<PointsConsensusRanking> Rankings,
    PointsConsensusMetadata Metadata
);

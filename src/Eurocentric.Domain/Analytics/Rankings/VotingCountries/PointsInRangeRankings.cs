namespace Eurocentric.Domain.Analytics.Rankings.VotingCountries;

/// <summary>
///     The result of a voting country points in range rankings query.
/// </summary>
/// <param name="Rankings">The page of rankings.</param>
/// <param name="Metadata">The query metadata.</param>
public readonly record struct PointsInRangeRankings(
    List<PointsInRangeRanking> Rankings,
    PointsInRangeMetadata Metadata
);

namespace Eurocentric.Domain.Analytics.Rankings.Competitors;

/// <summary>
///     The result of a competitor points average rankings query.
/// </summary>
/// <param name="Rankings">The page of rankings.</param>
/// <param name="Metadata">The query metadata.</param>
public readonly record struct PointsAverageRankings(
    List<PointsAverageRanking> Rankings,
    PointsAverageMetadata Metadata
);

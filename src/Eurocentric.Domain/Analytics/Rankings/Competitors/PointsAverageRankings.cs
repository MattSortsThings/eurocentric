namespace Eurocentric.Domain.Analytics.Rankings.Competitors;

public readonly record struct PointsAverageRankings(
    List<PointsAverageRanking> Rankings,
    PointsAverageMetadata Metadata
);

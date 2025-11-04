namespace Eurocentric.Domain.Analytics.Rankings.CompetingCountries;

public readonly record struct PointsAverageRankings(
    List<PointsAverageRanking> Rankings,
    PointsAverageMetadata Metadata
);

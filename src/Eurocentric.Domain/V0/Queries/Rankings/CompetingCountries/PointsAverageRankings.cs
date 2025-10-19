namespace Eurocentric.Domain.V0.Queries.Rankings.CompetingCountries;

public readonly record struct PointsAverageRankings(
    List<PointsAverageRanking> Rankings,
    PointsAverageMetadata Metadata
);

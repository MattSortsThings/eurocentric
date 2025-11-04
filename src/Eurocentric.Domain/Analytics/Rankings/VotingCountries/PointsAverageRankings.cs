namespace Eurocentric.Domain.Analytics.Rankings.VotingCountries;

public readonly record struct PointsAverageRankings(
    List<PointsAverageRanking> Rankings,
    PointsAverageMetadata Metadata
);

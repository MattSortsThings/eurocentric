namespace Eurocentric.Domain.Analytics.Rankings.VotingCountries;

/// <summary>
///     The result of a voting country points share rankings query.
/// </summary>
/// <param name="Rankings">The page of rankings.</param>
/// <param name="Metadata">The query metadata.</param>
public readonly record struct PointsShareRankings(List<PointsShareRanking> Rankings, PointsShareMetadata Metadata);

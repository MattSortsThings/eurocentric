namespace Eurocentric.Domain.Analytics.Rankings.CompetingCountries;

/// <summary>
///     The result of a competing country points share rankings query.
/// </summary>
/// <param name="Rankings">The page of rankings.</param>
/// <param name="Metadata">The query metadata.</param>
public readonly record struct PointsShareRankings(List<PointsShareRanking> Rankings, PointsShareMetadata Metadata);

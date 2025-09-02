using Eurocentric.Domain.V0.Rankings.Common;

namespace Eurocentric.Domain.V0.Rankings.CompetingCountries;

public sealed record PointsInRangeRankingsPage(List<PointsInRangeRanking> Rankings, PointsInRangeMetadata Metadata) :
    RankingsPage<PointsInRangeRanking, PointsInRangeMetadata>(Rankings, Metadata);

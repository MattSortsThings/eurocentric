using Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;

namespace Eurocentric.Apis.Public.V1.Features.CompetitorRankings;

public sealed record GetCompetitorPointsInRangeRankingsResponse(
    CompetitorPointsInRangeRanking[] Rankings,
    CompetitorPointsInRangeMetadata Metadata
);

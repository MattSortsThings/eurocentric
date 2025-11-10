using Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;

namespace Eurocentric.Apis.Public.V1.Features.CompetitorRankings;

public sealed record GetCompetitorPointsShareRankingsResponse(
    CompetitorPointsShareRanking[] Rankings,
    CompetitorPointsShareMetadata Metadata
);

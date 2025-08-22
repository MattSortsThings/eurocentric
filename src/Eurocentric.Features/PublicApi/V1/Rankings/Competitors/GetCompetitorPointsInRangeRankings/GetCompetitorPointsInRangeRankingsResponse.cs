using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsInRangeRankings;

public sealed record GetCompetitorPointsInRangeRankingsResponse(
    CompetitorPointsInRangeRanking[] Rankings,
    CompetitorPointsInRangeQueryMetadata Query,
    PaginationMetadata Pagination);

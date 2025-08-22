using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsShareRankings;

public sealed record GetCompetitorPointsShareRankingsResponse(
    CompetitorPointsShareRanking[] Rankings,
    CompetitorPointsShareQueryMetadata Query,
    PaginationMetadata Pagination);

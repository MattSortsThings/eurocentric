using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetitorPointsShareRankings;

public sealed record GetCompetitorPointsShareRankingsResponse(
    CompetitorPointsShareRanking[] Rankings,
    CompetitorPointsShareFilteringMetadata Filtering,
    PaginationMetadata Pagination);

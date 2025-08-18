using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetitorPointsAverageRankings;

public sealed record GetCompetitorPointsAverageRankingsResponse(
    CompetitorPointsAverageRanking[] Rankings,
    CompetitorPointsAverageFilteringMetadata Filtering,
    PaginationMetadata Pagination);

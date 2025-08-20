using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsAverageRankings;

public sealed record GetCompetitorPointsAverageRankingsResponse(
    CompetitorPointsAverageRanking[] Rankings,
    CompetitorPointsAverageFilteringMetadata Filtering,
    PaginationMetadata Pagination);

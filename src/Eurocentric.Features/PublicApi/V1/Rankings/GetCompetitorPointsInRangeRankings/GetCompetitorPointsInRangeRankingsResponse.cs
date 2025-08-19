using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetitorPointsInRangeRankings;

public sealed record GetCompetitorPointsInRangeRankingsResponse(
    CompetitorPointsInRangeRanking[] Rankings,
    CompetitorPointsInRangeFilteringMetadata Filtering,
    PaginationMetadata Pagination);

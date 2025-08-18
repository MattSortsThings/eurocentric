using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsInRangeRankings;

public sealed record GetCompetingCountryPointsInRangeRankingsResponse(
    CompetingCountryPointsInRangeRanking[] Rankings,
    CompetingCountryPointsInRangeFilteringMetadata Filtering,
    PaginationMetadata Pagination);

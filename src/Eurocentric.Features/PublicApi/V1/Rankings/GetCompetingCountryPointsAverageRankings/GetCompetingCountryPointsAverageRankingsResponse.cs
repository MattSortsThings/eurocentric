using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsAverageRankings;

public sealed record GetCompetingCountryPointsAverageRankingsResponse(
    CompetingCountryPointsAverageRanking[] Rankings,
    CompetingCountryPointsAverageFilteringMetadata Filtering,
    PaginationMetadata Pagination);

using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsShareRankings;

public sealed record GetCompetingCountryPointsShareRankingsResponse(
    CompetingCountryPointsShareRanking[] Rankings,
    CompetingCountryPointsShareFilteringMetadata Filtering,
    PaginationMetadata Pagination);

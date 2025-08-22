using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsAverageRankings;

public sealed record GetCompetingCountryPointsAverageRankingsResponse(
    CompetingCountryPointsAverageRanking[] Rankings,
    CompetingCountryPointsAverageQueryMetadata Query,
    PaginationMetadata Pagination);

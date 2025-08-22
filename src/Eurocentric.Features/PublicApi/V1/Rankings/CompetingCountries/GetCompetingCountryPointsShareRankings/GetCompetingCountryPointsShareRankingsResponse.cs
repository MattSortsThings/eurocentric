using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsShareRankings;

public sealed record GetCompetingCountryPointsShareRankingsResponse(
    CompetingCountryPointsShareRanking[] Rankings,
    CompetingCountryPointsShareQueryMetadata Query,
    PaginationMetadata Pagination);

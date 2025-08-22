using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsConsensusRankings;

public sealed record GetCompetingCountryPointsConsensusRankingsResponse(
    CompetingCountryPointsConsensusRanking[] Rankings,
    CompetingCountryPointsConsensusQueryMetadata Query,
    PaginationMetadata Pagination);

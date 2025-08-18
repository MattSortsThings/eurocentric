using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsConsensusRankings;

public sealed record GetCompetingCountryPointsConsensusRankingsResponse(
    CompetingCountryPointsConsensusRanking[] Rankings,
    CompetingCountryPointsConsensusFilteringMetadata Filtering,
    PaginationMetadata Pagination);

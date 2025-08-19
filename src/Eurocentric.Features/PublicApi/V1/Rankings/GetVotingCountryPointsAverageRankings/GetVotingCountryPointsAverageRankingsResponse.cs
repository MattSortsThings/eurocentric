using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetVotingCountryPointsAverageRankings;

public sealed record GetVotingCountryPointsAverageRankingsResponse(
    VotingCountryPointsAverageRanking[] Rankings,
    VotingCountryPointsAverageFilteringMetadata Filtering,
    PaginationMetadata Pagination);

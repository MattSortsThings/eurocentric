using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsConsensusRankings;

public sealed record GetVotingCountryPointsConsensusRankingsResponse(
    VotingCountryPointsConsensusRanking[] Rankings,
    VotingCountryPointsConsensusQueryMetadata Query,
    PaginationMetadata Pagination);

using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsShareRankings;

public sealed record GetVotingCountryPointsShareRankingsResponse(
    VotingCountryPointsShareRanking[] Rankings,
    VotingCountryPointsShareQueryMetadata Query,
    PaginationMetadata Pagination);

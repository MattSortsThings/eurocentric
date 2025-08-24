using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsInRangeRankings;

public sealed record GetVotingCountryPointsInRangeRankingsResponse(
    VotingCountryPointsInRangeRanking[] Rankings,
    VotingCountryPointsInRangeQueryMetadata Query,
    PaginationMetadata Pagination);

using Eurocentric.Apis.Public.V1.Dtos.Rankings.VotingCountries;

namespace Eurocentric.Apis.Public.V1.Features.Rankings.VotingCountries;

public sealed record GetVotingCountryPointsAverageRankingsResponse(
    VotingCountryPointsAverageRanking[] Rankings,
    VotingCountryPointsAverageMetadata Metadata
);

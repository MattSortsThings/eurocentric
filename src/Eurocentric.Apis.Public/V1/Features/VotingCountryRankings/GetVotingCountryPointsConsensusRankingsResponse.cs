using Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;

namespace Eurocentric.Apis.Public.V1.Features.VotingCountryRankings;

public sealed record GetVotingCountryPointsConsensusRankingsResponse(
    VotingCountryPointsConsensusRanking[] Rankings,
    VotingCountryPointsConsensusMetadata Metadata
);

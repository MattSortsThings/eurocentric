using Eurocentric.PublicApi.V0.VotingCountryRankings.Models;

namespace Eurocentric.PublicApi.V0.VotingCountryRankings.GetVotingCountryPointsShareRankings;

public sealed record GetVotingCountryPointsShareRankingsResponse(
    VotingCountryPointsShareRanking[] Items,
    VotingCountryPointsShareMetadata Metadata);

using Eurocentric.PublicApi.V1.VotingCountryRankings.Models;

namespace Eurocentric.PublicApi.V1.VotingCountryRankings.GetVotingCountryPointsShareRankings;

public sealed record GetVotingCountryPointsShareRankingsResponse(
    VotingCountryPointsShareItem[] Items,
    VotingCountryPointsShareMetadata Metadata);

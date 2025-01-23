using Eurocentric.Domain.Queries.Common;

namespace Eurocentric.Domain.Queries.VotingCountryRankings;

public record VotingCountryPointsSharePage(VotingCountryPointsShareRanking[] Items, VotingCountryPointsShareMetadata Metadata)
    : RankingsPage<VotingCountryPointsShareRanking, VotingCountryPointsShareMetadata>(Items, Metadata);

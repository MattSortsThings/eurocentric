using Eurocentric.Domain.Queries.Common;

namespace Eurocentric.Domain.Queries.VotingCountryRankings;

public record VotingCountryPointsSharePage(VotingCountryPointsShareItem[] Items, VotingCountryPointsShareMetadata Metadata)
    : RankingsPage<VotingCountryPointsShareItem, VotingCountryPointsShareMetadata>(Items, Metadata);

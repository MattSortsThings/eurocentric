namespace Eurocentric.Components.DataAccess.Common;

internal static class Sprocs
{
    internal static class Dbo
    {
        internal const string GetCompetingCountryPointsAverageRankings =
            "dbo.usp_get_competing_country_points_average_rankings";
        internal const string GetCompetingCountryPointsConsensusRankings =
            "dbo.usp_get_competing_country_points_consensus_rankings";
        internal const string GetCompetingCountryPointsInRangeRankings =
            "dbo.usp_get_competing_country_points_in_range_rankings";
        internal const string GetCompetingCountryPointsShareRankings =
            "dbo.usp_get_competing_country_points_share_rankings";

        internal const string GetCompetitorPointsAverageRankings = "dbo.usp_get_competitor_points_average_rankings";
        internal const string GetCompetitorPointsConsensusRankings = "dbo.usp_get_competitor_points_consensus_rankings";
        internal const string GetCompetitorPointsShareRankings = "dbo.usp_get_competitor_points_share_rankings";

        internal const string GetVotingCountryPointsAverageRankings =
            "dbo.usp_get_voting_country_points_average_rankings";
        internal const string GetVotingCountryPointsConsensusRankings =
            "dbo.usp_get_voting_country_points_consensus_rankings";
        internal const string GetVotingCountryPointsShareRankings = "dbo.usp_get_voting_country_points_share_rankings";
    }

    internal static class V0
    {
        internal const string GetBroadcastResultListings = "v0.usp_get_broadcast_result_listings";
        internal const string GetCompetingCountryPointsAverageRankings =
            "v0.usp_get_competing_country_points_average_rankings";
    }
}

namespace Eurocentric.Infrastructure.DataAccess.Common;

public static class DbStoredProcedures
{
    public static class Dbo
    {
        public const string GetCompetingCountryPointsAverageRankings = "dbo.usp_get_competing_country_points_average_rankings";
        public const string GetCompetingCountryPointsConsensusRankings =
            "dbo.usp_get_competing_country_points_consensus_rankings";
        public const string GetCompetingCountryPointsInRangeRankings = "dbo.usp_get_competing_country_points_in_range_rankings";
        public const string GetCompetingCountryPointsShareRankings = "dbo.usp_get_competing_country_points_share_rankings";
        public const string GetCompetitorPointsAverageRankings = "dbo.usp_get_competitor_points_average_rankings";
        public const string GetCompetitorPointsConsensusRankings = "dbo.usp_get_competitor_points_consensus_rankings";
        public const string GetCompetitorPointsInRangeRankings = "dbo.usp_get_competitor_points_in_range_rankings";
        public const string GetCompetitorPointsShareRankings = "dbo.usp_get_competitor_points_share_rankings";
        public const string GetVotingCountryPointsAverageRankings = "dbo.usp_get_voting_country_points_average_rankings";
    }

    public static class V0
    {
        public const string GetCompetingCountryPointsAverageRankings = "v0.usp_get_competing_country_points_average_rankings";
        public const string GetCompetingCountryPointsInRangeRankings = "v0.usp_get_competing_country_points_in_range_rankings";
    }
}

namespace Eurocentric.Infrastructure.DataAccess.Common;

public static class DbStoredProcedures
{
    public static class Dbo
    {
        public const string GetCompetingCountryPointsAverageRankings = "dbo.usp_get_competing_country_points_average_rankings";
        public const string GetCompetingCountryPointsShareRankings = "dbo.usp_get_competing_country_points_share_rankings";
    }

    public static class V0
    {
        public const string GetCompetingCountryPointsAverageRankings = "v0.usp_get_competing_country_points_average_rankings";
        public const string GetCompetingCountryPointsInRangeRankings = "v0.usp_get_competing_country_points_in_range_rankings";
    }
}

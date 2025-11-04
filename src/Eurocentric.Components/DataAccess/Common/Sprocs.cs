namespace Eurocentric.Components.DataAccess.Common;

internal static class Sprocs
{
    internal static class Dbo
    {
        internal const string GetCompetingCountryPointsAverageRankings =
            "dbo.usp_get_competing_country_points_average_rankings";
    }

    internal static class V0
    {
        internal const string GetBroadcastResultListings = "v0.usp_get_broadcast_result_listings";
        internal const string GetCompetingCountryPointsAverageRankings =
            "v0.usp_get_competing_country_points_average_rankings";
    }
}

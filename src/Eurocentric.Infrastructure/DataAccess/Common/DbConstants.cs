namespace Eurocentric.Infrastructure.DataAccess.Common;

/// <summary>
///     Contains database constants.
/// </summary>
public static class DbConstants
{
    public const string ConnectionStringKey = "AzureSql";

    public static class StoredProcedures
    {
        public const string GetCompetingCountryPointsAverageRankings = "dbo.usp_get_competing_country_points_average_rankings";
        public const string GetCompetingCountryPointsInRangeRankings = "dbo.usp_get_competing_country_points_in_range_rankings";
    }
}

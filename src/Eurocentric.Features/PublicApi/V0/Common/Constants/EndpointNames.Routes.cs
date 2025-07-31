namespace Eurocentric.Features.PublicApi.V0.Common.Constants;

internal static partial class EndpointNames
{
    internal static class Routes
    {
        internal static class Queryables
        {
            internal const string GetQueryableContestStages = "PublicApi.V0.Queryables.GetQueryableContestStages";
            internal const string GetQueryableCountries = "PublicApi.V0.Queryables.GetQueryableCountries";
        }

        internal static class Rankings
        {
            internal const string GetCompetingCountryPointsAverageRankings =
                "PublicApi.V0.Queryables.GetCompetingCountryPointsAverageRankings";
            internal const string GetCompetingCountryPointsInRangeRankings =
                "PublicApi.V0.Queryables.GetCompetingCountryPointsInRangeRankings";
        }
    }
}

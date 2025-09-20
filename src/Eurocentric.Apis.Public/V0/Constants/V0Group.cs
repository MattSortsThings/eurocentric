namespace Eurocentric.Apis.Public.V0.Constants;

internal static class V0Group
{
    internal const string Name = "PublicApi.V0";

    internal static class Queryables
    {
        internal const string Tag = "Queryables";

        internal static class Endpoints
        {
            internal const string GetQueryableBroadcasts = "PublicApi.V0.Queryables.GetQueryableBroadcasts";
            internal const string GetQueryableContests = "PublicApi.V0.Queryables.GetQueryableContests";
            internal const string GetQueryableCountries = "PublicApi.V0.Queryables.GetQueryableCountries";
        }
    }

    internal static class CompetingCountryRankings
    {
        internal const string Tag = "Competing Country Rankings";

        internal static class Endpoints
        {
            internal const string GetCompetingCountryPointsInRangeRankings =
                "PublicApi.V0.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings";
        }
    }

    internal static class Scoreboards
    {
        internal const string Tag = "Scoreboards";

        internal static class Endpoints
        {
            internal const string GetScoreboard = "PublicApi.V0.Scoreboards.GetScoreboard";
        }
    }
}

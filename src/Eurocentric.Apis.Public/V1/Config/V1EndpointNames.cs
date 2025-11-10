namespace Eurocentric.Apis.Public.V1.Config;

internal static class V1EndpointNames
{
    internal static class CompetingCountryRankings
    {
        internal const string GetCompetingCountryPointsAverageRankings =
            "PublicApi.V1.GetCompetingCountryPointsAverageRankings";
        internal const string GetCompetingCountryPointsShareRankings =
            "PublicApi.V1.GetCompetingCountryPointsShareRankings";
    }

    internal static class CompetitorRankings
    {
        internal const string GetCompetitorPointsAverageRankings = "PublicApi.V1.GetCompetitorPointsAverageRankings";
    }

    internal static class Queryables
    {
        internal const string GetQueryableBroadcasts = "PublicApi.V1.GetQueryableBroadcasts";
        internal const string GetQueryableContests = "PublicApi.V1.GetQueryableContests";
        internal const string GetQueryableCountries = "PublicApi.V1.GetQueryableCountries";
    }

    internal static class VotingCountryRankings
    {
        internal const string GetVotingCountryPointsAverageRankings =
            "PublicApi.V1.GetVotingCountryPointsAverageRankings";
    }
}

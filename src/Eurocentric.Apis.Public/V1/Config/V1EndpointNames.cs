namespace Eurocentric.Apis.Public.V1.Config;

internal static class V1EndpointNames
{
    internal static class CompetingCountryRankings
    {
        internal const string GetCompetingCountryPointsAverageRankings =
            "PublicApi.V1.GetCompetingCountryPointsAverageRankings";
        internal const string GetCompetingCountryPointsConsensusRankings =
            "PublicApi.V1.GetCompetingCountryPointsConsensusRankings";
        internal const string GetCompetingCountryPointsInRangeRankings =
            "PublicApi.V1.GetCompetingCountryPointsInRangeRankings";
        internal const string GetCompetingCountryPointsShareRankings =
            "PublicApi.V1.GetCompetingCountryPointsShareRankings";
    }

    internal static class CompetitorRankings
    {
        internal const string GetCompetitorPointsAverageRankings = "PublicApi.V1.GetCompetitorPointsAverageRankings";
        internal const string GetCompetitorPointsConsensusRankings =
            "PublicApi.V1.GetCompetitorPointsConsensusRankings";
        internal const string GetCompetitorPointsShareRankings = "PublicApi.V1.GetCompetitorPointsShareRankings";
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
        internal const string GetVotingCountryPointsConsensusRankings =
            "PublicApi.V1.GetVotingCountryPointsConsensusRankings";
        internal const string GetVotingCountryPointsShareRankings = "PublicApi.V1.GetVotingCountryPointsShareRankings";
    }
}

namespace Eurocentric.Features.PublicApi.V0.Common.Constants;

internal static class EndpointNames
{
    internal static class Filters
    {
        internal const string GetContestStages = "PublicApi.V0.Filters.GetContestStages";
        internal const string GetCountries = "PublicApi.V0.Filters.GetCountries";
        internal const string GetVotingMethods = "PublicApi.V0.Filters.GetVotingMethods";
    }

    internal static class Rankings
    {
        internal const string GetCompetingCountryPointsAverageRankings =
            "PublicApi.V0.Rankings.GetCompetingCountryPointsAverageRankings";
        internal const string GetCompetingCountryPointsInRangeRankings =
            "PublicApi.V0.Rankings.GetCompetingCountryPointsInRangeRankings";
    }
}

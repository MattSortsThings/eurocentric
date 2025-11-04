using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;

public interface IRestRequestFactory
{
    ICompetingCountryRankingsEndpoints CompetingCountryRankings { get; }

    IQueryablesEndpoints Queryables { get; }

    IVotingCountryRankingsEndpoints VotingCountryRankings { get; }

    interface ICompetingCountryRankingsEndpoints
    {
        RestRequest GetCompetingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams);
    }

    interface IQueryablesEndpoints
    {
        RestRequest GetQueryableBroadcasts();

        RestRequest GetQueryableContests();

        RestRequest GetQueryableCountries();
    }

    interface IVotingCountryRankingsEndpoints
    {
        RestRequest GetVotingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams);
    }
}

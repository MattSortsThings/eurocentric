using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils;

public interface IRestRequestFactory
{
    ICompetingCountryRankingsEndpoints CompetingCountryRankings { get; }

    IListingsEndpoints Listings { get; }

    IQueryablesEndpoints Queryables { get; }

    interface ICompetingCountryRankingsEndpoints
    {
        RestRequest GetCompetingCountryPointsAverageRankings(IDictionary<string, object?> queryParameters);
    }

    interface IListingsEndpoints
    {
        RestRequest GetBroadcastResultListings(IDictionary<string, object?> queryParameters);
    }

    interface IQueryablesEndpoints
    {
        RestRequest GetQueryableBroadcasts();
        RestRequest GetQueryableContests();
        RestRequest GetQueryableCountries();
    }
}

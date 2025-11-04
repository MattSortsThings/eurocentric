using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils;

public interface IRestRequestFactory
{
    ICompetingCountryRankingsEndpoints CompetingCountryRankings { get; }

    IListingsEndpoints Listings { get; }

    IQueryablesEndpoints Queryables { get; }

    interface ICompetingCountryRankingsEndpoints
    {
        RestRequest GetCompetingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParameters);
    }

    interface IListingsEndpoints
    {
        RestRequest GetBroadcastResultListings(IReadOnlyDictionary<string, object?> queryParameters);
    }

    interface IQueryablesEndpoints
    {
        RestRequest GetQueryableBroadcasts();
        RestRequest GetQueryableContests();
        RestRequest GetQueryableCountries();
    }
}

using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils;

public sealed partial class EuroFanKernel
{
    private sealed class RestRequestFactory(string apiVersion)
        : IRestRequestFactory,
            IRestRequestFactory.ICompetingCountryRankingsEndpoints,
            IRestRequestFactory.IListingsEndpoints,
            IRestRequestFactory.IQueryablesEndpoints
    {
        public RestRequest GetCompetingCountryPointsAverageRankings(IDictionary<string, object?> queryParameters)
        {
            return GetRequest("/public/api/{apiVersion}/competing-country-rankings/points-average")
                .AddUrlSegment("apiVersion", apiVersion)
                .AddQueryParameters(queryParameters);
        }

        public RestRequest GetBroadcastResultListings(IDictionary<string, object?> queryParameters)
        {
            return GetRequest("/public/api/{apiVersion}/listings/broadcast-result")
                .AddUrlSegment("apiVersion", apiVersion)
                .AddQueryParameters(queryParameters);
        }

        public RestRequest GetQueryableBroadcasts() =>
            GetRequest("/public/api/{apiVersion}/queryables/broadcasts").AddUrlSegment("apiVersion", apiVersion);

        public RestRequest GetQueryableContests() =>
            GetRequest("/public/api/{apiVersion}/queryables/contests").AddUrlSegment("apiVersion", apiVersion);

        public RestRequest GetQueryableCountries() =>
            GetRequest("/public/api/{apiVersion}/queryables/countries").AddUrlSegment("apiVersion", apiVersion);

        public IRestRequestFactory.ICompetingCountryRankingsEndpoints CompetingCountryRankings => this;

        public IRestRequestFactory.IListingsEndpoints Listings => this;

        public IRestRequestFactory.IQueryablesEndpoints Queryables => this;

        private static RestRequest GetRequest(string route) => new(route);
    }
}

using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Utils;

public sealed partial class ApiDriver
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.IQueryablesEndpoints
    {
        public RestRequest GetQueryableBroadcasts() => GetRequest("public/api/{apiVersion}/queryables/broadcasts")
            .AddUrlSegment("apiVersion", _apiVersion);

        public RestRequest GetQueryableContests() => GetRequest("public/api/{apiVersion}/queryables/contests")
            .AddUrlSegment("apiVersion", _apiVersion);

        public RestRequest GetQueryableCountries() => GetRequest("public/api/{apiVersion}/queryables/countries")
            .AddUrlSegment("apiVersion", _apiVersion);
    }
}

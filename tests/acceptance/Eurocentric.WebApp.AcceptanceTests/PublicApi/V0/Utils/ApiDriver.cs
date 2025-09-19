using Eurocentric.WebApp.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Utils;

public sealed partial class ApiDriver : IApiDriver
{
    private ApiDriver(WebAppFixture fixture, RestRequestFactory requestFactory)
    {
        RestClient = fixture;
        RequestFactory = requestFactory;
    }

    public IWebAppFixtureRestClient RestClient { get; }

    public IRestRequestFactory RequestFactory { get; }

    public static ApiDriver Create(WebAppFixture fixture, string apiVersion = "v0.x")
    {
        RestRequestFactory requestFactory = new(apiVersion);

        return new ApiDriver(fixture, requestFactory);
    }

    private sealed partial class RestRequestFactory : IRestRequestFactory
    {
        private readonly string _apiVersion;

        public RestRequestFactory(string apiVersion)
        {
            _apiVersion = apiVersion;
        }

        public IRestRequestFactory.IQueryablesEndpoints Queryables => this;

        private static RestRequest GetRequest(string route) => new(route);
    }
}

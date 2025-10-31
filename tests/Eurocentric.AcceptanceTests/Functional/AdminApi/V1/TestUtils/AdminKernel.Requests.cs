using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed partial class AdminKernel
{
    private sealed partial class RestRequestFactory(string apiVersion) : IRestRequestFactory
    {
        public IRestRequestFactory.IBroadcastsEndpoints Broadcasts => this;

        public IRestRequestFactory.IContestsEndpoints Contests => this;

        public IRestRequestFactory.ICountriesEndpoints Countries => this;

        private static RestRequest DeleteRequest(string route) => new(route, Method.Delete);

        private static RestRequest GetRequest(string route) => new(route);

        private static RestRequest PostRequest(string route) => new(route, Method.Post);
    }
}

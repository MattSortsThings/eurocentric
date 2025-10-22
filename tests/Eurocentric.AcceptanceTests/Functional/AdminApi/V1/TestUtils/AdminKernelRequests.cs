using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed partial class AdminKernel
{
    private sealed class RestRequestFactory(string apiVersion)
        : IRestRequestFactory,
            IRestRequestFactory.ICountriesEndpoints
    {
        public RestRequest GetCountries() =>
            GetRequest("/admin/api/{apiVersion}/countries").UseSecretApiKey().AddUrlSegment("apiVersion", apiVersion);

        public IRestRequestFactory.ICountriesEndpoints Countries => this;

        private static RestRequest GetRequest(string route) => new(route);
    }
}

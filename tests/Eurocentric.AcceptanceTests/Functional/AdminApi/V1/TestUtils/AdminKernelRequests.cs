using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Features.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed partial class AdminKernel
{
    private sealed class RestRequestFactory(string apiVersion)
        : IRestRequestFactory,
            IRestRequestFactory.ICountriesEndpoints
    {
        public RestRequest CreateCountry(CreateCountryRequest request) =>
            PostRequest("/admin/api/{apiVersion}/countries")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddJsonBody(request);

        public RestRequest GetCountries() =>
            GetRequest("/admin/api/{apiVersion}/countries").UseSecretApiKey().AddUrlSegment("apiVersion", apiVersion);

        public RestRequest GetCountry(Guid countryId) =>
            GetRequest("/admin/api/{apiVersion}/countries/{countryId}")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddUrlSegment("countryId", countryId);

        public IRestRequestFactory.ICountriesEndpoints Countries => this;

        private static RestRequest GetRequest(string route) => new(route);

        private static RestRequest PostRequest(string route) => new(route, Method.Post);
    }
}

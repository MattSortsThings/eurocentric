using Eurocentric.Apis.Admin.V0.Features.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils;

public sealed partial class AdminKernel
{
    private sealed class RestRequestFactory(string apiVersion)
        : IRestRequestFactory,
            IRestRequestFactory.ICountriesEndpoints
    {
        public RestRequest CreateCountry(CreateCountryRequest requestBody) =>
            PostRequest("/admin/api/{apiVersion}/countries")
                .AddUrlSegment("apiVersion", apiVersion)
                .AddJsonBody(requestBody);

        public RestRequest DeleteCountry(Guid countryId) =>
            DeleteRequest("/admin/api/{apiVersion}/countries/{countryId}")
                .AddUrlSegment("apiVersion", apiVersion)
                .AddUrlSegment("countryId", countryId);

        public RestRequest GetCountries() =>
            GetRequest("/admin/api/{apiVersion}/countries").AddUrlSegment("apiVersion", apiVersion);

        public RestRequest GetCountry(Guid countryId) =>
            GetRequest("/admin/api/{apiVersion}/countries/{countryId}")
                .AddUrlSegment("apiVersion", apiVersion)
                .AddUrlSegment("countryId", countryId);

        public IRestRequestFactory.ICountriesEndpoints Countries => this;

        private static RestRequest DeleteRequest(string route) => new(route, Method.Delete);

        private static RestRequest GetRequest(string route) => new(route);

        private static RestRequest PostRequest(string route) => new(route, Method.Post);
    }
}

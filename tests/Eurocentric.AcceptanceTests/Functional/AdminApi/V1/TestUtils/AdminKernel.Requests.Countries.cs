using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Features.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed partial class AdminKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.ICountriesEndpoints
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
    }
}

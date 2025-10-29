using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Features.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed partial class AdminKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.ICountriesEndpoints
    {
        public RestRequest CreateCountry(CreateCountryRequest requestBody)
        {
            return PostRequest("/admin/api/{apiVersion}/countries")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddJsonBody(requestBody);
        }

        public RestRequest GetCountries()
        {
            return GetRequest("/admin/api/{apiVersion}/countries")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion);
        }

        public RestRequest GetCountry(Guid countryId)
        {
            return GetRequest("/admin/api/{apiVersion}/countries/{countryId}")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddUrlSegment("countryId", countryId);
        }
    }
}

using Eurocentric.Apis.Admin.V0.Features.Countries;
using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.AdminApi.V0.Utils;

public sealed partial class ApiDriver
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.ICountriesEndpoints
    {
        public RestRequest CreateCountry(CreateCountry.Request requestBody) => PostRequest("/admin/api/{apiVersion}/countries")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddJsonBody(requestBody);

        public RestRequest GetCountry(Guid countryId) => GetRequest("/admin/api/{apiVersion}/countries/{countryId}")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("countryId", countryId);

        public RestRequest GetCountries() => GetRequest("/admin/api/{apiVersion}/countries")
            .AddUrlSegment("apiVersion", _apiVersion);
    }
}

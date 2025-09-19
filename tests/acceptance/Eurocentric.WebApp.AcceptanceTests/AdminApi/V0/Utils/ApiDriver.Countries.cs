using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.AdminApi.V0.Utils;

public sealed partial class ApiDriver
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.ICountriesEndpoints
    {
        public RestRequest GetCountries() => GetRequest("/admin/api/{apiVersion}/countries")
            .AddUrlSegment("apiVersion", _apiVersion);
    }
}

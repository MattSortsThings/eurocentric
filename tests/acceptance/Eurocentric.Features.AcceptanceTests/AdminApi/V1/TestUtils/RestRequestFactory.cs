using Eurocentric.Features.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

public sealed class RestRequestFactory : IRestRequestFactory,
    IRestRequestFactory.ICountriesEndpoints
{
    private readonly string _apiVersion;

    public RestRequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public RestRequest GetCountry(Guid countryId) => GetRequest("/admin/api/{apiVersion}/countries/{countryId}")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddUrlSegment("countryId", countryId);

    public IRestRequestFactory.ICountriesEndpoints Countries => this;


    private static RestRequest GetRequest(string route) => new RestRequest(route).UseSecretApiKey();
}

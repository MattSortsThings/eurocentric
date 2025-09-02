using Eurocentric.Features.AdminApi.V0.Countries.CreateCountry;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;

public sealed class RestRequestFactory : IRestRequestFactory,
    IRestRequestFactory.ICountriesEndpoints
{
    private readonly string _apiVersion;

    public RestRequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public RestRequest CreateCountry(CreateCountryRequest requestBody) => PostRequest("/admin/api/{apiVersion}/countries")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddJsonBody(requestBody);

    public RestRequest GetCountries() => GetRequest("/admin/api/{apiVersion}/countries")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetCountry(Guid countryId) => GetRequest("/admin/api/{apiVersion}/countries/{countryId}")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddUrlSegment("countryId", countryId);

    public IRestRequestFactory.ICountriesEndpoints Countries => this;

    private static RestRequest GetRequest(string route) => new(route);

    private static RestRequest PostRequest(string route) => new(route, Method.Post);
}

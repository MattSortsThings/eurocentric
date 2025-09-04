using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

public sealed class RestRequestFactory : IRestRequestFactory,
    IRestRequestFactory.IContestsEndpoints,
    IRestRequestFactory.ICountriesEndpoints
{
    private readonly string _apiVersion;

    public RestRequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public RestRequest GetContest(Guid contestId) => GetRequest("/admin/api/{apiVersion}/contests/{contestId}")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddUrlSegment("contestId", contestId);

    public RestRequest CreateCountry(CreateCountryRequest requestBody) => PostRequest("/admin/api/{apiVersion}/countries")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddJsonBody(requestBody);

    public RestRequest GetCountries() => GetRequest("/admin/api/{apiVersion}/countries")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetCountry(Guid countryId) => GetRequest("/admin/api/{apiVersion}/countries/{countryId}")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddUrlSegment("countryId", countryId);

    public IRestRequestFactory.IContestsEndpoints Contests => this;

    public IRestRequestFactory.ICountriesEndpoints Countries => this;

    private static RestRequest GetRequest(string route) => new RestRequest(route).UseSecretApiKey();

    private static RestRequest PostRequest(string route) => new RestRequest(route, Method.Post).UseSecretApiKey();
}

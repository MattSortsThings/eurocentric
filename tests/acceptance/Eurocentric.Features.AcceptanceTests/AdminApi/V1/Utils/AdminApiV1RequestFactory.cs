using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Features.AdminApi.V1.Countries;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public sealed class AdminApiV1RequestFactory : IAdminApiV1RequestFactory,
    IAdminApiV1RequestFactory.IBroadcastsEndpoints,
    IAdminApiV1RequestFactory.IContestsEndpoints,
    IAdminApiV1RequestFactory.ICountriesEndpoints
{
    private readonly string _apiVersion;

    public AdminApiV1RequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public IAdminApiV1RequestFactory.IBroadcastsEndpoints Broadcasts => this;

    public IAdminApiV1RequestFactory.IContestsEndpoints Contests => this;

    public IAdminApiV1RequestFactory.ICountriesEndpoints Countries => this;

    public RestRequest GetBroadcast(Guid broadcastId) => Get("/admin/api/{apiVersion}/broadcasts/{broadcastId}")
        .AddUrlSegment("broadcastId", broadcastId);

    public RestRequest CreateContest(CreateContestRequest requestBody) => Post("/admin/api/{apiVersion}/contests")
        .AddJsonBody(requestBody);

    public RestRequest GetContest(Guid contestId) => Get("/admin/api/{apiVersion}/contests/{contestId}")
        .AddUrlSegment("contestId", contestId);

    public RestRequest GetContests() => Get("/admin/api/{apiVersion}/contests");

    public RestRequest CreateCountry(CreateCountryRequest requestBody) => Post("/admin/api/{apiVersion}/countries")
        .AddJsonBody(requestBody);

    public RestRequest GetCountries() => Get("/admin/api/{apiVersion}/countries");

    public RestRequest GetCountry(Guid countryId) => Get("/admin/api/{apiVersion}/countries/{countryId}")
        .AddUrlSegment("countryId", countryId);

    private RestRequest Get(string route) => new RestRequest(route)
        .UseSecretApiKey()
        .AddUrlSegment("apiVersion", _apiVersion);

    private RestRequest Post(string route) => new RestRequest(route, Method.Post)
        .UseSecretApiKey()
        .AddUrlSegment("apiVersion", _apiVersion);
}

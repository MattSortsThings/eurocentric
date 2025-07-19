using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Features.AdminApi.V1.Countries;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public sealed class RequestFactory : IRequestFactory,
    IBroadcastsRequestFactory,
    IContestsRequestFactory,
    ICountriesRequestFactory
{
    private readonly string _apiVersion;

    private RequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public RestRequest GetBroadcast(Guid broadcastId) => Get("/admin/api/{apiVersion}/broadcasts/{broadcastId}")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddUrlSegment("broadcastId", broadcastId);

    public RestRequest GetBroadcasts() => Get("/admin/api/{apiVersion}/broadcasts")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest CreateChildBroadcast(Guid contestId, CreateChildBroadcastRequest requestBody) =>
        Post("/admin/api/{apiVersion}/contests/{contestId}/broadcasts")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("contestId", contestId)
            .AddJsonBody(requestBody);

    public RestRequest CreateContest(CreateContestRequest requestBody) => Post("/admin/api/{apiVersion}/contests")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddJsonBody(requestBody);

    public RestRequest GetContest(Guid contestId) => Get("/admin/api/{apiVersion}/contests/{contestId}")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddUrlSegment("contestId", contestId);

    public RestRequest GetContests() => Get("/admin/api/{apiVersion}/contests")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest CreateCountry(CreateCountryRequest requestBody) => Post("/admin/api/{apiVersion}/countries")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddJsonBody(requestBody);

    public RestRequest GetCountries() => Get("/admin/api/{apiVersion}/countries")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetCountry(Guid countryId) => Get("/admin/api/{apiVersion}/countries/{countryId}")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddUrlSegment("countryId", countryId);

    public IBroadcastsRequestFactory Broadcasts => this;

    public IContestsRequestFactory Contests => this;

    public ICountriesRequestFactory Countries => this;

    public static RequestFactory WithApiVersion(string apiVersion) => new(apiVersion);

    private static RestRequest Get(string route)
    {
        RestRequest request = new(route);

        return request.UseSecretApiKey();
    }

    private static RestRequest Post(string route)
    {
        RestRequest request = new(route, Method.Post);

        return request.UseSecretApiKey();
    }
}

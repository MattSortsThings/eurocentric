using Eurocentric.Features.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;

public sealed class RestRequestFactory : IRestRequestFactory,
    IRestRequestFactory.ICompetingCountryRankingsEndpoints,
    IRestRequestFactory.IQueryablesEndpoints
{
    private readonly string _apiVersion;

    public RestRequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public RestRequest GetCompetingCountryPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("public/api/{apiVersion}/rankings/competing-countries/points-in-range")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);

    public RestRequest GetQueryableContestStages() => GetRequest("public/api/{apiVersion}/queryables/contest-stages")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetQueryableCountries() => GetRequest("public/api/{apiVersion}/queryables/countries")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetQueryableVotingMethods() => GetRequest("public/api/{apiVersion}/queryables/voting-methods")
        .AddUrlSegment("apiVersion", _apiVersion);


    public IRestRequestFactory.ICompetingCountryRankingsEndpoints CompetingCountryRankings => this;

    public IRestRequestFactory.IQueryablesEndpoints Queryables => this;

    private static RestRequest GetRequest(string route) => new(route);
}

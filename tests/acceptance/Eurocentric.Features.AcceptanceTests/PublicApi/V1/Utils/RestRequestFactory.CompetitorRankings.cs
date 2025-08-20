using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public sealed partial class RestRequestFactory : IRestRequestFactory.ICompetitorRankingsEndpoints
{
    public RestRequest GetCompetitorPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("public/api/{apiVersion}/rankings/competitors/points-average")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);

    public RestRequest GetCompetitorPointsConsensusRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("public/api/{apiVersion}/rankings/competitors/points-consensus")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);

    public RestRequest GetCompetitorPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("public/api/{apiVersion}/rankings/competitors/points-in-range")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);

    public RestRequest GetCompetitorPointsShareRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("public/api/{apiVersion}/rankings/competitors/points-share")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);
}

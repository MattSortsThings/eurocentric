using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public sealed partial class RestRequestFactory : IRestRequestFactory.IVotingCountryRankingsEndpoints
{
    public RestRequest GetVotingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("public/api/{apiVersion}/rankings/voting-countries/points-average")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);

    public RestRequest GetVotingCountryPointsShareRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("public/api/{apiVersion}/rankings/voting-countries/points-share")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);
}

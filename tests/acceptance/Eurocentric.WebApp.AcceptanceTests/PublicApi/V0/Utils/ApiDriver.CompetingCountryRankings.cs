using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Utils;

public sealed partial class ApiDriver
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.ICompetingCountryRankingsEndpoints
    {
        public RestRequest GetCompetingCountryPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams) =>
            GetRequest("/public/api/{apiVersion}/rankings/competing-countries/points-in-range")
                .AddUrlSegment("apiVersion", _apiVersion)
                .AddQueryParameters(queryParams);
    }
}

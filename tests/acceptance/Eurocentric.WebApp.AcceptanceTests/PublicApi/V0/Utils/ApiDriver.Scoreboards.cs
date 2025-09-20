using Eurocentric.Apis.Public.V0.Enums;
using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Utils;

public sealed partial class ApiDriver
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.IScoreboardsEndpoints
    {
        public RestRequest GetScoreboard(int contestYear, ContestStage contestStage) =>
            GetRequest("/public/api/{apiVersion}/scoreboards")
                .AddUrlSegment("apiVersion", _apiVersion)
                .AddQueryParameter(nameof(contestYear), contestYear)
                .AddQueryParameter(nameof(contestStage), contestStage.ToString());
    }
}

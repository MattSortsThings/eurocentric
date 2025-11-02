using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Features.Broadcasts;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed partial class AdminKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.IBroadcastsEndpoints
    {
        public RestRequest AwardBroadcastJuryPoints(Guid broadcastId, AwardBroadcastJuryPointsRequest requestBody)
        {
            return PatchRequest("/admin/api/{apiVersion}/broadcasts/{broadcastId}/award-jury")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddUrlSegment("broadcastId", broadcastId)
                .AddJsonBody(requestBody);
        }

        public RestRequest AwardBroadcastTelevotePoints(
            Guid broadcastId,
            AwardBroadcastTelevotePointsRequest requestBody
        )
        {
            return PatchRequest("/admin/api/{apiVersion}/broadcasts/{broadcastId}/award-televote")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddUrlSegment("broadcastId", broadcastId)
                .AddJsonBody(requestBody);
        }

        public RestRequest DeleteBroadcast(Guid broadcastId)
        {
            return DeleteRequest("/admin/api/{apiVersion}/broadcasts/{broadcastId}")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddUrlSegment("broadcastId", broadcastId);
        }

        public RestRequest GetBroadcast(Guid broadcastId)
        {
            return GetRequest("/admin/api/{apiVersion}/broadcasts/{broadcastId}")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddUrlSegment("broadcastId", broadcastId);
        }

        public RestRequest GetBroadcasts()
        {
            return GetRequest("/admin/api/{apiVersion}/broadcasts")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion);
        }
    }
}

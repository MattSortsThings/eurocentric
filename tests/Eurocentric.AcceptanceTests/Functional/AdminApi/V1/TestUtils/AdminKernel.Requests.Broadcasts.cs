using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed partial class AdminKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.IBroadcastsEndpoints
    {
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

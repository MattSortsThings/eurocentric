using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed partial class AdminKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.IBroadcastsEndpoints
    {
        public RestRequest GetBroadcasts() =>
            GetRequest("/admin/api/{apiVersion}/broadcasts").UseSecretApiKey().AddUrlSegment("apiVersion", apiVersion);
    }
}

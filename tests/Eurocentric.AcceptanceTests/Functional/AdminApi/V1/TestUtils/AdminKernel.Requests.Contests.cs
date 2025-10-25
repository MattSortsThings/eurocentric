using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed partial class AdminKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.IContestsEndpoints
    {
        public RestRequest GetContests() =>
            GetRequest("/admin/api/{apiVersion}/contests").UseSecretApiKey().AddUrlSegment("apiVersion", apiVersion);
    }
}

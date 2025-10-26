using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Features.Contests;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed partial class AdminKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.IContestsEndpoints
    {
        public RestRequest CreateContest(CreateContestRequest request) =>
            PostRequest("/admin/api/{apiVersion}/contests")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddJsonBody(request);

        public RestRequest GetContest(Guid contestId) =>
            GetRequest("/admin/api/{apiVersion}/contests/{contestId}")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddUrlSegment("contestId", contestId);

        public RestRequest GetContests() =>
            GetRequest("/admin/api/{apiVersion}/contests").UseSecretApiKey().AddUrlSegment("apiVersion", apiVersion);
    }
}

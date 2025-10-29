using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Features.Contests;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed partial class AdminKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.IContestsEndpoints
    {
        public RestRequest CreateContest(CreateContestRequest requestBody)
        {
            return PostRequest("/admin/api/{apiVersion}/contests")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddJsonBody(requestBody);
        }

        public RestRequest GetContest(Guid contestId)
        {
            return GetRequest("/admin/api/{apiVersion}/contests/{contestId}")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddUrlSegment("contestId", contestId);
        }

        public RestRequest GetContests()
        {
            return GetRequest("/admin/api/{apiVersion}/contests")
                .UseSecretApiKey()
                .AddUrlSegment("apiVersion", apiVersion);
        }
    }
}

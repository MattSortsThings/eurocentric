using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

public sealed partial class AdminApiV1Driver : IAdminApiV1Driver.IContests
{
    public async Task<ProblemOrResponse<CreateContestResponse>> CreateContest(CreateContestRequest requestBody,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = Post("/admin/api/{apiVersion}/contests")
            .UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddJsonBody(requestBody);

        return await _restClient.SendRequestAsync<CreateContestResponse>(request, cancellationToken);
    }

    public async Task<ProblemOrResponse<CreateChildBroadcastResponse>> CreateChildBroadcast(Guid contestId,
        CreateChildBroadcastRequest requestBody,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = Post("/admin/api/{apiVersion}/contests/{contestId}/broadcasts")
            .UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("contestId", contestId)
            .AddJsonBody(requestBody);

        return await _restClient.SendRequestAsync<CreateChildBroadcastResponse>(request, cancellationToken);
    }

    public async Task<ProblemOrResponse<GetContestResponse>> GetContest(Guid contestId,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = Get("/admin/api/{apiVersion}/contests/{contestId}")
            .UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("contestId", contestId);

        return await _restClient.SendRequestAsync<GetContestResponse>(request, cancellationToken);
    }

    public async Task<ProblemOrResponse<GetContestsResponse>> GetContests(CancellationToken cancellationToken = default)
    {
        RestRequest request = Get("/admin/api/{apiVersion}/contests")
            .UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion);

        return await _restClient.SendRequestAsync<GetContestsResponse>(request, cancellationToken);
    }
}

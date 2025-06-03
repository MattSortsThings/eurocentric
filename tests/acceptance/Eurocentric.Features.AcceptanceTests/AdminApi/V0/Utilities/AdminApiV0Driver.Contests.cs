using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V0.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utilities;

public sealed partial class AdminApiV0Driver : IAdminApiV0Driver.IContests
{
    public async Task<ResponseOrProblem<CreateContestResponse>> CreateContest(CreateContestRequest requestBody,
        CancellationToken cancellationToken = default)
    {
        RestRequest restRequest = Post("/admin/api/{apiVersion}/contests")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddJsonBody(requestBody);

        return await _restClient.SendRequestAsync<CreateContestResponse>(restRequest, cancellationToken);
    }

    public async Task<ResponseOrProblem<GetContestResponse>> GetContest(Guid contestId,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = Get("/admin/api/{apiVersion}/contests/{contestId}")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("contestId", contestId);

        return await _restClient.SendRequestAsync<GetContestResponse>(request, cancellationToken);
    }

    public async Task<ResponseOrProblem<GetContestsResponse>> GetContests(CancellationToken cancellationToken = default)
    {
        RestRequest request = Get("/admin/api/{apiVersion}/contests")
            .AddUrlSegment("apiVersion", _apiVersion);

        return await _restClient.SendRequestAsync<GetContestsResponse>(request, cancellationToken);
    }
}

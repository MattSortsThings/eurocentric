using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V0.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;

public sealed class AdminApiV0Driver
{
    private readonly string _apiVersion;
    private readonly ITestClient _client;

    private AdminApiV0Driver(ITestClient client, string apiVersion)
    {
        _client = client;
        _apiVersion = apiVersion;
    }

    public async Task<ResponseOrProblem<CreateContest.Response>> CreateContestAsync(CreateContest.Request requestBody,
        CancellationToken cancellationToken)
    {
        RestRequest request = new("admin/api/v{apiVersion}/contests", Method.Post);

        request.AddUrlSegment("apiVersion", _apiVersion)
            .AddJsonBody(requestBody)
            .UseSecretApiKey();

        return await _client.SendRequestAsync<CreateContest.Response>(request, cancellationToken);
    }

    public async Task<ResponseOrProblem<GetContest.Response>> GetContestAsync(Guid contestId,
        CancellationToken cancellationToken)
    {
        RestRequest request = new("admin/api/v{apiVersion}/contests/{contestId}");

        request.AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("contestId", contestId)
            .UseSecretApiKey();

        return await _client.SendRequestAsync<GetContest.Response>(request, cancellationToken);
    }

    public async Task<ResponseOrProblem<GetContests.Response>> GetContestsAsync(CancellationToken cancellationToken)
    {
        RestRequest request = new("admin/api/v{apiVersion}/contests");

        request.AddUrlSegment("apiVersion", _apiVersion)
            .UseSecretApiKey();

        return await _client.SendRequestAsync<GetContests.Response>(request, cancellationToken);
    }

    public static AdminApiV0Driver Create(ITestClient client, int majorVersion, int minorVersion)
    {
        string routePrefix = $"{majorVersion}.{minorVersion}";

        return new AdminApiV0Driver(client, routePrefix);
    }
}

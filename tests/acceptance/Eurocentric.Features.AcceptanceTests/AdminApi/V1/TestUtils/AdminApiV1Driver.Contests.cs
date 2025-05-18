using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

public sealed partial class AdminApiV1Driver
{
    public async Task<ResponseOrProblem<GetContestResponse>> GetContestAsync(Guid contestId,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = new("/admin/api/v{apiVersion}/contests/{contestId}");

        request.UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("contestId", contestId);

        return await _client.SendRequestAsync<GetContestResponse>(request, cancellationToken);
    }
}

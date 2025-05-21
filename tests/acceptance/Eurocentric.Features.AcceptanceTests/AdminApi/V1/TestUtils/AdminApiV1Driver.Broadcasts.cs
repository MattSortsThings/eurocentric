using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

public sealed partial class AdminApiV1Driver
{
    public async Task<ResponseOrProblem<GetBroadcastResponse>> GetBroadcastAsync(Guid broadcastId,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = new("/admin/api/v{apiVersion}/broadcasts/{broadcastId}");

        request.UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("broadcastId", broadcastId);

        return await _client.SendRequestAsync<GetBroadcastResponse>(request, cancellationToken);
    }
}

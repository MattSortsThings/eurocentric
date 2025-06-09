using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

public sealed partial class AdminApiV1Driver : IAdminApiV1Driver.IBroadcasts
{
    public async Task<ProblemOrResponse<GetBroadcastResponse>> GetBroadcast(Guid broadcastId,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = Get("/admin/api/{apiVersion}/broadcasts/{broadcastId}")
            .UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("broadcastId", broadcastId);

        return await _restClient.SendRequestAsync<GetBroadcastResponse>(request, cancellationToken);
    }

    public async Task<ProblemOrResponse<GetBroadcastsResponse>> GetBroadcasts(CancellationToken cancellationToken = default)
    {
        RestRequest request = Get("/admin/api/{apiVersion}/broadcasts")
            .UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion);

        return await _restClient.SendRequestAsync<GetBroadcastsResponse>(request, cancellationToken);
    }
}

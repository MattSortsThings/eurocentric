using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

internal static partial class AdminApiV1DriverExtensions
{
    internal static async Task<Broadcast> GetABroadcastAsync(this IAdminApiV1Driver.IBroadcasts driver,
        Guid broadcastId,
        CancellationToken cancellationToken = default)
    {
        ProblemOrResponse<GetBroadcastResponse> problemOrResponse = await driver.GetBroadcast(broadcastId, cancellationToken);

        return problemOrResponse.AsResponse.Data!.Broadcast;
    }

    internal static async Task<Broadcast[]> GetAllBroadcastsAsync(this IAdminApiV1Driver.IBroadcasts driver,
        CancellationToken cancellationToken = default)
    {
        ProblemOrResponse<GetBroadcastsResponse> problemOrResponse = await driver.GetBroadcasts(cancellationToken);

        return problemOrResponse.AsResponse.Data!.Broadcasts;
    }
}

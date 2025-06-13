using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

internal static partial class AdminApiV1DriverExtensions
{
    internal static async Task AwardASetOfJuryPointsAsync(this IAdminApiV1Driver.IBroadcasts driver,
        Guid[]? rankedCompetingCountryIds = null,
        Guid broadcastId = default,
        Guid votingCountryId = default, CancellationToken cancellationToken = default)
    {
        AwardJuryPointsRequest requestBody = new()
        {
            VotingCountryId = votingCountryId, RankedCompetingCountryIds = rankedCompetingCountryIds ?? []
        };

        _ = await driver.AwardJuryPoints(broadcastId, requestBody, cancellationToken);
    }

    internal static async Task AwardASetOfTelevotePointsAsync(this IAdminApiV1Driver.IBroadcasts driver,
        Guid[]? rankedCompetingCountryIds = null,
        Guid broadcastId = default,
        Guid votingCountryId = default, CancellationToken cancellationToken = default)
    {
        AwardTelevotePointsRequest requestBody = new()
        {
            VotingCountryId = votingCountryId, RankedCompetingCountryIds = rankedCompetingCountryIds ?? []
        };

        _ = await driver.AwardTelevotePoints(broadcastId, requestBody, cancellationToken);
    }

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

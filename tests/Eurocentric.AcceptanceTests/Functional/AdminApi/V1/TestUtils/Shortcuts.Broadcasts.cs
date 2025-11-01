using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Broadcasts;
using Eurocentric.Apis.Admin.V1.Features.Contests;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static partial class Shortcuts
{
    public static async Task AwardBroadcastTelevotePointsAsync(
        this AdminKernel kernel,
        Guid broadcastId,
        params AwardBroadcastTelevotePointsRequest[] requestBodies
    )
    {
        foreach (AwardBroadcastTelevotePointsRequest requestBody in requestBodies)
        {
            RestRequest request = kernel.Requests.Broadcasts.AwardBroadcastTelevotePoints(broadcastId, requestBody);
            _ = await kernel.Client.SendAsync(request);
        }
    }

    public static async Task<Broadcast> CreateABroadcastAsync(
        this AdminKernel kernel,
        Guid?[] competingCountryIds = null!,
        ContestStage contestStage = default,
        DateOnly broadcastDate = default,
        Guid contestId = default
    )
    {
        CreateContestBroadcastRequest requestBody = new()
        {
            BroadcastDate = broadcastDate,
            ContestStage = contestStage,
            CompetingCountryIds = competingCountryIds,
        };

        RestRequest request = kernel.Requests.Contests.CreateContestBroadcast(contestId, requestBody);
        ProblemOrResponse<CreateContestBroadcastResponse> response =
            await kernel.Client.SendAsync<CreateContestBroadcastResponse>(request);

        return response.AsResponse.Data!.Broadcast;
    }

    public static async Task DeleteABroadcastAsync(this AdminKernel kernel, Guid broadcastId)
    {
        RestRequest request = kernel.Requests.Broadcasts.DeleteBroadcast(broadcastId);
        _ = await kernel.Client.SendAsync(request);
    }

    public static async Task<Broadcast> GetABroadcastAsync(this AdminKernel kernel, Guid broadcastId)
    {
        RestRequest request = kernel.Requests.Broadcasts.GetBroadcast(broadcastId);
        ProblemOrResponse<GetBroadcastResponse> response = await kernel.Client.SendAsync<GetBroadcastResponse>(request);

        return response.AsResponse.Data!.Broadcast;
    }

    public static async Task<Broadcast[]> GetAllBroadcastsAsync(this AdminKernel kernel)
    {
        RestRequest request = kernel.Requests.Broadcasts.GetBroadcasts();
        ProblemOrResponse<GetBroadcastsResponse> response = await kernel.Client.SendAsync<GetBroadcastsResponse>(
            request
        );

        return response.AsResponse.Data!.Broadcasts;
    }
}

using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Broadcasts.AwardTelevotePoints;
using Eurocentric.Features.AdminApi.V1.Broadcasts.GetBroadcast;
using Eurocentric.Features.AdminApi.V1.Broadcasts.GetBroadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests.CreateChildBroadcast;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Broadcasts;

public static class ApiDriverExtensions
{
    public static async Task<Broadcast> CreateSingleBroadcastAsync(this IApiDriver driver,
        Guid[] competingCountryIds = null!,
        Guid contestId = default,
        ContestStage contestStage = default,
        DateOnly broadcastDate = default)
    {
        CreateChildBroadcastRequest requestBody = new()
        {
            ContestStage = contestStage, BroadcastDate = broadcastDate, CompetingCountryIds = competingCountryIds
        };

        RestRequest request = driver.RequestFactory.Contests.CreateChildBroadcast(contestId, requestBody);
        ProblemOrResponse<CreateChildBroadcastResponse> response =
            await driver.RestClient.SendAsync<CreateChildBroadcastResponse>(request);

        return response.AsResponse.Data!.Broadcast;
    }

    public static async Task<Broadcast> GetSingleBroadcastAsync(this IApiDriver driver, Guid broadcastId)
    {
        RestRequest request = driver.RequestFactory.Broadcasts.GetBroadcast(broadcastId);
        ProblemOrResponse<GetBroadcastResponse> response = await driver.RestClient.SendAsync<GetBroadcastResponse>(request);

        return response.AsResponse.Data!.Broadcast;
    }

    public static async Task<Broadcast[]> GetAllBroadcastsAsync(this IApiDriver driver)
    {
        RestRequest request = driver.RequestFactory.Broadcasts.GetBroadcasts();
        ProblemOrResponse<GetBroadcastsResponse> response = await driver.RestClient.SendAsync<GetBroadcastsResponse>(request);

        return response.AsResponse.Data!.Broadcasts;
    }

    public static async Task DeleteSingleBroadcastAsync(this IApiDriver driver, Guid broadcastId)
    {
        RestRequest request = driver.RequestFactory.Broadcasts.DeleteBroadcast(broadcastId);
        ProblemOrResponse response = await driver.RestClient.SendAsync(request);

        await Assert.That(response.IsT1).IsTrue();
    }

    public static async Task AwardMultipleTelevotePointsAsync(this IApiDriver driver,
        Guid broadcastId,
        IEnumerable<AwardTelevotePointsRequest> requestBodies)
    {
        foreach (AwardTelevotePointsRequest requestBody in requestBodies)
        {
            RestRequest request = driver.RequestFactory.Broadcasts.AwardTelevotePoints(broadcastId, requestBody);
            ProblemOrResponse response = await driver.RestClient.SendAsync(request);
            await Assert.That(response.IsT1).IsTrue();
        }
    }
}

using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Broadcasts.AwardJuryPoints;
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

    public static async Task AwardMultipleJuryPointsAsync(this IApiDriver driver,
        Guid broadcastId,
        IEnumerable<AwardJuryPointsRequest> requestBodies)
    {
        foreach (AwardJuryPointsRequest requestBody in requestBodies)
        {
            RestRequest request = driver.RequestFactory.Broadcasts.AwardJuryPoints(broadcastId, requestBody);
            ProblemOrResponse response = await driver.RestClient.SendAsync(request);
            await Assert.That(response.IsT1).IsTrue();
        }
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

    public static async Task AwardAllJuryPointsAsync(this IApiDriver driver, Broadcast broadcast)
    {
        IEnumerable<AwardJuryPointsRequest> requestBodies = broadcast.GenerateAllAwardJuryPointsRequests();

        await driver.AwardMultipleJuryPointsAsync(broadcast.Id, requestBodies);
    }

    public static async Task AwardAllTelevotePointsAsync(this IApiDriver driver, Broadcast broadcast)
    {
        IEnumerable<AwardTelevotePointsRequest> requestBodies = broadcast.GenerateAllAwardTelevotePointsRequests();

        await driver.AwardMultipleTelevotePointsAsync(broadcast.Id, requestBodies);
    }

    private static IEnumerable<AwardJuryPointsRequest> GenerateAllAwardJuryPointsRequests(this Broadcast broadcast)
    {
        Guid[] allCompetingCountryIds = broadcast.Competitors.Select(competitor => competitor.CompetingCountryId).ToArray();

        foreach (Voter televote in broadcast.Juries.Where(voter => !voter.PointsAwarded))
        {
            Guid votingCountryId = televote.VotingCountryId;

            yield return new AwardJuryPointsRequest
            {
                VotingCountryId = votingCountryId,
                RankedCompetingCountryIds = allCompetingCountryIds.Where(id => id != votingCountryId).ToArray()
            };
        }
    }

    private static IEnumerable<AwardTelevotePointsRequest> GenerateAllAwardTelevotePointsRequests(this Broadcast broadcast)
    {
        Guid[] allCompetingCountryIds = broadcast.Competitors.Select(competitor => competitor.CompetingCountryId).ToArray();

        foreach (Voter televote in broadcast.Televotes.Where(voter => !voter.PointsAwarded))
        {
            Guid votingCountryId = televote.VotingCountryId;

            yield return new AwardTelevotePointsRequest
            {
                VotingCountryId = votingCountryId,
                RankedCompetingCountryIds = allCompetingCountryIds.Where(id => id != votingCountryId).ToArray()
            };
        }
    }
}

using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Broadcasts;
using Eurocentric.Apis.Admin.V1.Features.Contests;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static partial class Shortcuts
{
    public static async Task AwardAllBroadcastJuryPointsAsync(
        this AdminKernel kernel,
        Broadcast broadcast,
        Guid? excludedVotingCountryId = null
    )
    {
        Guid broadcastId = broadcast.Id;
        AwardBroadcastJuryPointsRequest[] requestBodies = GenerateJuryRequestBodies(broadcast, excludedVotingCountryId);

        await kernel.AwardBroadcastJuryPointsAsync(broadcastId, requestBodies);
    }

    public static async Task AwardAllBroadcastTelevotePointsAsync(
        this AdminKernel kernel,
        Broadcast broadcast,
        Guid? excludedVotingCountryId = null
    )
    {
        Guid broadcastId = broadcast.Id;
        AwardBroadcastTelevotePointsRequest[] requestBodies = GenerateTelevoteRequestBodies(
            broadcast,
            excludedVotingCountryId
        );

        await kernel.AwardBroadcastTelevotePointsAsync(broadcastId, requestBodies);
    }

    public static async Task AwardBroadcastJuryPointsAsync(
        this AdminKernel kernel,
        Guid broadcastId,
        params AwardBroadcastJuryPointsRequest[] requestBodies
    )
    {
        foreach (AwardBroadcastJuryPointsRequest requestBody in requestBodies)
        {
            RestRequest request = kernel.Requests.Broadcasts.AwardBroadcastJuryPoints(broadcastId, requestBody);
            _ = await kernel.Client.SendAsync(request);
        }
    }

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

    private static AwardBroadcastTelevotePointsRequest[] GenerateTelevoteRequestBodies(
        Broadcast broadcast,
        Guid? excludedVotingCountryId = null
    )
    {
        Guid[] competingCountryIds = broadcast
            .Competitors.Select(competitor => competitor.CompetingCountryId)
            .ToArray();

        IEnumerable<Televote> televotes = broadcast.Televotes.Where(televote =>
            televote.VotingCountryId != excludedVotingCountryId
        );

        return televotes
            .Select(televote =>
            {
                Guid votingCountryId = televote.VotingCountryId;

                return new AwardBroadcastTelevotePointsRequest
                {
                    VotingCountryId = votingCountryId,
                    RankedCompetingCountryIds = competingCountryIds
                        .Where(countryId => countryId != votingCountryId)
                        .ToArray(),
                };
            })
            .ToArray();
    }

    private static AwardBroadcastJuryPointsRequest[] GenerateJuryRequestBodies(
        Broadcast broadcast,
        Guid? excludedVotingCountryId = null
    )
    {
        Guid[] competingCountryIds = broadcast
            .Competitors.Select(competitor => competitor.CompetingCountryId)
            .ToArray();

        IEnumerable<Jury> juries = broadcast.Juries.Where(jury => jury.VotingCountryId != excludedVotingCountryId);

        return juries
            .Select(jury =>
            {
                Guid votingCountryId = jury.VotingCountryId;

                return new AwardBroadcastJuryPointsRequest
                {
                    VotingCountryId = votingCountryId,
                    RankedCompetingCountryIds = competingCountryIds
                        .Where(countryId => countryId != votingCountryId)
                        .ToArray(),
                };
            })
            .ToArray();
    }
}

using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Broadcasts;

public static class AdminActorMixins
{
    public static async Task Given_I_have_deleted_my_broadcast(this IAdminActor admin)
    {
        Broadcast myBroadcast = admin.GivenBroadcasts.GetSingle();

        RestRequest request = admin.RequestFactory.Broadcasts.DeleteBroadcast(myBroadcast.Id);

        ProblemOrResponse problemOrResponse = await admin.RestClient.SendAsync(request, TestContext.Current.CancellationToken);

        Assert.True(problemOrResponse.IsT1);
    }

    public static async Task Then_no_broadcasts_should_exist(this IAdminActor admin)
    {
        Broadcast[] existingBroadcasts = await admin.GetAllExistingBroadcastsAsync();

        Assert.Empty(existingBroadcasts);
    }

    public static void Then_the_response_problem_details_extensions_should_include_my_broadcast_ID(this IAdminActor admin)
    {
        Broadcast myBroadcast = admin.GivenBroadcasts.GetSingle();

        Assert.NotNull(admin.ResponseProblemDetails);

        Assert.Contains(admin.ResponseProblemDetails.Extensions, kvp => kvp is { Key: "broadcastId", Value: JsonElement je }
                                                                        && je.GetGuid() == myBroadcast.Id);
    }

    public static async Task<Broadcast[]> GetAllExistingBroadcastsAsync(this IAdminActor admin)
    {
        RestRequest request = admin.RequestFactory.Broadcasts.GetBroadcasts();

        ProblemOrResponse<GetBroadcastsResponse> problemOrResponse =
            await admin.RestClient.SendAsync<GetBroadcastsResponse>(request, TestContext.Current.CancellationToken);

        return problemOrResponse.AsResponse.Data!.Broadcasts;
    }
}

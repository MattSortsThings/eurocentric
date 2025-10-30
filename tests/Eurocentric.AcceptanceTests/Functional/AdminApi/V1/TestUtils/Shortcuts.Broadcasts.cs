using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Features.Broadcasts;
using Eurocentric.Apis.Admin.V1.Features.Contests;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using ApiContestStage = Eurocentric.Apis.Admin.V1.Enums.ContestStage;
using BroadcastDto = Eurocentric.Apis.Admin.V1.Dtos.Broadcasts.Broadcast;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static partial class Shortcuts
{
    public static async Task<BroadcastDto> CreateABroadcastAsync(
        this AdminKernel kernel,
        Guid?[] competingCountryIds = null!,
        ApiContestStage contestStage = default,
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
        BroadcastId id = BroadcastId.FromValue(broadcastId);

        await kernel.BackDoor.ExecuteScopedAsync(DeleteAsync(id));
    }

    public static async Task<BroadcastDto[]> GetAllBroadcastsAsync(this AdminKernel kernel)
    {
        RestRequest request = kernel.Requests.Broadcasts.GetBroadcasts();
        ProblemOrResponse<GetBroadcastsResponse> response = await kernel.Client.SendAsync<GetBroadcastsResponse>(
            request
        );

        return response.AsResponse.Data!.Broadcasts;
    }

    private static Func<IServiceProvider, Task> DeleteAsync(BroadcastId broadcastId)
    {
        BroadcastId id = broadcastId;

        return async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Broadcasts.Where(broadcast => broadcast.Id.Equals(id)).ExecuteDeleteAsync();
        };
    }
}

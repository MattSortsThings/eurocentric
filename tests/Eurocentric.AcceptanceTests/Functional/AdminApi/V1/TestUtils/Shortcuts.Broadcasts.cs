using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using ApiContestStage = Eurocentric.Apis.Admin.V1.Enums.ContestStage;
using BroadcastAggregate = Eurocentric.Domain.Aggregates.Broadcasts.Broadcast;
using BroadcastDto = Eurocentric.Apis.Admin.V1.Dtos.Broadcasts.Broadcast;
using DomainContestStage = Eurocentric.Domain.Enums.ContestStage;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static partial class Shortcuts
{
    public static async Task<BroadcastDto> CreateABroadcastWithDummyContestAndCountriesAsync(
        this AdminKernel kernel,
        ApiContestStage contestStage = default,
        DateOnly broadcastDate = default
    )
    {
        BroadcastId id = BroadcastId.FromValue(Guid.NewGuid());
        BroadcastDate? date = BroadcastDate.FromValue(broadcastDate).GetValueOrDefault();
        DomainContestStage stage = (DomainContestStage)(int)contestStage;

        BroadcastAggregate aggregate = BroadcastAggregate.CreateDummyBroadcast(id, date, stage);
        Broadcast dto = aggregate.ToDto();

        await kernel.BackDoor.ExecuteScopedAsync(PersistAsync(aggregate));

        return dto;
    }

    private static Func<IServiceProvider, Task> PersistAsync(BroadcastAggregate aggregate)
    {
        BroadcastAggregate broadcast = aggregate;

        return async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            dbContext.Broadcasts.Add(broadcast);
            await dbContext.SaveChangesAsync();
        };
    }
}

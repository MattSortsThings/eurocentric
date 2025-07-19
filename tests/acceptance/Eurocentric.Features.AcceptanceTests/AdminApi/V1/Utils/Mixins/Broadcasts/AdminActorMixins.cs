using Eurocentric.Domain.Identifiers;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Broadcasts;

public static class AdminActorMixins
{
    public static async Task Given_I_have_deleted_my_broadcast(this IAdminActor admin)
    {
        BroadcastId broadcastId = BroadcastId.FromValue(admin.GivenBroadcasts.GetSingle().Id);

        Func<IServiceProvider, Task> deleteAsync = async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();

            await dbContext.Broadcasts.Where(broadcast => broadcast.Id == broadcastId).ExecuteDeleteAsync();
        };

        await admin.BackDoor.ExecuteScopedAsync(deleteAsync);
    }
}

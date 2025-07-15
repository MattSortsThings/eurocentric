using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Broadcasts;

internal static class AdminActorMixins
{
    internal static async Task Given_I_have_created_a_child_broadcast_for_my_contest(this IAdminActor admin,
        string[] competingCountryCodes = null!,
        string contestStage = "",
        string broadcastDate = "")
    {
        ContestId myContestId = ContestId.FromValue(admin.GivenContests.Single().Id);
        CountryId[] countryIds = competingCountryCodes.Select(admin.GivenCountries.GetId)
            .Select(CountryId.FromValue)
            .ToArray();
        DateOnly date = DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd");
        ContestStage stage = Enum.Parse<ContestStage>(contestStage);

        List<Competitor> competitors = countryIds.Select((id, index) =>
                new Competitor(id, index + 1))
            .ToList();

        List<Jury> juries = countryIds.Select(id => new Jury(id)).ToList();

        List<Televote> televotes = countryIds.Select(id => new Televote(id)).ToList();

        Broadcast broadcast = new(BroadcastId.Create(DateTimeOffset.UtcNow),
            date,
            myContestId,
            stage,
            competitors,
            juries,
            televotes);

        Func<IServiceProvider, Task> persistBroadcastAsync = async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();

            dbContext.Broadcasts.Add(broadcast);

            await dbContext.SaveChangesAsync();
        };

        await admin.BackDoor.ExecuteScopedAsync(persistBroadcastAsync);

        admin.GivenBroadcasts.Add(broadcast.ToBroadcastDto());
    }

    internal static async Task Given_I_have_deleted_every_broadcast_I_have_created(this IAdminActor admin)
    {
        HashSet<BroadcastId> broadcastIds = admin.GivenBroadcasts.Select(x => x.Id)
            .Select(BroadcastId.FromValue)
            .ToHashSet();

        Func<IServiceProvider, Task> deleteCountriesAsync = async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();

            await dbContext.Broadcasts.Where(broadcast => broadcastIds.Contains(broadcast.Id))
                .ExecuteDeleteAsync();
        };

        await admin.BackDoor.ExecuteScopedAsync(deleteCountriesAsync);
    }
}

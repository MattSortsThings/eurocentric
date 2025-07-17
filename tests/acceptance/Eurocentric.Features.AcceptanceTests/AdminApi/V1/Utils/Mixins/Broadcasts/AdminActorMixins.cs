using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Broadcast = Eurocentric.Domain.Aggregates.Broadcasts.Broadcast;
using Competitor = Eurocentric.Domain.Aggregates.Broadcasts.Competitor;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Broadcasts;

public static class AdminActorMixins
{
    public static async Task Given_I_have_created_a_child_broadcast_for_my_contest(this IAdminActor admin,
        string[] competingCountryCodes = null!,
        string broadcastDate = "",
        string contestStage = "")
    {
        ContestId contestId = ContestId.FromValue(admin.GivenContests.GetSingle().Id);
        DateOnly date = DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd");
        ContestStage stage = Enum.Parse<ContestStage>(contestStage);

        List<Competitor> competitors = competingCountryCodes.Select(admin.GivenCountries.LookupId)
            .Select(CountryId.FromValue)
            .Select((countryId, index) => new Competitor(countryId, index + 1))
            .ToList();

        List<Jury> juries = competitors.Select(competitor => competitor.CompetingCountryId)
            .Select(countryId => new Jury(countryId))
            .ToList();

        List<Televote> televotes = competitors.Select(competitor => competitor.CompetingCountryId)
            .Select(countryId => new Televote(countryId))
            .ToList();

        Broadcast broadcast = new(BroadcastId.Create(DateTimeOffset.UtcNow),
            date,
            contestId,
            stage,
            competitors, juries, televotes);

        Func<IServiceProvider, Task> persistAsync = async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();

            dbContext.Broadcasts.Add(broadcast);
            await dbContext.SaveChangesAsync();
        };

        await admin.BackDoor.ExecuteScopedAsync(persistAsync);

        admin.GivenBroadcasts.Add(broadcast.ToBroadcastDto());
    }

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

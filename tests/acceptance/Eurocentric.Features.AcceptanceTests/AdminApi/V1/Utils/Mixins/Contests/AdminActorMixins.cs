using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;

internal static class AdminActorMixins
{
    internal static async Task Given_I_have_created_a_Stockholm_format_contest(this IAdminActor admin,
        string[] group2Countries = null!,
        string[] group1Countries = null!,
        int contestYear = 0,
        string cityName = "")
    {
        IEnumerable<Participant> participants = admin.CreateGroup1Participants(group1Countries)
            .Concat(admin.CreateGroup2Participants(group2Countries));

        StockholmFormatContest contest = new(ContestId.Create(DateTimeOffset.UtcNow),
            participants.ToList(),
            ContestYear.FromValue(contestYear).Value,
            CityName.FromValue(cityName).Value);

        Func<IServiceProvider, Task> persistAsync = async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            dbContext.Contests.Add(contest);
            await dbContext.SaveChangesAsync();
        };

        await admin.BackDoor.ExecuteScopedAsync(persistAsync);

        admin.GivenContests.Add(contest.ToContestDto());
    }

    internal static async Task Given_I_have_created_a_Liverpool_format_contest(this IAdminActor admin,
        string[] group2Countries = null!,
        string[] group1Countries = null!,
        string group0Country = "",
        int contestYear = 0,
        string cityName = "")
    {
        IEnumerable<Participant> participants = admin.CreateGroup1Participants(group1Countries)
            .Concat(admin.CreateGroup2Participants(group2Countries))
            .Append(admin.CreateGroup0Participant(group0Country));

        LiverpoolFormatContest contest = new(ContestId.Create(DateTimeOffset.UtcNow),
            participants.ToList(),
            ContestYear.FromValue(contestYear).Value,
            CityName.FromValue(cityName).Value);

        Func<IServiceProvider, Task> persistAsync = async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            dbContext.Contests.Add(contest);
            await dbContext.SaveChangesAsync();
        };

        await admin.BackDoor.ExecuteScopedAsync(persistAsync);

        admin.GivenContests.Add(contest.ToContestDto());
    }

    internal static async Task Given_I_have_deleted_every_contest_I_have_created(this IAdminActor admin)
    {
        HashSet<ContestId> contestIds = admin.GivenContests.Select(x => x.Id)
            .Select(ContestId.FromValue)
            .ToHashSet();

        Func<IServiceProvider, Task> deleteContestsAsync = async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();

            IEnumerable<Contest> contestsToDelete = dbContext.Contests.AsSplitQuery()
                .AsEnumerable()
                .Join(contestIds,
                    x => x.Id,
                    y => y,
                    (x, _) => x);

            dbContext.Contests.RemoveRange(contestsToDelete);

            await dbContext.SaveChangesAsync();
        };

        await admin.BackDoor.ExecuteScopedAsync(deleteContestsAsync);
    }

    private static Participant CreateGroup0Participant(this IAdminActor admin, string countryCode)
    {
        CountryId countryId = CountryId.FromValue(admin.GivenCountries.First(country => country.CountryCode == countryCode).Id);

        return Participant.CreateInGroup0(countryId);
    }

    private static IEnumerable<Participant> CreateGroup1Participants(this IAdminActor admin, IEnumerable<string> countryCodes) =>
        from countryCode in countryCodes
        let countryId = CountryId.FromValue(admin.GivenCountries.First(country => country.CountryCode == countryCode).Id)
        let actName = ActName.FromValue(countryCode + " Act")
        let songTitle = SongTitle.FromValue(countryCode + " Song")
        select Participant.CreateInGroup1(countryId, actName, songTitle).Value;

    private static IEnumerable<Participant> CreateGroup2Participants(this IAdminActor admin, IEnumerable<string> countryCodes) =>
        from countryCode in countryCodes
        let countryId = CountryId.FromValue(admin.GivenCountries.First(country => country.CountryCode == countryCode).Id)
        let actName = ActName.FromValue(countryCode + " Act")
        let songTitle = SongTitle.FromValue(countryCode + " Song")
        select Participant.CreateInGroup2(countryId, actName, songTitle).Value;
}

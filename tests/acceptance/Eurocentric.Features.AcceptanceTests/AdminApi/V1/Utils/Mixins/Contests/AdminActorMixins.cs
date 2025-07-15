using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using Contest = Eurocentric.Domain.Aggregates.Contests.Contest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;

internal static class AdminActorMixins
{
    internal static async Task Given_I_have_created_a_Stockholm_format_contest(this IAdminActor admin,
        string[] group2CountryCodes = null!,
        string[] group1CountryCodes = null!,
        int contestYear = 0,
        string cityName = "")
    {
        CreateContestRequest requestBody = new()
        {
            ContestYear = contestYear,
            CityName = cityName,
            ContestFormat = ContestFormat.Stockholm,
            Group0ParticipatingCountryId = null,
            Group1Participants = group1CountryCodes.Select(admin.GivenCountries.GetId).ToContestParticipantSpecifications(),
            Group2Participants = group2CountryCodes.Select(admin.GivenCountries.GetId).ToContestParticipantSpecifications()
        };

        RestRequest request = admin.RequestFactory.Contests.CreateContest(requestBody);

        ProblemOrResponse<CreateContestResponse> problemOrResponse =
            await admin.RestClient.SendAsync<CreateContestResponse>(request, TestContext.Current.CancellationToken);

        admin.GivenContests.Add(problemOrResponse.AsResponse.Data!.Contest);
    }

    internal static async Task Given_I_have_created_a_Liverpool_format_contest(this IAdminActor admin,
        string[] group2CountryCodes = null!,
        string[] group1CountryCodes = null!,
        string group0CountryCode = "",
        int contestYear = 0,
        string cityName = "")
    {
        CreateContestRequest requestBody = new()
        {
            ContestYear = contestYear,
            CityName = cityName,
            ContestFormat = ContestFormat.Liverpool,
            Group0ParticipatingCountryId = admin.GivenCountries.GetId(group0CountryCode),
            Group1Participants = group1CountryCodes.Select(admin.GivenCountries.GetId).ToContestParticipantSpecifications(),
            Group2Participants = group2CountryCodes.Select(admin.GivenCountries.GetId).ToContestParticipantSpecifications()
        };

        RestRequest request = admin.RequestFactory.Contests.CreateContest(requestBody);

        ProblemOrResponse<CreateContestResponse> problemOrResponse =
            await admin.RestClient.SendAsync<CreateContestResponse>(request, TestContext.Current.CancellationToken);

        admin.GivenContests.Add(problemOrResponse.AsResponse.Data!.Contest);
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
}

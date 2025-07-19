using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;

public static class AdminActorExtensions
{
    public static async Task Given_I_have_created_a_Stockholm_format_contest(this IAdminActor admin,
        string[] group2CountryCodes = null!,
        string[] group1CountryCodes = null!,
        string cityName = "",
        int contestYear = 0)
    {
        CreateContestRequest requestBody = new()
        {
            ContestYear = contestYear,
            CityName = cityName,
            ContestFormat = ContestFormat.Stockholm,
            Group0ParticipatingCountryId = null,
            Group1Participants = group1CountryCodes.Select(admin.GivenCountries.LookupId)
                .Select(DefaultValues.ParticipantSpec)
                .ToArray(),
            Group2Participants = group2CountryCodes.Select(admin.GivenCountries.LookupId)
                .Select(DefaultValues.ParticipantSpec)
                .ToArray()
        };

        Contest createdContest = await admin.CreateContestAsync(requestBody);

        admin.GivenContests.Add(createdContest);
    }

    public static async Task Given_I_have_created_a_Liverpool_format_contest(this IAdminActor admin,
        string[] group2CountryCodes = null!,
        string[] group1CountryCodes = null!,
        string group0CountryCode = "",
        string cityName = "",
        int contestYear = 0)
    {
        CreateContestRequest requestBody = new()
        {
            ContestYear = contestYear,
            CityName = cityName,
            ContestFormat = ContestFormat.Liverpool,
            Group0ParticipatingCountryId = admin.GivenCountries.LookupId(group0CountryCode),
            Group1Participants = group1CountryCodes.Select(admin.GivenCountries.LookupId)
                .Select(DefaultValues.ParticipantSpec)
                .ToArray(),
            Group2Participants = group2CountryCodes.Select(admin.GivenCountries.LookupId)
                .Select(DefaultValues.ParticipantSpec)
                .ToArray()
        };

        Contest createdContest = await admin.CreateContestAsync(requestBody);

        admin.GivenContests.Add(createdContest);
    }

    public static async Task Given_I_have_created_a_child_broadcast_for_my_contest(this IAdminActor admin,
        string[] competingCountryCodes = null!,
        string broadcastDate = "",
        string contestStage = "")
    {
        Contest myContest = admin.GivenContests.GetSingle();

        CreateChildBroadcastRequest requestBody = new()
        {
            ContestStage = Enum.Parse<ContestStage>(contestStage),
            BroadcastDate = DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
            CompetingCountryIds = competingCountryCodes.Select(admin.GivenCountries.LookupId).ToArray()
        };

        Broadcast createdBroadcast = await admin.CreateChildBroadcastAsync(myContest.Id, requestBody);
        admin.GivenBroadcasts.Add(createdBroadcast);

        Contest updatedContest = await admin.GetExistingContestByIdAsync(myContest.Id);
        admin.GivenContests.Replace(updatedContest);
    }

    public static async Task Given_I_have_deleted_my_contest(this IAdminActor admin)
    {
        ContestId contestId = ContestId.FromValue(admin.GivenContests.GetSingle().Id);

        Func<IServiceProvider, Task> deleteAsync = async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();

            await dbContext.Contests.Where(contest => contest.Id == contestId).ExecuteDeleteAsync();
        };

        await admin.BackDoor.ExecuteScopedAsync(deleteAsync);
    }

    private static async Task<Contest> CreateContestAsync(this IAdminActor admin, CreateContestRequest requestBody)
    {
        RestRequest request = admin.RequestFactory.Contests.CreateContest(requestBody);

        ProblemOrResponse<CreateContestResponse> problemOrResponse =
            await admin.RestClient.SendAsync<CreateContestResponse>(request, TestContext.Current.CancellationToken);

        return problemOrResponse.AsResponse.Data!.Contest;
    }

    private static async Task<Broadcast> CreateChildBroadcastAsync(this IAdminActor admin, Guid contestId,
        CreateChildBroadcastRequest requestBody)
    {
        RestRequest request = admin.RequestFactory.Contests.CreateChildBroadcast(contestId, requestBody);
        ProblemOrResponse<CreateChildBroadcastResponse> problemOrResponse =
            await admin.RestClient.SendAsync<CreateChildBroadcastResponse>(request, TestContext.Current.CancellationToken);

        return problemOrResponse.AsResponse.Data!.Broadcast;
    }

    private static async Task<Contest> GetExistingContestByIdAsync(this IAdminActor admin, Guid contestId)
    {
        RestRequest request = admin.RequestFactory.Contests.GetContest(contestId);
        ProblemOrResponse<GetContestResponse> problemOrResponse =
            await admin.RestClient.SendAsync<GetContestResponse>(request, TestContext.Current.CancellationToken);

        return problemOrResponse.AsResponse.Data!.Contest;
    }
}

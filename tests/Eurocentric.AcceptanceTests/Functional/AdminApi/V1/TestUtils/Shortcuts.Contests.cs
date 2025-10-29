using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Contests;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using ContestDto = Eurocentric.Apis.Admin.V1.Dtos.Contests.Contest;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static partial class Shortcuts
{
    public static async Task<ContestDto> CreateALiverpoolRulesContestAsync(
        this AdminKernel kernel,
        Guid[] semiFinal2CountryIds = null!,
        Guid[] semiFinal1CountryIds = null!,
        Guid globalTelevoteVotingCountryId = default,
        string? cityName = null,
        int contestYear = 0
    )
    {
        CreateContestRequest requestBody = new()
        {
            ContestYear = contestYear,
            CityName = cityName ?? TestDefaults.CityName,
            ContestRules = ContestRules.Liverpool,
            GlobalTelevoteVotingCountryId = globalTelevoteVotingCountryId,
            Participants = semiFinal1CountryIds
                .Select(TestDefaults.SemiFinal1ParticipantRequest)
                .Concat(semiFinal2CountryIds.Select(TestDefaults.SemiFinal2ParticipantRequest))
                .ToArray(),
        };

        RestRequest request = kernel.Requests.Contests.CreateContest(requestBody);

        ProblemOrResponse<CreateContestResponse> response = await kernel.Client.SendAsync<CreateContestResponse>(
            request
        );

        return response.AsResponse.Data!.Contest;
    }

    public static async Task<ContestDto> CreateAStockholmRulesContestAsync(
        this AdminKernel kernel,
        Guid[] semiFinal2CountryIds = null!,
        Guid[] semiFinal1CountryIds = null!,
        string? cityName = null,
        int contestYear = 0
    )
    {
        CreateContestRequest requestBody = new()
        {
            ContestYear = contestYear,
            CityName = cityName ?? TestDefaults.CityName,
            ContestRules = ContestRules.Stockholm,
            GlobalTelevoteVotingCountryId = null,
            Participants = semiFinal1CountryIds
                .Select(TestDefaults.SemiFinal1ParticipantRequest)
                .Concat(semiFinal2CountryIds.Select(TestDefaults.SemiFinal2ParticipantRequest))
                .ToArray(),
        };

        RestRequest request = kernel.Requests.Contests.CreateContest(requestBody);

        ProblemOrResponse<CreateContestResponse> response = await kernel.Client.SendAsync<CreateContestResponse>(
            request
        );

        return response.AsResponse.Data!.Contest;
    }

    public static async Task DeleteAContestAsync(this AdminKernel kernel, Guid contestId)
    {
        ContestId id = ContestId.FromValue(contestId);

        await kernel.BackDoor.ExecuteScopedAsync(DeleteAsync(id));
    }

    public static async Task<ContestDto[]> GetAllContestsAsync(this AdminKernel kernel)
    {
        RestRequest request = kernel.Requests.Contests.GetContests();
        ProblemOrResponse<GetContestsResponse> response = await kernel.Client.SendAsync<GetContestsResponse>(request);

        return response.AsResponse.Data!.Contests;
    }

    private static Func<IServiceProvider, Task> DeleteAsync(ContestId contestId)
    {
        ContestId idToDelete = contestId;

        return async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Contests.Where(contest => contest.Id.Equals(idToDelete)).ExecuteDeleteAsync();
        };
    }
}

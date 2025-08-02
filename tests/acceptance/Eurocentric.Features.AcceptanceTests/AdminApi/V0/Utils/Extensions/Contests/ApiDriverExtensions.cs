using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Contests.CreateContest;
using Eurocentric.Features.AdminApi.V0.Contests.GetContest;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Extensions.Contests;

public static class ApiDriverExtensions
{
    public static async Task<Contest> CreateSingleContestAsync(this IApiDriver driver,
        Guid[] countryIds = null!,
        ContestFormat contestFormat = ContestFormat.Liverpool,
        string cityName = "CityName",
        int contestYear = 0)
    {
        CreateContestRequest requestBody = new()
        {
            ContestFormat = contestFormat,
            ContestYear = contestYear,
            CityName = cityName,
            ParticipatingCountryIds = countryIds
        };

        RestRequest request = driver.RequestFactory.Contests.CreateContest(requestBody);
        ProblemOrResponse<CreateContestResponse> problemOrResponse =
            await driver.RestClient.SendAsync<CreateContestResponse>(request);

        return problemOrResponse.AsResponse.Data!.Contest;
    }

    public static async Task DeleteSingleContestAsync(this IApiDriver driver, Guid contestId)
    {
        Guid targetContestId = contestId;
        Func<IServiceProvider, Task> deleteAsync = async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            await dbContext.V0Contests.Where(contest => contest.Id == targetContestId)
                .ExecuteDeleteAsync();
        };

        await driver.BackDoor.ExecuteScopedAsync(deleteAsync);
    }

    public static async Task<Contest> GetSingleContestAsync(this IApiDriver driver, Guid contestId)
    {
        RestRequest request = driver.RequestFactory.Contests.GetContest(contestId);
        ProblemOrResponse<GetContestResponse> problemOrResponse = await driver.RestClient.SendAsync<GetContestResponse>(request);

        return problemOrResponse.AsResponse.Data!.Contest;
    }
}

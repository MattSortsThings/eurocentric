using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Contests.CreateContest;
using Eurocentric.Features.AdminApi.V1.Contests.GetContests;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using ContestDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;
using ContestFormat = Eurocentric.Features.AdminApi.V1.Common.Enums.ContestFormat;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;

public static class ApiDriverExtensions
{
    public static async Task<ContestDto> CreateSingleLiverpoolFormatContestAsync(this IApiDriver driver,
        Guid[] group2CountryIds = null!,
        Guid[] group1CountryIds = null!,
        Guid group0CountryId = default,
        string cityName = TestDefaults.CityName,
        int contestYear = 0)
    {
        CreateContestRequest requestBody = new()
        {
            ContestFormat = ContestFormat.Liverpool,
            ContestYear = contestYear,
            CityName = cityName,
            Group0ParticipatingCountryId = group0CountryId,
            Group1ParticipantData = group1CountryIds.Select(TestDefaults.ParticipantDatum).ToArray(),
            Group2ParticipantData = group2CountryIds.Select(TestDefaults.ParticipantDatum).ToArray()
        };

        RestRequest request = driver.RequestFactory.Contests.CreateContest(requestBody);
        ProblemOrResponse<CreateContestResponse> response = await driver.RestClient.SendAsync<CreateContestResponse>(request);

        return response.AsResponse.Data!.Contest;
    }

    public static async Task<ContestDto> CreateSingleStockholmFormatContestAsync(this IApiDriver driver,
        Guid[] group2CountryIds = null!,
        Guid[] group1CountryIds = null!,
        string cityName = TestDefaults.CityName,
        int contestYear = 0)
    {
        CreateContestRequest requestBody = new()
        {
            ContestFormat = ContestFormat.Stockholm,
            ContestYear = contestYear,
            CityName = cityName,
            Group0ParticipatingCountryId = null,
            Group1ParticipantData = group1CountryIds.Select(TestDefaults.ParticipantDatum).ToArray(),
            Group2ParticipantData = group2CountryIds.Select(TestDefaults.ParticipantDatum).ToArray()
        };

        RestRequest request = driver.RequestFactory.Contests.CreateContest(requestBody);
        ProblemOrResponse<CreateContestResponse> response = await driver.RestClient.SendAsync<CreateContestResponse>(request);

        return response.AsResponse.Data!.Contest;
    }

    public static async Task<ContestDto[]> GetAllContestsAsync(this IApiDriver driver)
    {
        RestRequest request = driver.RequestFactory.Contests.GetContests();
        ProblemOrResponse<GetContestsResponse> response = await driver.RestClient.SendAsync<GetContestsResponse>(request);

        return response.AsResponse.Data!.Contests;
    }

    public static async Task DeleteSingleContestAsync(this IApiDriver driver, Guid contestId)
    {
        ContestId contestIdToDelete = ContestId.FromValue(contestId);

        await driver.BackDoor.ExecuteScopedAsync(DeleteContestAsync(contestIdToDelete));
    }

    private static Func<IServiceProvider, Task> DeleteContestAsync(ContestId contestId)
    {
        ContestId idToDelete = contestId;

        return async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            await dbContext.Contests.Where(c => c.Id == idToDelete).ExecuteDeleteAsync();
        };
    }
}

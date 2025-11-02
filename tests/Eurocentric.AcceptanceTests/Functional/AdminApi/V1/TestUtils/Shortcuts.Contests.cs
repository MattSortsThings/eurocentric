using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Contests;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static partial class Shortcuts
{
    public static async Task<Contest> CreateALiverpoolRulesContestAsync(
        this AdminKernel kernel,
        Guid[] semiFinal2CountryIds = null!,
        Guid[] semiFinal1CountryIds = null!,
        Guid globalTelevoteCountryId = default,
        string? cityName = null,
        int contestYear = 0
    )
    {
        CreateContestRequest requestBody = new()
        {
            ContestYear = contestYear,
            CityName = cityName ?? TestDefaults.CityName,
            ContestRules = ContestRules.Liverpool,
            GlobalTelevoteVotingCountryId = globalTelevoteCountryId,
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

    public static async Task<Contest> CreateAStockholmRulesContestAsync(
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
        RestRequest request = kernel.Requests.Contests.DeleteContest(contestId);
        _ = await kernel.Client.SendAsync(request);
    }

    public static async Task<Contest> GetAContestAsync(this AdminKernel kernel, Guid contestId)
    {
        RestRequest request = kernel.Requests.Contests.GetContest(contestId);
        ProblemOrResponse<GetContestResponse> response = await kernel.Client.SendAsync<GetContestResponse>(request);

        return response.AsResponse.Data!.Contest;
    }

    public static async Task<Contest[]> GetAllContestsAsync(this AdminKernel kernel)
    {
        RestRequest request = kernel.Requests.Contests.GetContests();
        ProblemOrResponse<GetContestsResponse> response = await kernel.Client.SendAsync<GetContestsResponse>(request);

        return response.AsResponse.Data!.Contests;
    }
}

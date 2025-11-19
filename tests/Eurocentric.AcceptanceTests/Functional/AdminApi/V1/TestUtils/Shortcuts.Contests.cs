using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Contests;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static partial class Shortcuts
{
    extension(AdminKernel kernel)
    {
        public async Task<Contest> CreateALiverpoolRulesContestAsync(
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

        public async Task<Contest> CreateAStockholmRulesContestAsync(
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

        public async Task DeleteAContestAsync(Guid contestId)
        {
            RestRequest request = kernel.Requests.Contests.DeleteContest(contestId);
            _ = await kernel.Client.SendAsync(request);
        }

        public async Task<Contest> GetAContestAsync(Guid contestId)
        {
            RestRequest request = kernel.Requests.Contests.GetContest(contestId);
            ProblemOrResponse<GetContestResponse> response = await kernel.Client.SendAsync<GetContestResponse>(request);

            return response.AsResponse.Data!.Contest;
        }

        public async Task<Contest[]> GetAllContestsAsync()
        {
            RestRequest request = kernel.Requests.Contests.GetContests();
            ProblemOrResponse<GetContestsResponse> response = await kernel.Client.SendAsync<GetContestsResponse>(
                request
            );

            return response.AsResponse.Data!.Contests;
        }
    }
}

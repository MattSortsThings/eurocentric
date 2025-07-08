using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;
using Eurocentric.Features.AdminApi.V0.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Mixins.Contests;

internal static class AdminActorMixins
{
    internal static async Task Given_I_have_created_a_Stockholm_format_contest(this IAdminActor admin,
        string cityName = "",
        int contestYear = 0)
    {
        CreateContestRequest requestBody = new()
        {
            ContestYear = contestYear, CityName = cityName, ContestFormat = ContestFormat.Stockholm
        };

        RestRequest request = admin.RequestFactory.Contests.CreateContest(requestBody);
        ProblemOrResponse<CreateContestResponse> problemOrResponse =
            await admin.RestClient.SendAsync<CreateContestResponse>(request, TestContext.Current.CancellationToken);

        Contest contest = problemOrResponse.AsResponse.Data!.Contest;

        admin.GivenContests.Add(contest);
    }

    internal static async Task Given_I_have_created_a_Liverpool_format_contest(this IAdminActor admin,
        string cityName = "",
        int contestYear = 0)
    {
        CreateContestRequest requestBody = new()
        {
            ContestYear = contestYear, CityName = cityName, ContestFormat = ContestFormat.Liverpool
        };

        RestRequest request = admin.RequestFactory.Contests.CreateContest(requestBody);
        ProblemOrResponse<CreateContestResponse> problemOrResponse =
            await admin.RestClient.SendAsync<CreateContestResponse>(request, TestContext.Current.CancellationToken);

        Contest contest = problemOrResponse.AsResponse.Data!.Contest;

        admin.GivenContests.Add(contest);
    }
}

using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;
using Eurocentric.Features.AdminApi.V0.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.Contests.Utils;

internal static class ActorMixins
{
    internal static async Task Given_I_have_created_a_Stockholm_format_contest(
        this IActor actor,
        string cityName = "",
        int contestYear = 0)
    {
        CreateContestRequest requestBody = new()
        {
            ContestYear = contestYear, CityName = cityName, ContestFormat = ContestFormat.Stockholm
        };

        Contest createdContest = await actor.CreateContestAsync(requestBody, TestContext.Current.CancellationToken);

        actor.GivenContests.Add(createdContest);
    }

    private static async Task<Contest> CreateContestAsync(
        this IActor actor,
        CreateContestRequest requestBody,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = new("/admin/api/{apiVersion}/contests", Method.Post);

        request.AddUrlSegment("apiVersion", actor.ApiVersion)
            .AddJsonBody(requestBody);

        ProblemOrResponse<CreateContestResponse> problemOrResponse =
            await actor.RestClient.SendRequestAsync<CreateContestResponse>(request, cancellationToken);

        return problemOrResponse.AsResponse.Data!.Contest;
    }

    internal static async Task<Contest> GetContestAsync(
        this IActor actor,
        Guid contestId,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = new("/admin/api/{apiVersion}/contests/{contestId}");

        request.AddUrlSegment("apiVersion", actor.ApiVersion)
            .AddUrlSegment("contestId", contestId);

        ProblemOrResponse<GetContestResponse> problemOrResponse =
            await actor.RestClient.SendRequestAsync<GetContestResponse>(request, cancellationToken);

        return problemOrResponse.AsResponse.Data!.Contest;
    }
}

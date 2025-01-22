using System.Net;
using Eurocentric.AdminApi.Tests.Acceptance.Utils;
using Eurocentric.AdminApi.V1.Contests.CreateContest;
using Eurocentric.AdminApi.V1.Contests.Models;
using Eurocentric.Tests.Utils.Fixtures;
using RestSharp;

namespace Eurocentric.AdminApi.Tests.Acceptance.V1.Contests;

public static class CreateContestTests
{
    private static void ShouldMatch(this Contest contest, CreateContestRequest request)
    {
        Assert.Equal(request.ContestYear, contest.ContestYear);
        Assert.Equal(request.HostCityName, contest.HostCityName);
        Assert.Equal(request.VotingRules, contest.VotingRules);
    }

    public sealed class AdminApi : CleanWebAppTests
    {
        public AdminApi(CleanWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_create_contest_and_return_200_given_valid_request()
        {
            // Arrange
            CreateContestRequest request = new()
            {
                ContestYear = 2025, HostCityName = "Basel", VotingRules = VotingRules.Liverpool
            };

            RestRequest restRequest = Requests.Post.To("admin/api/v1.0/contests").AddJsonBody(request);

            // Act
            RestResponse<CreateContestResponse> result =
                await Sut.ExecuteAsync<CreateContestResponse>(restRequest, TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => result.ShouldHaveStatusCode(HttpStatusCode.OK),
                () => result.Data!.Contest.ShouldMatch(request)
            );
        }
    }
}

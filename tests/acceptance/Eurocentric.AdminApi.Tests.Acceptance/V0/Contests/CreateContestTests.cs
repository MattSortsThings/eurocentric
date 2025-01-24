using System.Net;
using Eurocentric.AdminApi.Tests.Acceptance.Utils;
using Eurocentric.AdminApi.V0.Contests.CreateContest;
using Eurocentric.AdminApi.V0.Contests.Models;
using Eurocentric.PublicApi.Tests.Acceptance.Utils;
using Eurocentric.Tests.Utils.Fixtures;
using RestSharp;

namespace Eurocentric.AdminApi.Tests.Acceptance.V0.Contests;

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
            CreateContestRequest requestBody = new()
            {
                ContestYear = 2025, HostCityName = "Basel", VotingRules = VotingRules.Liverpool
            };

            RestRequest request = PostRequest.To(Apis.Admin.V0.Latest.Uri + "contests")
                .AddHeader("Accept", "application/json")
                .AddHeader("Content-Type", "application/json")
                .AddHeader("X-Api-Key", TestApiKeys.Admin)
                .AddJsonBody(requestBody);

            // Act
            (HttpStatusCode statusCode, CreateContestResponse response) =
                await Sut.ExecuteAsync<CreateContestResponse>(request, TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.OK),
                () => response.Contest.ShouldMatch(requestBody)
            );
        }
    }
}

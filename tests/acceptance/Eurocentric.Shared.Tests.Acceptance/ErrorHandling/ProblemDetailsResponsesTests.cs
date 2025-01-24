using System.Net;
using Eurocentric.AdminApi.V0.Contests.CreateContest;
using Eurocentric.AdminApi.V0.Contests.Models;
using Eurocentric.Shared.Tests.Acceptance.Utils;
using Eurocentric.Tests.Utils.Fixtures;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Shared.Tests.Acceptance.ErrorHandling;

public static class ProblemDetailsResponsesTests
{
    public sealed class AdminApi : SeededWebAppTests
    {
        public AdminApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        private static CreateContestRequest CreateContestRequestWithVotingRules(VotingRules votingRules) =>
            new() { ContestYear = 2025, HostCityName = "HostCityName", VotingRules = votingRules };

        [Fact]
        public async Task Should_return_422_with_problem_details_given_valid_but_unprocessable_request()
        {
            // Arrange
            RestRequest restRequest = PostRequest.To(Apis.Admin.V0.Latest.Uri + "contests")
                .AddHeader("X-Api-Key", TestApiKeys.Admin)
                .AddHeader("Accept", "application/json")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(CreateContestRequestWithVotingRules(VotingRules.Undefined));

            // Act
            RestResponse<ProblemDetails> result =
                await Sut.ExecuteAsync<ProblemDetails>(restRequest, TestContext.Current.CancellationToken);

            // Assert
            ProblemDetails problemDetails = result.Data!;

            Assert.Multiple(
                () => result.ShouldHaveStatusCode(HttpStatusCode.UnprocessableEntity),
                () => problemDetails.ShouldHaveStatus(422),
                () => problemDetails.ShouldHaveTitle("IllegalVotingRules"),
                () => problemDetails.ShouldHaveDetail("Cannot create a contest with Undefined voting rules."),
                () => problemDetails.ShouldHaveInstance("POST /admin/api/v0.1/contests")
            );
        }
    }

    public sealed class PublicApi : SeededWebAppTests
    {
        public PublicApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_return_400_with_problem_details_given_invalid_request()
        {
            // Arrange
            const int invalidPageSize = 0;

            RestRequest restRequest = GetRequest.To(Apis.Public.V0.Latest.Uri + "voting-country-rankings/points-share")
                .AddHeader("X-Api-Key", TestApiKeys.Public)
                .AddHeader("Accept", "application/json")
                .AddQueryParameter("pageSize", invalidPageSize)
                .AddQueryParameter("targetCountryCode", "GB");

            // Act
            RestResponse<ProblemDetails> result =
                await Sut.ExecuteAsync<ProblemDetails>(restRequest, TestContext.Current.CancellationToken);

            // Assert
            ProblemDetails problemDetails = result.Data!;

            Assert.Multiple(
                () => result.ShouldHaveStatusCode(HttpStatusCode.BadRequest),
                () => problemDetails.ShouldHaveStatus(400),
                () => problemDetails.ShouldHaveTitle("InvalidPageSize"),
                () => problemDetails.ShouldHaveDetail("Page size value cannot be less than 1."),
                () => problemDetails.ShouldHaveInstance("GET /public/api/v0.1/voting-country-rankings/points-share" +
                                                        "?pageSize=0&targetCountryCode=GB"),
                () => problemDetails.ShouldHaveExtensionsEntry("pageSize", 0)
            );
        }
    }
}

using System.Net;
using Eurocentric.Shared.Tests.Acceptance.Utils;
using Eurocentric.Tests.Utils.Fixtures;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Shared.Tests.Acceptance.ErrorHandling;

public static class GlobalExceptionHandlingTests
{
    public sealed class AdminApi : SeededWebAppTests
    {
        public AdminApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_return_400_with_problem_details_given_POST_request_with_missing_required_body_property()
        {
            // Arrange
            const string resourceUri = Apis.Admin.V0.Latest.Uri + "contests";

            const string unparseableJson = """
                                           {"hostCityName": "Basel", "votingRules": "Stockholm"}
                                           """;

            RestRequest request = RestRequestFactory.Post(resourceUri)
                .UseAdminApiKey()
                .AddJsonBody(unparseableJson);

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) =
                await Sut.ExecuteAsync<ProblemDetails>(request, TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => problemDetails.ShouldHaveTitle("BadHttpRequest"),
                () => problemDetails.ShouldHaveInstance("POST /admin/api/v0.1/contests"),
                () => problemDetails.ShouldHaveDetail("BadHttpRequestException was thrown while handling the request."),
                () => problemDetails.ShouldHaveExtensionsEntry("error",
                    "Failed to read parameter 'CreateContestRequest request' from the request body as JSON."),
                () => problemDetails.ShouldHaveExtensionsEntry("jsonError",
                    "JSON deserialization for type 'Eurocentric.AdminApi.V0.Contests.CreateContest.CreateContestRequest' " +
                    "was missing required properties including: 'contestYear'."),
                () => problemDetails.ShouldHaveExtensionsEntry("jsonPath", "$")
            );
        }

        [Fact]
        public async Task Should_return_400_with_problem_details_given_POST_request_with_unparseable_required_body_property()
        {
            // Arrange
            const string resourceUri = Apis.Admin.V0.Latest.Uri + "contests";

            const string unparseableJson = """
                                           {
                                            "contestYear": 2025,
                                            "hostCityName": "Basel",
                                            "votingRules": "NOT_VALID_VOTING_RULES"
                                           }
                                           """;

            RestRequest request = RestRequestFactory.Post(resourceUri)
                .UseAdminApiKey()
                .AddJsonBody(unparseableJson);

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) =
                await Sut.ExecuteAsync<ProblemDetails>(request, TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => problemDetails.ShouldHaveTitle("BadHttpRequest"),
                () => problemDetails.ShouldHaveInstance("POST /admin/api/v0.1/contests"),
                () => problemDetails.ShouldHaveDetail("BadHttpRequestException was thrown while handling the request."),
                () => problemDetails.ShouldHaveExtensionsEntry("error",
                    "Failed to read parameter 'CreateContestRequest request' from the request body as JSON."),
                () => problemDetails.ShouldHaveExtensionsEntry("jsonError",
                    "The JSON value could not be converted to Eurocentric.AdminApi.V0.Contests.Models.VotingRules. " +
                    "Path: $.votingRules | LineNumber: 3 | BytePositionInLine: 40."),
                () => problemDetails.ShouldHaveExtensionsEntry("jsonPath", "$.votingRules")
            );
        }
    }

    public sealed class PublicApi : SeededWebAppTests
    {
        public PublicApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_return_400_with_problem_details_given_GET_request_with_missing_required_query_param()
        {
            // Arrange
            const string resourceUri = Apis.Public.V0.Latest.Uri + "voting-country-rankings/points-share";

            RestRequest request = RestRequestFactory.Get(resourceUri)
                .UsePublicApiKey();

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) =
                await Sut.ExecuteAsync<ProblemDetails>(request, TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => problemDetails.ShouldHaveTitle("BadHttpRequest"),
                () => problemDetails.ShouldHaveInstance("GET /public/api/v0.1/voting-country-rankings/points-share"),
                () => problemDetails.ShouldHaveDetail("BadHttpRequestException was thrown while handling the request."),
                () => problemDetails.ShouldHaveExtensionsEntry("error",
                    "Required parameter 'string TargetCountryCode' was not provided from query string.")
            );
        }

        [Fact]
        public async Task Should_return_400_with_problem_details_given_GET_request_with_unparseable_enum_query_param()
        {
            // Arrange
            const string resourceUri = Apis.Public.V0.Latest.Uri + "voting-country-rankings/points-share";

            RestRequest request = RestRequestFactory.Get(resourceUri)
                .UsePublicApiKey()
                .AddQueryParameter("targetCountryCode", "GB")
                .AddQueryParameter("votingMethod", "NOT_A_VOTING_METHOD");

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) =
                await Sut.ExecuteAsync<ProblemDetails>(request, TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => problemDetails.ShouldHaveTitle("BadHttpRequest"),
                () => problemDetails.ShouldHaveInstance("GET /public/api/v0.1/voting-country-rankings/points-share" +
                                                        "?targetCountryCode=GB&votingMethod=NOT_A_VOTING_METHOD"),
                () => problemDetails.ShouldHaveDetail("BadHttpRequestException was thrown while handling the request."),
                () => problemDetails.ShouldHaveExtensionsEntry("error",
                    "Failed to bind parameter 'Nullable<VotingMethod> VotingMethod' from 'NOT_A_VOTING_METHOD'.")
            );
        }

        [Fact]
        public async Task Should_return_500_with_problem_details_given_valid_request_that_causes_exception()
        {
            // Arrange
            const string resourceUri = Apis.Public.V0.Latest.Uri + "voting-country-rankings/points-share";

            RestRequest request = RestRequestFactory.Get(resourceUri)
                .UsePublicApiKey()
                .AddQueryParameter("targetCountryCode", "  ")
                .AddQueryParameter("votingMethod", "Any");

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) =
                await Sut.ExecuteAsync<ProblemDetails>(request, TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.InternalServerError),
                () => problemDetails.ShouldHaveTitle("InternalServerError"),
                () => problemDetails.ShouldHaveInstance("GET /public/api/v0.1/voting-country-rankings/points-share" +
                                                        "?targetCountryCode=%20%20&votingMethod=Any"),
                () => problemDetails.ShouldHaveDetail("ArgumentException was thrown while handling the request."),
                () => problemDetails.ShouldHaveSingleExtension("traceId")
            );
        }
    }
}

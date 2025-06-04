using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Microsoft.AspNetCore.Http;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public sealed class GlobalExceptionHandlingTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Fact]
    public async Task Should_receive_unsuccessful_response_with_problem_details_given_missing_required_request_body_property()
    {
        // Arrange
        const string createContestRoute = "/admin/api/v0.2/contests";

        RestRequest createContestRequest = new(createContestRoute, Method.Post);

        createContestRequest.UseSecretApiKey()
            .AddJsonBody(new { ContestFormat = "Stockholm", CityName = "Turin" });

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(createContestRequest, TestContext.Current.CancellationToken);

        // Assert
        var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
        Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
        Assert.Equal("POST /admin/api/v0.2/contests", problemDetails.Instance);
        Assert.Contains(problemDetails.Extensions, kvp => kvp.Key == "exceptionMessage");
    }

    [Fact]
    public async Task Should_receive_unsuccessful_response_with_problem_details_given_invalid_request_body_enum_name()
    {
        // Arrange
        const string createContestRoute = "/admin/api/v0.2/contests";

        RestRequest createContestRequest = new(createContestRoute, Method.Post);

        createContestRequest.UseSecretApiKey()
            .AddJsonBody(new { ContestFormat = "INVALID", CityName = "Turin", ContestYear = 2022 });

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(createContestRequest, TestContext.Current.CancellationToken);

        // Assert
        var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
        Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
        Assert.Equal("POST /admin/api/v0.2/contests", problemDetails.Instance);
        Assert.Contains(problemDetails.Extensions, kvp => kvp.Key == "exceptionMessage");
    }

    [Fact]
    public async Task Should_receive_unsuccessful_response_with_problem_details_given_invalid_request_body_enum_int_value()
    {
        // Arrange
        const string createContestRoute = "/admin/api/v0.2/contests";

        RestRequest createContestRequest = new(createContestRoute, Method.Post);

        createContestRequest.UseSecretApiKey()
            .AddJsonBody(new { ContestFormat = 999999, CityName = "Turin", ContestYear = 2022 });

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(createContestRequest, TestContext.Current.CancellationToken);

        // Assert
        var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("InvalidEnumArgumentException was thrown while handling the request.", problemDetails.Detail);
        Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
        Assert.Equal("POST /admin/api/v0.2/contests", problemDetails.Instance);
        Assert.Contains(problemDetails.Extensions, kvp => kvp.Key == "exceptionMessage");
    }

    [Fact]
    public async Task Should_receive_unsuccessful_response_with_problem_details_given_missing_required_query_parameter()
    {
        // Arrange
        const string getPointsShareVotingCountryRankingsRoute = "/public/api/v0.2/voting-country-rankings/points-share";

        RestRequest getRankingsRequest = new(getPointsShareVotingCountryRankingsRoute);

        getRankingsRequest.UseSecretApiKey();

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(getRankingsRequest, TestContext.Current.CancellationToken);

        // Assert
        var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
        Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
        Assert.Equal("GET /public/api/v0.2/voting-country-rankings/points-share", problemDetails.Instance);
        Assert.Contains(problemDetails.Extensions, kvp => kvp.Key == "exceptionMessage");
    }

    [Fact]
    public async Task Should_receive_unsuccessful_response_with_problem_details_given_invalid_query_parameter_enum_name()
    {
        // Arrange
        const string getPointsShareVotingCountryRankingsRoute = "/public/api/v0.2/voting-country-rankings/points-share";

        RestRequest getRankingsRequest = new(getPointsShareVotingCountryRankingsRoute);

        getRankingsRequest.UseSecretApiKey()
            .AddQueryParameter("competingCountryCode", "GB")
            .AddQueryParameter("votingMethod", "INVALID");

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(getRankingsRequest, TestContext.Current.CancellationToken);

        // Assert
        var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
        Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
        Assert.Equal("GET /public/api/v0.2/voting-country-rankings/points-share?competingCountryCode=GB&votingMethod=INVALID",
            problemDetails.Instance);
        Assert.Contains(problemDetails.Extensions, kvp => kvp.Key == "exceptionMessage");
    }

    [Fact]
    public async Task Should_receive_unsuccessful_response_with_problem_details_given_invalid_query_parameter_enum_int_value()
    {
        // Arrange
        const string getPointsShareVotingCountryRankingsRoute = "/public/api/v0.2/voting-country-rankings/points-share";

        RestRequest getRankingsRequest = new(getPointsShareVotingCountryRankingsRoute);

        getRankingsRequest.UseSecretApiKey()
            .AddQueryParameter("competingCountryCode", "GB")
            .AddQueryParameter("votingMethod", 999999);

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(getRankingsRequest, TestContext.Current.CancellationToken);

        // Assert
        var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("InvalidEnumArgumentException was thrown while handling the request.", problemDetails.Detail);
        Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
        Assert.Equal("GET /public/api/v0.2/voting-country-rankings/points-share?competingCountryCode=GB&votingMethod=999999",
            problemDetails.Instance);
        Assert.Contains(problemDetails.Extensions, kvp => kvp.Key == "exceptionMessage");
    }
}

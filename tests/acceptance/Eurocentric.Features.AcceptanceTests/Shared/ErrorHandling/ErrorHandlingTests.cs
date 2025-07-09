using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public static class ErrorHandlingTests
{
    private static (HttpStatusCode statusCode, ProblemDetails? problemDetails, string? retryAfter) ParseAsProblemWithRetryAfter(
        ProblemOrResponse problemOrResponse)
    {
        RestResponse<ProblemDetails> problem = problemOrResponse.AsProblem;

        return (problem.StatusCode, problem.Data, problem.GetHeaderValue("Retry-After"));
    }

    public sealed class Endpoints(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_400_with_ProblemDetails_on_missing_required_request_body_property()
        {
            // Arrange
            RestRequest request = Post("/admin/api/v0.2/contests")
                .AddJsonBody("""
                             {
                                "cityName": "CityName",
                                "contestFormat": "Stockholm"
                             }
                             """);

            const string expectedExceptionMessage =
                "Failed to read parameter \"CreateContestRequest requestBody\" from the request body as JSON.";

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

            Assert.NotNull(problemDetails);

            Assert.Equal(400, problemDetails.Status);
            Assert.Equal("Bad HTTP request", problemDetails.Title);
            Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
            Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
            Assert.Equal("POST /admin/api/v0.2/contests", problemDetails.Instance);
            Assert.Contains(problemDetails.Extensions, kvp => kvp is { Key: "exceptionMessage", Value: JsonElement je }
                                                              && je.GetString() == expectedExceptionMessage);
        }

        [Fact]
        public async Task Should_return_400_with_ProblemDetails_on_invalid_enum_request_body_property_string_value()
        {
            // Arrange
            RestRequest request = Post("/admin/api/v0.2/contests")
                .AddJsonBody("""
                             {
                                "contestYear": 2025,
                                "cityName": "CityName",
                                "contestFormat": "INVALID"
                             }
                             """);

            const string expectedExceptionMessage =
                "Failed to read parameter \"CreateContestRequest requestBody\" from the request body as JSON.";

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

            Assert.NotNull(problemDetails);

            Assert.Equal(400, problemDetails.Status);
            Assert.Equal("Bad HTTP request", problemDetails.Title);
            Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
            Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
            Assert.Equal("POST /admin/api/v0.2/contests", problemDetails.Instance);
            Assert.Contains(problemDetails.Extensions, kvp => kvp is { Key: "exceptionMessage", Value: JsonElement je }
                                                              && je.GetString() == expectedExceptionMessage);
        }

        [Fact]
        public async Task Should_return_400_with_ProblemDetails_on_invalid_enum_request_body_property_int_value()
        {
            // Arrange
            RestRequest request = Post("/admin/api/v0.2/contests")
                .AddJsonBody("""
                             {
                                "contestYear": 2025,
                                "cityName": "CityName",
                                "contestFormat": 999999
                             }
                             """);

            const string expectedExceptionMessage =
                "The value of argument 'contestFormat' (999999) is invalid for Enum type 'ContestFormat'. " +
                "(Parameter 'contestFormat')";

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

            Assert.NotNull(problemDetails);

            Assert.Equal(400, problemDetails.Status);
            Assert.Equal("Invalid enum argument", problemDetails.Title);
            Assert.Equal("InvalidEnumArgumentException was thrown while handling the request.", problemDetails.Detail);
            Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
            Assert.Equal("POST /admin/api/v0.2/contests", problemDetails.Instance);
            Assert.Contains(problemDetails.Extensions, kvp => kvp is { Key: "exceptionMessage", Value: JsonElement je }
                                                              && je.GetString() == expectedExceptionMessage);
        }

        [Fact]
        public async Task Should_return_400_with_ProblemDetails_on_missing_required_query_parameter()
        {
            // Arrange
            RestRequest request = Get("/public/api/v0.2/rankings/competing-countries/points-in-range")
                .AddQueryParameter("maxPoints", 12);

            const string expectedExceptionMessage = "Required parameter \"int MinPoints\" was not provided from query string.";

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

            Assert.NotNull(problemDetails);

            Assert.Equal(400, problemDetails.Status);
            Assert.Equal("Bad HTTP request", problemDetails.Title);
            Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
            Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
            Assert.Equal("GET /public/api/v0.2/rankings/competing-countries/points-in-range?maxPoints=12",
                problemDetails.Instance);
            Assert.Contains(problemDetails.Extensions, kvp => kvp is { Key: "exceptionMessage", Value: JsonElement je }
                                                              && je.GetString() == expectedExceptionMessage);
        }

        [Fact]
        public async Task Should_return_400_with_ProblemDetails_on_invalid_enum_query_parameter_string_value()
        {
            // Arrange
            RestRequest request = Get("/public/api/v0.2/rankings/competing-countries/points-average")
                .AddQueryParameter("votingMethod", "INVALID");

            const string expectedExceptionMessage =
                "Failed to bind parameter \"Nullable<VotingMethodFilter> VotingMethod\" from \"INVALID\".";

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

            Assert.NotNull(problemDetails);

            Assert.Equal(400, problemDetails.Status);
            Assert.Equal("Bad HTTP request", problemDetails.Title);
            Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
            Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
            Assert.Equal("GET /public/api/v0.2/rankings/competing-countries/points-average?votingMethod=INVALID",
                problemDetails.Instance);
            Assert.Contains(problemDetails.Extensions, kvp => kvp is { Key: "exceptionMessage", Value: JsonElement je }
                                                              && je.GetString() == expectedExceptionMessage);
        }

        [Fact]
        public async Task Should_return_400_with_ProblemDetails_on_invalid_enum_query_parameter_int_value()
        {
            // Arrange
            RestRequest request = Get("/public/api/v0.2/rankings/competing-countries/points-average")
                .AddQueryParameter("votingMethod", 999999);

            const string expectedExceptionMessage =
                "The value of argument 'votingMethod' (999999) is invalid for Enum type 'VotingMethodFilter'. " +
                "(Parameter 'votingMethod')";

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

            Assert.NotNull(problemDetails);

            Assert.Equal(400, problemDetails.Status);
            Assert.Equal("Invalid enum argument", problemDetails.Title);
            Assert.Equal("InvalidEnumArgumentException was thrown while handling the request.", problemDetails.Detail);
            Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
            Assert.Equal("GET /public/api/v0.2/rankings/competing-countries/points-average?votingMethod=999999",
                problemDetails.Instance);
            Assert.Contains(problemDetails.Extensions, kvp => kvp is { Key: "exceptionMessage", Value: JsonElement je }
                                                              && je.GetString() == expectedExceptionMessage);
        }

        [Fact]
        public async Task Should_return_404_with_ProblemDetails_on_non_existent_resource_requested()
        {
            // Arrange
            RestRequest request = Get("/admin/api/v0.1/contests/a4cbdf80-cdfb-4906-b758-b22969eb6c7d");

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

            // When
            Assert.Equal(HttpStatusCode.NotFound, statusCode);

            Assert.NotNull(problemDetails);

            Assert.Equal(404, problemDetails.Status);
            Assert.Equal("Contest not found", problemDetails.Title);
            Assert.Equal("No contest exists with the provided contest ID.", problemDetails.Detail);
            Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.5", problemDetails.Type);
            Assert.Equal("GET /admin/api/v0.1/contests/a4cbdf80-cdfb-4906-b758-b22969eb6c7d", problemDetails.Instance);
            Assert.Contains(problemDetails.Extensions, kvp => kvp is { Key: "contestId", Value: JsonElement je }
                                                              && je.GetString() == "a4cbdf80-cdfb-4906-b758-b22969eb6c7d");
        }

        [Fact]
        public async Task Should_return_409_with_ProblemDetails_on_extrinsically_illegal_request_given_system_state()
        {
            // Arrange
            const int duplicateYear = 2025;
            await CreateContestWithYearAsync(duplicateYear);

            RestRequest request = Post("/admin/api/v0.2/contests")
                .AddJsonBody(new { ContestYear = duplicateYear, CityName = "CityName", ContestFormat = "Liverpool" });

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

            // When
            Assert.Equal(HttpStatusCode.Conflict, statusCode);

            Assert.NotNull(problemDetails);

            Assert.Equal(409, problemDetails.Status);
            Assert.Equal("Contest year conflict", problemDetails.Title);
            Assert.Equal("A contest already exists with the provided contest year.", problemDetails.Detail);
            Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.10", problemDetails.Type);
            Assert.Equal("POST /admin/api/v0.2/contests", problemDetails.Instance);
            Assert.Contains(problemDetails.Extensions, kvp => kvp is { Key: "contestYear", Value: JsonElement je }
                                                              && je.GetInt32() == duplicateYear);
        }

        [Fact]
        public async Task Should_return_422_with_ProblemDetails_on_intrinsically_illegal_request()
        {
            // Arrange
            const int illegalYear = 0;

            RestRequest request = Post("/admin/api/v0.2/contests")
                .AddJsonBody(new { ContestYear = illegalYear, CityName = "CityName", ContestFormat = "Liverpool" });

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            var (statusCode, problemDetails) = (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

            // When
            Assert.Equal(HttpStatusCode.UnprocessableEntity, statusCode);

            Assert.NotNull(problemDetails);

            Assert.Equal(422, problemDetails.Status);
            Assert.Equal("Illegal contest year value", problemDetails.Title);
            Assert.Equal("Contest year value must be an integer between 2016 and 2050.", problemDetails.Detail);
            Assert.Equal("https://tools.ietf.org/html/rfc4918#section-11.2", problemDetails.Type);
            Assert.Equal("POST /admin/api/v0.2/contests", problemDetails.Instance);
            Assert.Contains(problemDetails.Extensions, kvp => kvp is { Key: "contestYear", Value: JsonElement je }
                                                              && je.GetInt32() == illegalYear);
        }

        [Fact]
        [Trait("Category", "expensive")]
        [Trait("Category", "clear-box")]
        public async Task Should_return_503_with_retry_after_value_and_ProblemDetails_on_Dapper_database_timeout()
        {
            // Arrange
            await BackDoor.ExecuteScopedAsync(BackDoorOperations.EnsureDatabasePausedAsync);

            RestRequest request = Get("/public/api/v0.2/rankings/competing-countries/points-average");

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            (HttpStatusCode statusCode, ProblemDetails? problemDetails, string? retryAfter) =
                ParseAsProblemWithRetryAfter(problemOrResponse);

            // Assert
            Assert.Equal(HttpStatusCode.ServiceUnavailable, statusCode);

            Assert.Equal("120", retryAfter);

            Assert.NotNull(problemDetails);

            Assert.Equal(503, problemDetails.Status);
            Assert.Equal("Database timeout", problemDetails.Title);
            Assert.Equal("SqlException was thrown while handling the request because the database connection " +
                         "or operation timed out. Please retry after c.120 seconds.", problemDetails.Detail);
            Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.6.4", problemDetails.Type);
            Assert.Equal("GET /public/api/v0.2/rankings/competing-countries/points-average", problemDetails.Instance);
            Assert.DoesNotContain(problemDetails.Extensions, kvp => kvp.Key == "exceptionMessage");
        }

        [Fact]
        [Trait("Category", "expensive")]
        [Trait("Category", "clear-box")]
        public async Task Should_return_503_with_retry_after_value_and_ProblemDetails_on_EfCore_database_timeout()
        {
            // Arrange
            await BackDoor.ExecuteScopedAsync(BackDoorOperations.EnsureDatabasePausedAsync);

            RestRequest request = Get("/public/api/v0.2/filters/countries");

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            (HttpStatusCode statusCode, ProblemDetails? problemDetails, string? retryAfter) =
                ParseAsProblemWithRetryAfter(problemOrResponse);

            // Assert
            Assert.Equal(HttpStatusCode.ServiceUnavailable, statusCode);

            Assert.Equal("120", retryAfter);

            Assert.NotNull(problemDetails);

            Assert.Equal(503, problemDetails.Status);
            Assert.Equal("Database timeout", problemDetails.Title);
            Assert.Equal("SqlException was thrown while handling the request because the database connection " +
                         "or operation timed out. Please retry after c.120 seconds.", problemDetails.Detail);
            Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.6.4", problemDetails.Type);
            Assert.Equal("GET /public/api/v0.2/filters/countries", problemDetails.Instance);
            Assert.DoesNotContain(problemDetails.Extensions, kvp => kvp.Key == "exceptionMessage");
        }

        private async Task CreateContestWithYearAsync(int contestYear)
        {
            RestRequest request = Post("/admin/api/v0.2/contests");

            request.AddJsonBody(new { ContestYear = contestYear, CityName = "CityName", ContestFormat = "Liverpool" });

            _ = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);
        }
    }
}

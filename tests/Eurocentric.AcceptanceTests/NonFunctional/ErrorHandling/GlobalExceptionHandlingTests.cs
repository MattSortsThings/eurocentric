using System.Net;
using Eurocentric.AcceptanceTests.TestUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.AcceptanceTests.NonFunctional.ErrorHandling;

[Category("error-handling")]
public sealed class GlobalExceptionHandlingTests : ParallelSeededAcceptanceTest
{
    [Test]
    [Category("clear-box")]
    public async Task Should_return_400_with_problem_details_on_missing_required_request_body_property()
    {
        // Arrange
        Guid contestId2023 = Guid.Parse("01979693-f66a-7377-a61e-6146a499b45e");

        const string noContestStageJsonBody = """
            {
               "broadcastDate": "2023-05-03",
               "competingCountryIds": [
                    "01979615-a771-7494-ba80-2cb6cfc8e75fF",
                    "01979614-bd9e-796c-beda-545301fda84d"
               ]
            }
            """;

        RestRequest createContestBroadcastRequest = PostRequest("/admin/api/v1.0/contests/{contestId}/broadcasts")
            .UseSecretApiKey()
            .AddUrlSegment("contestId", contestId2023)
            .AddJsonBody(noContestStageJsonBody);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(createContestBroadcastRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert
            .That(problem.Data)
            .HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasInstance("POST /admin/api/v1.0/contests/01979693-f66a-7377-a61e-6146a499b45e/broadcasts")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension(
                "exceptionMessage",
                """Failed to read parameter "CreateContestBroadcastRequest request" from the request body as JSON."""
            );
    }

    [Test]
    [Category("clear-box")]
    public async Task Should_return_400_with_problem_details_on_request_body_enum_property_with_invalid_string_value()
    {
        // Arrange
        Guid contestId2023 = Guid.Parse("01979693-f66a-7377-a61e-6146a499b45e");

        const string noContestStageJsonBody = """
            {
               "contestStage": "INVALID",
               "broadcastDate": "2023-05-03",
               "competingCountryIds": [
                    "01979615-a771-7494-ba80-2cb6cfc8e75fF",
                    "01979614-bd9e-796c-beda-545301fda84d"
               ]
            }
            """;

        RestRequest createContestBroadcastRequest = PostRequest("/admin/api/v1.0/contests/{contestId}/broadcasts")
            .UseSecretApiKey()
            .AddUrlSegment("contestId", contestId2023)
            .AddJsonBody(noContestStageJsonBody);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(createContestBroadcastRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert
            .That(problem.Data)
            .HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasInstance("POST /admin/api/v1.0/contests/01979693-f66a-7377-a61e-6146a499b45e/broadcasts")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension(
                "exceptionMessage",
                """Failed to read parameter "CreateContestBroadcastRequest request" from the request body as JSON."""
            );
    }

    [Test]
    [Category("clear-box")]
    public async Task Should_return_400_with_problem_details_on_request_body_enum_property_with_invalid_integer_value()
    {
        // Arrange
        Guid contestId2023 = Guid.Parse("01979693-f66a-7377-a61e-6146a499b45e");

        const string noContestStageJsonBody = """
            {
               "contestStage": 999999,
               "broadcastDate": "2023-05-03",
               "competingCountryIds": [
                    "01979615-a771-7494-ba80-2cb6cfc8e75fF",
                    "01979614-bd9e-796c-beda-545301fda84d"
               ]
            }
            """;

        RestRequest createContestBroadcastRequest = PostRequest("/admin/api/v1.0/contests/{contestId}/broadcasts")
            .UseSecretApiKey()
            .AddUrlSegment("contestId", contestId2023)
            .AddJsonBody(noContestStageJsonBody);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(createContestBroadcastRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert
            .That(problem.Data)
            .HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasInstance("POST /admin/api/v1.0/contests/01979693-f66a-7377-a61e-6146a499b45e/broadcasts")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension(
                "exceptionMessage",
                """Failed to read parameter "CreateContestBroadcastRequest request" from the request body as JSON."""
            );
    }

    [Test]
    [Category("clear-box")]
    public async Task Should_return_400_with_problem_details_on_missing_required_query_param()
    {
        // Arrange
        RestRequest getListingsRequest = GetRequest("/public/api/v1.0/listings/broadcast-result")
            .UseSecretApiKey()
            .AddQueryParameter("contestStage", "SemiFinal1");

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getListingsRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert
            .That(problem.Data)
            .HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasInstance("GET /public/api/v1.0/listings/broadcast-result?contestStage=SemiFinal1")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension(
                "exceptionMessage",
                """Required parameter "int ContestYear" was not provided from query string."""
            );
    }

    [Test]
    [Category("clear-box")]
    public async Task Should_return_400_with_problem_details_on_enum_query_param_with_invalid_string_value()
    {
        // Arrange
        RestRequest getListingsRequest = GetRequest("/public/api/v1.0/listings/broadcast-result")
            .UseSecretApiKey()
            .AddQueryParameter("contestYear", 2023)
            .AddQueryParameter("contestStage", "INVALID");

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getListingsRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert
            .That(problem.Data)
            .HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasInstance("GET /public/api/v1.0/listings/broadcast-result?contestYear=2023&contestStage=INVALID")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension(
                "exceptionMessage",
                """Failed to bind parameter "ContestStage ContestStage" from "INVALID"."""
            );
    }

    [Test]
    [Category("clear-box")]
    public async Task Should_return_400_with_problem_details_on_enum_query_param_with_invalid_int_value()
    {
        // Arrange
        RestRequest getListingsRequest = GetRequest("/public/api/v1.0/listings/broadcast-result")
            .UseSecretApiKey()
            .AddQueryParameter("contestYear", 2023)
            .AddQueryParameter("contestStage", 999999);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getListingsRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert
            .That(problem.Data)
            .HasTitle("Invalid enum argument")
            .And.HasDetail("InvalidEnumArgumentException was thrown while handling the request.")
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasInstance("GET /public/api/v1.0/listings/broadcast-result?contestYear=2023&contestStage=999999")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage", "Invalid ContestStage enum value: 999999.");
    }

    private static RestRequest GetRequest(string route) => new(route);

    private static RestRequest PostRequest(string route) => new(route, Method.Post);
}

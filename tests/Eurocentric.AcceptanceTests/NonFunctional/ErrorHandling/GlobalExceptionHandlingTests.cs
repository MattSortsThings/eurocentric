using System.Net;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils.Assertions;
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
        RestRequest createCountryRequest = PostRequest("/admin/api/v0.1/countries")
            .UseSecretApiKey()
            .AddJsonBody("""{ "countryType": "Real", "countryName": "CountryName" }""");

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            createCountryRequest,
            TestContext.Current!.CancellationToken
        );

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert
            .That(problem.Data)
            .HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasInstance("POST /admin/api/v0.1/countries")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension(
                "exceptionMessage",
                """Failed to read parameter "CreateCountryRequest request" from the request body as JSON."""
            );
    }

    [Test]
    [Category("clear-box")]
    public async Task Should_return_400_with_problem_details_on_request_body_enum_property_with_invalid_string_value()
    {
        // Arrange
        RestRequest createCountryRequest = PostRequest("/admin/api/v0.1/countries")
            .UseSecretApiKey()
            .AddJsonBody("""{ "countryType": "INVALID", "countryCode": "AA", "countryName": "CountryName" }""");

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            createCountryRequest,
            TestContext.Current!.CancellationToken
        );

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert
            .That(problem.Data)
            .HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasInstance("POST /admin/api/v0.1/countries")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension(
                "exceptionMessage",
                """Failed to read parameter "CreateCountryRequest request" from the request body as JSON."""
            );
    }

    [Test]
    [Category("clear-box")]
    public async Task Should_return_400_with_problem_details_on_request_body_enum_property_with_invalid_integer_value()
    {
        // Arrange
        RestRequest createCountryRequest = PostRequest("/admin/api/v0.1/countries")
            .UseSecretApiKey()
            .AddJsonBody("""{ "countryType": 999999, "countryCode": "AA", "countryName": "CountryName" }""");

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            createCountryRequest,
            TestContext.Current!.CancellationToken
        );

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert
            .That(problem.Data)
            .HasTitle("Invalid enum argument")
            .And.HasDetail("InvalidEnumArgumentException was thrown while handling the request.")
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasInstance("POST /admin/api/v0.1/countries")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage", "Invalid CountryType enum value: 999999.");
    }

    [Test]
    [Category("clear-box")]
    public async Task Should_return_400_with_problem_details_on_missing_required_query_param()
    {
        // Arrange
        RestRequest getListingsRequest = GetRequest("/public/api/v0.2/listings/broadcast-result")
            .UseSecretApiKey()
            .AddQueryParameter("contestStage", "SemiFinal1");

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            getListingsRequest,
            TestContext.Current!.CancellationToken
        );

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert
            .That(problem.Data)
            .HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasInstance("GET /public/api/v0.2/listings/broadcast-result?contestStage=SemiFinal1")
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
        RestRequest getListingsRequest = GetRequest("/public/api/v0.2/listings/broadcast-result")
            .UseSecretApiKey()
            .AddQueryParameter("contestYear", 2023)
            .AddQueryParameter("contestStage", "INVALID");

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            getListingsRequest,
            TestContext.Current!.CancellationToken
        );

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert
            .That(problem.Data)
            .HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasInstance("GET /public/api/v0.2/listings/broadcast-result?contestYear=2023&contestStage=INVALID")
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
        RestRequest getListingsRequest = GetRequest("/public/api/v0.2/listings/broadcast-result")
            .UseSecretApiKey()
            .AddQueryParameter("contestYear", 2023)
            .AddQueryParameter("contestStage", 999999);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            getListingsRequest,
            TestContext.Current!.CancellationToken
        );

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert
            .That(problem.Data)
            .HasTitle("Invalid enum argument")
            .And.HasDetail("InvalidEnumArgumentException was thrown while handling the request.")
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasInstance("GET /public/api/v0.2/listings/broadcast-result?contestYear=2023&contestStage=999999")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage", "Invalid ContestStage enum value: 999999.");
    }

    private static RestRequest GetRequest(string route) => new(route);

    private static RestRequest PostRequest(string route) => new(route, Method.Post);
}

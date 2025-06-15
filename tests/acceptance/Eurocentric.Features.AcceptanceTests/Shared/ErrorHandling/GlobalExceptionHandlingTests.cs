using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public sealed class GlobalExceptionHandlingTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Fact]
    public async Task Should_receive_unsuccessful_response_with_problem_details_given_missing_required_request_body_property()
    {
        // Arrange
        RestRequest request = Post("/admin/api/v1.0/contests")
            .UseSecretApiKey()
            .AddJsonBody(new { ContestYear = 2016, CityName = "CityName", ContestFormat = "Stockholm" });

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
        Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
        Assert.Equal("POST /admin/api/v1.0/contests", problemDetails.Instance);
        Assert.Contains(problemDetails.Extensions, kvp => kvp.Key == "exceptionMessage");
    }

    [Fact]
    public async Task Should_receive_unsuccessful_response_with_problem_details_given_invalid_request_body_enum_name()
    {
        // Arrange
        RestRequest request = Post("/admin/api/v1.0/contests")
            .UseSecretApiKey()
            .AddJsonBody(new
            {
                ContestYear = 2016,
                CityName = "CityName",
                ContestFormat = "INVALID",
                Group1Participants = Array.Empty<object>(),
                Group2Participants = Array.Empty<object>()
            });

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
        Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
        Assert.Equal("POST /admin/api/v1.0/contests", problemDetails.Instance);
        Assert.Contains(problemDetails.Extensions, kvp => kvp.Key == "exceptionMessage");
    }

    [Fact]
    public async Task Should_receive_unsuccessful_response_with_problem_details_given_invalid_request_body_enum_int_value()
    {
        // Arrange
        RestRequest request = Post("/admin/api/v1.0/contests")
            .UseSecretApiKey()
            .AddJsonBody(new
            {
                ContestYear = 2016,
                CityName = "CityName",
                ContestFormat = 999999,
                Group1Participants = Array.Empty<object>(),
                Group2Participants = Array.Empty<object>()
            });

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("InvalidEnumArgumentException was thrown while handling the request.", problemDetails.Detail);
        Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.Type);
        Assert.Equal("POST /admin/api/v1.0/contests", problemDetails.Instance);
        Assert.Contains(problemDetails.Extensions, kvp => kvp.Key == "exceptionMessage");
    }

    [Fact]
    public async Task Should_receive_unsuccessful_response_with_problem_details_given_missing_required_query_parameter()
    {
        // Arrange
        RestRequest request = Get("/public/api/v0.2/voting-country-rankings/points-share")
            .UseSecretApiKey();

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(request, TestContext.Current.CancellationToken);

        // Assert
        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

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
        RestRequest request = Get("/public/api/v0.2/voting-country-rankings/points-share")
            .UseSecretApiKey()
            .AddQueryParameter("competingCountryCode", "GB")
            .AddQueryParameter("votingMethod", "INVALID");

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(request, TestContext.Current.CancellationToken);

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
        RestRequest request = Get("/public/api/v0.2/voting-country-rankings/points-share")
            .UseSecretApiKey()
            .AddQueryParameter("competingCountryCode", "GB")
            .AddQueryParameter("votingMethod", 999999);

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(request, TestContext.Current.CancellationToken);

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

using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Shared.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Contests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public sealed class GlobalExceptionHandlingTests : AcceptanceTestBase
{
    public GlobalExceptionHandlingTests(WebAppFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_return_status_code_400_with_problem_details_given_query_string_with_invalid_int_enum_param()
    {
        // Arrange
        RestRequest request = new("public/api/v0.2/voting-country-rankings/points-share");

        request.AddQueryParameter("competingCountryCode", "GB")
            .AddQueryParameter("votingMethod", 999999)
            .UseDemoApiKey();

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (responseOrProblem.AsT1.StatusCode, responseOrProblem.AsT1.Data);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);

        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("GET /public/api/v0.2/voting-country-rankings/points-share?competingCountryCode=GB&votingMethod=999999",
            problemDetails.Instance);
        Assert.Equal("InvalidEnumArgumentException was thrown while handling the request.", problemDetails.Detail);
        Assert.Contains(problemDetails.Extensions, kvp =>
            kvp is { Key: "exceptionMessage", Value: JsonElement j }
            && j.GetString() == "The value of argument 'votingMethod' (999999) is invalid for Enum type 'VotingMethod'. " +
            "(Parameter 'votingMethod')");
    }

    [Fact]
    public async Task Should_return_status_code_400_with_problem_details_given_request_body_with_missing_required_property()
    {
        // Arrange
        RestRequest request = new("admin/api/v1.0/countries", Method.Post);

        var requestBody = new { CountryName = "CountryName" };

        request.UseSecretApiKey()
            .AddJsonBody(requestBody);

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (responseOrProblem.AsT1.StatusCode, responseOrProblem.AsT1.Data);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);

        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("POST /admin/api/v1.0/countries", problemDetails.Instance);
        Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
        Assert.Contains(problemDetails.Extensions, kvp =>
            kvp is { Key: "exceptionMessage", Value: JsonElement j }
            && j.GetString() == "Failed to read parameter \"CreateCountryRequest request\" from the request body as JSON.");
    }

    [Fact]
    public async Task Should_return_status_code_400_with_problem_details_given_request_body_with_unparseable_enum_property()
    {
        // Arrange
        RestRequest request = new("admin/api/v1.0/contests", Method.Post);

        var requestBody = new
        {
            ContestFormat = "INVALID",
            CityName = "CityName",
            ContestYear = 2025,
            Group1Participants = CreateArbitraryContestParticipantData(3),
            Group2Participants = CreateArbitraryContestParticipantData(3)
        };

        request.UseSecretApiKey()
            .AddJsonBody(requestBody);

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (responseOrProblem.AsT1.StatusCode, responseOrProblem.AsT1.Data);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);

        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("POST /admin/api/v1.0/contests", problemDetails.Instance);
        Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
        Assert.Contains(problemDetails.Extensions, kvp =>
            kvp is { Key: "exceptionMessage", Value: JsonElement j }
            && j.GetString() == "Failed to read parameter \"CreateContestRequest request\" from the request body as JSON.");
    }

    [Fact]
    public async Task Should_return_status_code_400_with_problem_details_given_request_body_with_invalid_int_enum_property()
    {
        // Arrange
        RestRequest request = new("admin/api/v1.0/contests", Method.Post);

        var requestBody = new
        {
            ContestFormat = 999999,
            CityName = "CityName",
            ContestYear = 2025,
            Group1Participants = CreateArbitraryContestParticipantData(3),
            Group2Participants = CreateArbitraryContestParticipantData(3)
        };

        request.UseSecretApiKey()
            .AddJsonBody(requestBody);

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (responseOrProblem.AsT1.StatusCode, responseOrProblem.AsT1.Data);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.NotNull(problemDetails);

        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("POST /admin/api/v1.0/contests", problemDetails.Instance);
        Assert.Equal("InvalidEnumArgumentException was thrown while handling the request.", problemDetails.Detail);
        Assert.Contains(problemDetails.Extensions, kvp =>
            kvp is { Key: "exceptionMessage", Value: JsonElement j }
            && j.GetString() == "The value of argument 'contestFormat' (999999) is invalid for Enum type 'ContestFormat'. " +
            "(Parameter 'contestFormat')");
    }

    private static ContestParticipantDatum[] CreateArbitraryContestParticipantData(int count) => Enumerable.Range(0, count)
        .Select(_ => new ContestParticipantDatum { CountryId = Guid.NewGuid(), ActName = "A", SongTitle = "S" }).ToArray();
}

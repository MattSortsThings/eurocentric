using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Shared.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public sealed class ProblemDetailsResponsesTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Fact]
    public async Task Should_receive_unsuccessful_response_with_problem_details_when_request_is_unsuccessful()
    {
        // Arrange
        RestRequest request = Get("/admin/api/v0.2/contests/b32cae3d-2aff-4adc-808e-5e6293c3dd8e")
            .UseSecretApiKey();

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) =
            (problemOrResponse.AsProblem.StatusCode, problemOrResponse.AsProblem.Data);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, statusCode);

        Assert.NotNull(problemDetails);

        Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        Assert.Equal("Contest not found", problemDetails.Title);
        Assert.Equal("No contest exists with the provided contest ID.", problemDetails.Detail);
        Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.5", problemDetails.Type);
        Assert.Equal("GET /admin/api/v0.2/contests/b32cae3d-2aff-4adc-808e-5e6293c3dd8e", problemDetails.Instance);
        Assert.Contains(problemDetails.Extensions, kvp => kvp is { Key: "contestId", Value: JsonElement e }
                                                          && e.GetString() == "b32cae3d-2aff-4adc-808e-5e6293c3dd8e");
    }
}

using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public static class ProblemDetailsResponsesTests
{
    public sealed class Api(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_unsuccessful_response_with_ProblemDetails_object_on_failed_request()
        {
            // Arrange
            RestRequest request = new("/admin/api/v0.1/contests/a4cbdf80-cdfb-4906-b758-b22969eb6c7d");

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
            Assert.Contains(problemDetails.Extensions, kvp => kvp is { Key: "contestId", Value: JsonElement je }
                                                              && je.GetString() == "a4cbdf80-cdfb-4906-b758-b22969eb6c7d");
        }
    }
}

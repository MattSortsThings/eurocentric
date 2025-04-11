using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.TestUtils.WebAppFixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public sealed class ProblemDetailsResponsesTests(WebAppFixture webAppFixture) : AcceptanceTestBase(webAppFixture)
{
    [Fact]
    public async Task Api_should_return_unsuccessful_response_with_ProblemDetails_when_request_is_unsuccessful()
    {
        // Arrange
        const string route = "/admin/api/v0.2/stations/{stationId}";
        const int stationId = 0;

        RestRequest restRequest = Get(route).UseAdminApiKey().AddUrlSegment(nameof(stationId), stationId);

        // Act
        var (statusCode, problemDetails, _) =
            await Sut.SendAsync<ProblemDetails>(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, statusCode);

        Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        Assert.Equal("Station not found", problemDetails.Title);
        Assert.Equal("No station exists with the specified ID.", problemDetails.Detail);
        Assert.NotNull(problemDetails.Type);
        Assert.Equal("GET /admin/api/v0.2/stations/0", problemDetails.Instance);
        Assert.True(problemDetails.Extensions.TryGetValue(nameof(stationId), out object? value)
                    && value is JsonElement j
                    && j.GetInt32() == stationId);
    }
}

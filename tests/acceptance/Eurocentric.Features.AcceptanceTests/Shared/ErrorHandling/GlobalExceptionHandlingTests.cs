using System.Net;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.TestUtils.WebAppFixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public sealed class GlobalExceptionHandlingTests(WebAppFixture webAppFixture) : AcceptanceTestBase(webAppFixture)
{
    [Fact]
    public async Task Api_should_return_BadRequest_with_ProblemDetails_when_BadHttpRequestException_thrown()
    {
        // Arrange
        RestRequest restRequest = Post("admin/api/v0.2/stations").UseAdminApiKey().AddJsonBody(new { MalformedRequest = true });

        // Act
        (HttpStatusCode statusCode, ProblemDetails problemDetails, _) =
            await Sut.SendAsync<ProblemDetails>(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("BadHttpRequestException was thrown while handling the request.", problemDetails.Detail);
        Assert.Equal("POST /admin/api/v0.2/stations", problemDetails.Instance);
        Assert.NotNull(problemDetails.Type);
        Assert.True(problemDetails.Extensions.ContainsKey("exceptionMessage"));
    }

    [Fact]
    public async Task Api_should_return_BadRequest_with_ProblemDetails_when_InvalidEnumArgumentException_thrown()
    {
        // Arrange
        RestRequest restRequest = Get("public/api/v0.1/stations").UsePublicApiKey().AddQueryParameter("line", 999999);

        // Act
        (HttpStatusCode statusCode, ProblemDetails problemDetails, _) =
            await Sut.SendAsync<ProblemDetails>(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Bad HTTP request", problemDetails.Title);
        Assert.Equal("InvalidEnumArgumentException was thrown while handling the request.", problemDetails.Detail);
        Assert.Equal("GET /public/api/v0.1/stations?line=999999", problemDetails.Instance);
        Assert.NotNull(problemDetails.Type);
        Assert.True(problemDetails.Extensions.ContainsKey("exceptionMessage"));
    }
}

using System.Net;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Security;

public sealed class ApiKeyAuthenticationTests(WebAppFixture webAppFixture) : AcceptanceTestBase(webAppFixture)
{
    [Fact]
    public async Task AdminApi_should_not_authenticate_request_that_uses_no_API_key()
    {
        // Arrange
        RestRequest restRequest = Get("admin/api/v0.1/stations/1");

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, restResponse.StatusCode);
    }

    [Fact]
    public async Task PublicApi_should_not_authenticate_request_that_uses_no_API_key()
    {
        // Arrange
        RestRequest restRequest = Get("public/api/v0.1/stations/");

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, restResponse.StatusCode);
    }

    [Fact]
    public async Task Docs_endpoint_should_authenticate_and_authorize_request_that_uses_no_API_key()
    {
        // Arrange
        RestRequest restRequest = Get("docs/public-api-v0.1");

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
    }

    [Fact]
    public async Task OpenApi_endpoint_should_authenticate_and_authorize_request_that_uses_no_API_key()
    {
        // Arrange
        RestRequest restRequest = Get("openapi/public-api-v0.1.json");

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
    }
}

using System.Net;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Security;

public sealed class AdminApiAuthorizationTests(WebAppFixture webAppFixture) : AcceptanceTestBase(webAppFixture)
{
    private const string Route = "admin/api/v0.1/stations/1";

    [Fact]
    public async Task AdminApi_should_authenticate_and_authorize_request_using_Secret_API_key()
    {
        // Arrange
        RestRequest restRequest = Get(Route).AddHeader("X-Api-Key", TestApiKeys.Secret);

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
    }

    [Fact]
    public async Task AdminApi_should_authenticate_but_not_authorize_request_using_Demo_API_key()
    {
        // Arrange
        RestRequest restRequest = Get(Route).AddHeader("X-Api-Key", TestApiKeys.Demo);

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, restResponse.StatusCode);
    }

    [Fact]
    public async Task AdminApi_should_not_authenticate_request_using_unrecognized_API_key()
    {
        // Arrange
        RestRequest restRequest = Get(Route).AddHeader("X-Api-Key", TestApiKeys.Unrecognized);

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, restResponse.StatusCode);
    }
}

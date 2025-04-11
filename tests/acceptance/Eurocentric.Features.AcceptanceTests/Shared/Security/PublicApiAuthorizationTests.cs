using System.Net;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Security;

public sealed class PublicApiAuthorizationTests(WebAppFixture webAppFixture) : AcceptanceTestBase(webAppFixture)
{
    private const string Route = "public/api/v0.1/stations?line=Metropolitan";

    [Fact]
    public async Task PublicApi_should_authenticate_and_authorize_request_using_Secret_API_key()
    {
        // Arrange
        RestRequest restRequest = Get(Route).AddHeader("X-Api-Key", TestApiKeys.Secret);

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
    }

    [Fact]
    public async Task PublicApi_should_authenticate_and_authorize_request_using_Demo_API_key()
    {
        // Arrange
        RestRequest restRequest = Get(Route).AddHeader("X-Api-Key", TestApiKeys.Demo);

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
    }

    [Fact]
    public async Task PublicApi_should_not_authenticate_request_using_unrecognized_API_key()
    {
        // Arrange
        RestRequest restRequest = Get(Route).AddHeader("X-Api-Key", TestApiKeys.Unrecognized);

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, restResponse.StatusCode);
    }
}

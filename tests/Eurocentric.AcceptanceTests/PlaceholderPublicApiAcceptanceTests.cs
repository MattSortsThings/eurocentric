using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests;

[Category("placeholder")]
public sealed class PlaceholderPublicApiAcceptanceTests : ParallelSeededAcceptanceTest
{
    [Test]
    [Arguments("v0.1")]
    [Arguments("v0.2")]
    public async Task Public_API_request_should_return_successful_response(string apiVersion)
    {
        // Arrange
        RestRequest request = new RestRequest("/public/api/{apiVersion}/queryables/countries").AddUrlSegment(
            "apiVersion",
            apiVersion
        );

        // Act
        RestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        await Assert.That(response.StatusCode).IsSuccess();
    }
}

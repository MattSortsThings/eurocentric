using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests;

[Category("placeholder")]
public sealed class PlaceholderAdminApiAcceptanceTests : SerialCleanAcceptanceTest
{
    [Test]
    [Arguments("v0.1")]
    [Arguments("v0.2")]
    public async Task Admin_API_request_should_return_successful_response(string apiVersion)
    {
        // Arrange
        RestRequest request = new RestRequest("/admin/api/{apiVersion}/countries").AddUrlSegment(
            "apiVersion",
            apiVersion
        );

        // Act
        RestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        await Assert.That(response.StatusCode).IsSuccess();
    }
}

using Eurocentric.Features.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Documentation;

public sealed class DocsEndpointTests : ParallelSeededAcceptanceTest
{
    [Test]
    [Arguments("admin-api-v0.1")]
    [Arguments("admin-api-v0.2")]
    [Arguments("public-api-v0.1")]
    [Arguments("public-api-v0.2")]
    public async Task Endpoint_should_serve_requested_documentation_page_for_anonymous_client(string docName)
    {
        // Arrange
        RestRequest request = new RestRequest("/docs/{docName}")
            .AddUrlSegment("docName", docName)
            .AddHeader("Accept", "text/html");

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        await Assert.That(response.IsSuccessful).IsTrue();
    }
}

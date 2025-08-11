using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Documentation;

public sealed class DocsEndpointTests : ParallelCleanAcceptanceTest
{
    [Test]
    [Arguments("admin-api-v0.1")]
    [Arguments("admin-api-v0.2")]
    [Arguments("admin-api-v1.0")]
    [Arguments("public-api-v0.1")]
    [Arguments("public-api-v0.2")]
    public async Task Endpoint_should_serve_requested_API_documentation_page_to_anonymous_client(string docName)
    {
        // Arrange
        RestRequest request = new RestRequest("docs/{docName}")
            .AddUrlSegment("docName", docName)
            .AddHeader("Accept", "text/html");

        // Act
        ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

        (HttpStatusCode statusCode, string? html) = (result.AsResponse.StatusCode, result.AsResponse.Content);

        // Assert
        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.OK);

        await Assert.That(html).IsNotNull();
    }
}

using System.Net;
using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests.NonFunctional.OpenApi;

[Category("open-api")]
public sealed class ApiDocumentationPagesTests : ParallelSeededAcceptanceTest
{
    [Test]
    [Arguments("admin-api-v0.1")]
    [Arguments("admin-api-v0.2")]
    [Arguments("admin-api-v1.0")]
    [Arguments("public-api-v0.1")]
    [Arguments("public-api-v0.2")]
    public async Task Should_serve_requested_API_HTML_document_to_anonymous_client(string docName)
    {
        // Arrange
        RestRequest getDocumentRequest = GetRequest("/docs/{docName}")
            .AddUrlSegment("docName", docName)
            .AddHeader("Accept", "text/html");

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getDocumentRequest);

        // Assert
        RestResponse response = await Assert.That(problemOrResponse).IsResponse().And.IsNotNull();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }

    private static RestRequest GetRequest(string route) => new(route);
}

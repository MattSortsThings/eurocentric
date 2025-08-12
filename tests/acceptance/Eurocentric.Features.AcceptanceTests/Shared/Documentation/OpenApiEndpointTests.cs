using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Documentation;

public sealed class OpenApiEndpointTests : ParallelCleanAcceptanceTest
{
    private static async Task<string[]> ExtractAllPaths(string? json)
    {
        string jsonText = await Assert.That(json).IsNotNull();

        using JsonDocument doc = JsonDocument.Parse(jsonText);

        return doc.RootElement.GetProperty("paths")
            .EnumerateObject()
            .Select(property => property.Name)
            .ToArray();
    }

    [Test]
    [Arguments("admin-api-v0.1", "/admin/api/v0.1")]
    [Arguments("admin-api-v0.2", "/admin/api/v0.2")]
    [Arguments("admin-api-v1.0", "/admin/api/v1.0")]
    [Arguments("public-api-v0.1", "/public/api/v0.1")]
    [Arguments("public-api-v0.2", "/public/api/v0.2")]
    [Arguments("public-api-v1.0", "/public/api/v1.0")]
    public async Task Endpoint_should_serve_requested_OpenAPI_document_to_anonymous_client(string docName, string pathPrefix)
    {
        // Arrange
        RestRequest request = new RestRequest("openapi/{docName}.json").AddUrlSegment("docName", docName);

        // Act
        ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

        (HttpStatusCode statusCode, string? json) = (result.AsResponse.StatusCode, result.AsResponse.Content);

        // Assert
        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.OK);

        string[] pathsInDocument = await ExtractAllPaths(json);

        await Assert.That(pathsInDocument).IsNotEmpty()
            .And.ContainsOnly(path => path.StartsWith(pathPrefix));
    }

    [Test]
    [Arguments("admin-api-v0")]
    [Arguments("admin-api-v0.3")]
    [Arguments("admin-api-v1")]
    [Arguments("admin-api-v1.1")]
    [Arguments("public-api-v0")]
    [Arguments("public-api-v0.3")]
    [Arguments("public-api-v1")]
    [Arguments("public-api-v1.1")]
    public async Task Endpoint_should_fail_on_non_existent_OpenAPI_document_requested(string docName)
    {
        // Arrange
        RestRequest request = new RestRequest("openapi/{docName}.json").AddUrlSegment("docName", docName);

        // Act
        ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

        HttpStatusCode statusCode = result.AsProblem.StatusCode;

        // Assert
        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.NotFound);
    }
}

using System.Net;
using System.Text.Json;
using Eurocentric.AcceptanceTests.TestUtils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.AcceptanceTests.NonFunctional.OpenApi;

[Category("open-api")]
public sealed class OpenApiDocumentsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [Arguments("admin-api-v0.1")]
    [Arguments("admin-api-v0.2")]
    [Arguments("admin-api-v1.0")]
    [Arguments("public-api-v0.1")]
    [Arguments("public-api-v0.2")]
    [Arguments("public-api-v1.0")]
    public async Task OpenAPI_endpoint_should_serve_requested_OpenAPI_JSON_document_to_anonymous_client(string docName)
    {
        // Arrange
        RestRequest getDocumentRequest = GetRequest("/openapi/{docName}.json").AddUrlSegment("docName", docName);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getDocumentRequest);

        // Assert
        RestResponse response = await Assert.That(problemOrResponse).IsResponse().And.IsNotNull();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }

    [Test]
    [Arguments("admin-api-v0.1", "/admin/api/v0.1")]
    [Arguments("admin-api-v0.2", "/admin/api/v0.2")]
    [Arguments("admin-api-v1.0", "/admin/api/v1.0")]
    [Arguments("public-api-v0.1", "/public/api/v0.1")]
    [Arguments("public-api-v0.2", "/public/api/v0.2")]
    [Arguments("public-api-v1.0", "/public/api/v1.0")]
    public async Task OpenAPI_document_should_have_correct_server_URL_and_paths_for_API_release(
        string docName,
        string serverUrlSuffix
    )
    {
        // Arrange
        RestRequest getDocumentRequest = GetRequest("/openapi/{docName}.json").AddUrlSegment("docName", docName);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getDocumentRequest);

        // Assert
        RestResponse response = await Assert.That(problemOrResponse).IsResponse().And.IsNotNull();

        (string serverUrl, string[] paths) = await ExtractServerUrlAndAllPathsAsync(response.Content);

        await Assert.That(serverUrl).EndsWith(serverUrlSuffix);

        await Assert.That(paths).DoesNotContain(path => path.Contains(serverUrlSuffix));
    }

    private static async Task<(string ServerUrl, string[] Paths)> ExtractServerUrlAndAllPathsAsync(string? json)
    {
        string jsonText = await Assert.That(json).IsNotNull();

        using JsonDocument doc = JsonDocument.Parse(jsonText);

        string serverUrl = doc
            .RootElement.GetProperty("servers")
            .EnumerateArray()
            .First()
            .GetProperty("url")
            .GetString()!;

        string[] paths = doc
            .RootElement.GetProperty("paths")
            .EnumerateObject()
            .Select(property => property.Name)
            .ToArray();

        return (serverUrl, paths);
    }

    [Test]
    [Arguments("not-a-document")]
    [Arguments("admin-api-v0")]
    [Arguments("admin-api-v0.3")]
    [Arguments("admin-api-v1.1")]
    [Arguments("public-api-v0")]
    [Arguments("public-api-v0.3")]
    [Arguments("public-api-v1.1")]
    public async Task OpenAPI_endpoint_should_return_404_on_non_existent_OpenAPI_document_requested(string docName)
    {
        // Arrange
        RestRequest getDocumentRequest = GetRequest("/openapi/{docName}.json").AddUrlSegment("docName", docName);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getDocumentRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    private static RestRequest GetRequest(string route) => new(route);
}

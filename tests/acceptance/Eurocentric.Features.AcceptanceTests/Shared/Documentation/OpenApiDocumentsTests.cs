using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Shared.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Documentation;

public sealed class OpenApiDocumentsTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("admin-api-v0.1", "/admin/api/v0.1/")]
    [InlineData("admin-api-v0.2", "/admin/api/v0.2/")]
    [InlineData("public-api-v0.1", "/public/api/v0.1/")]
    [InlineData("public-api-v0.2", "/public/api/v0.2/")]
    public async Task Should_be_able_to_retrieve_OpenAPI_JSON_document_by_name_without_using_API_key(
        string docName,
        string pathPrefix)
    {
        // Arrange
        RestRequest openApiRequest = new("openapi/{docName}.json");

        openApiRequest.AddUrlSegment("docName", docName);

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(openApiRequest, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, string? json) =
            (problemOrResponse.AsResponse.StatusCode, problemOrResponse.AsResponse.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);

        Assert.NotNull(json);

        string[] pathsInDocument = ExtractAllPaths(json);

        Assert.NotEmpty(pathsInDocument);
        Assert.All(pathsInDocument, path => Assert.StartsWith(pathPrefix, path));
    }

    [Theory]
    [InlineData("admin-api-v0")]
    [InlineData("admin-api-v0.3")]
    [InlineData("admin-api-v1.0")]
    [InlineData("public-api-v0")]
    [InlineData("public-api-v0.3")]
    [InlineData("public-api-v1.0")]
    public async Task Should_be_unable_to_retrieve_non_existent_OpenAPI_JSON_document_using_no_API_key(string docName)
    {
        // Arrange
        RestRequest openApiRequest = new("openapi/{docName}.json");

        openApiRequest.AddUrlSegment("docName", docName);

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(openApiRequest, TestContext.Current.CancellationToken);

        HttpStatusCode statusCode = problemOrResponse.AsProblem.StatusCode;

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, statusCode);
    }

    private static string[] ExtractAllPaths(string json)
    {
        using JsonDocument doc = JsonDocument.Parse(json);

        return doc.RootElement.GetProperty("paths")
            .EnumerateObject()
            .Select(property => property.Name)
            .ToArray();
    }
}

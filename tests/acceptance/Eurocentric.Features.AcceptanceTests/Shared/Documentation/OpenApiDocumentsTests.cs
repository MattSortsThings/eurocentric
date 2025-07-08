using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Documentation;

public static class OpenApiDocumentsTests
{
    private static string[] ExtractAllPaths(string? json)
    {
        Assert.NotNull(json);

        using JsonDocument doc = JsonDocument.Parse(json);

        return doc.RootElement.GetProperty("paths")
            .EnumerateObject()
            .Select(property => property.Name)
            .ToArray();
    }

    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("admin-api-v0.1", "/admin/api/v0.1/")]
        [InlineData("admin-api-v0.2", "/admin/api/v0.2/")]
        [InlineData("public-api-v0.1", "/public/api/v0.1/")]
        [InlineData("public-api-v0.2", "/public/api/v0.2/")]
        public async Task Should_serve_requested_OpenAPI_document_to_anonymous_client(
            string docName,
            string pathPrefix)
        {
            // Arrange
            RestRequest request = Get("openapi/{docName}.json").AddUrlSegment("docName", docName);

            // Act
            ProblemOrResponse problemOrResponse =
                await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            (HttpStatusCode statusCode, string? json) =
                (problemOrResponse.AsResponse.StatusCode, problemOrResponse.AsResponse.Content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);

            string[] pathsInDocument = ExtractAllPaths(json);

            Assert.NotEmpty(pathsInDocument);
            Assert.All(pathsInDocument, path => Assert.StartsWith(pathPrefix, path));
        }

        [Theory]
        [InlineData("admin-api-v0.3")]
        [InlineData("admin-api-v1.0")]
        [InlineData("public-api-v0.3")]
        [InlineData("public-api-v1.0")]
        public async Task Should_fail_on_non_existent_OpenAPI_document_requested(string docName)
        {
            // Arrange
            RestRequest request = Get("openapi/{docName}.json").AddUrlSegment("docName", docName);

            // Act
            ProblemOrResponse problemOrResponse =
                await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, problemOrResponse.AsProblem.StatusCode);
        }
    }
}

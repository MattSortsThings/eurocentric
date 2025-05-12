using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Shared.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Documentation;

public sealed class OpenApiDocumentsTests : AcceptanceTestBase
{
    public OpenApiDocumentsTests(WebAppFixture fixture) : base(fixture) { }

    [Theory]
    [InlineData("admin-api-v0.1", "/admin/api/v0.1")]
    [InlineData("admin-api-v0.2", "/admin/api/v0.2")]
    [InlineData("public-api-v0.1", "/public/api/v0.1")]
    [InlineData("public-api-v0.2", "/public/api/v0.2")]
    public async Task Should_return_status_code_200_with_requested_OpenAPI_JSON_document_given_anonymous_request(string docName,
        string expectedPathPrefix)
    {
        // Arrange
        RestRequest request = new("openapi/{docName}.json");

        request.AddUrlSegment(nameof(docName), docName);

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, string? content) = (responseOrProblem.AsT0.StatusCode, responseOrProblem.AsT0.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);

        Assert.NotNull(content);

        string[] paths = ExtractAllPaths(content);

        Assert.NotEmpty(paths);
        Assert.All(paths, path => Assert.StartsWith(expectedPathPrefix, path));
    }

    [Theory]
    [InlineData("admin-api-v0.3")]
    [InlineData("admin-api-v1.0")]
    [InlineData("public-api-v0.3")]
    [InlineData("public-api-v1.0")]
    public async Task Should_return_404_when_non_existent_OpenAPI_document_requested(string docName)
    {
        // Arrange
        RestRequest request = new("openapi/{docName}.json");

        request.AddUrlSegment(nameof(docName), docName);

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        HttpStatusCode statusCode = responseOrProblem.AsT1.StatusCode;

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, statusCode);
    }

    private static string[] ExtractAllPaths(string jsonContent)
    {
        JsonDocument json = JsonDocument.Parse(jsonContent);
        JsonElement root = json.RootElement;

        return root.GetProperty("paths").EnumerateObject().Select(property => property.Name).ToArray();
    }
}

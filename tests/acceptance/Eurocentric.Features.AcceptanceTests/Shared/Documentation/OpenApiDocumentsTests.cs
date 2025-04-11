using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Documentation;

public sealed class OpenApiDocumentsTests(WebAppFixture webAppFixture) : AcceptanceTestBase(webAppFixture)
{
    private const string Route = "openapi/{docName}.json";

    [Theory]
    [ClassData(typeof(ApiPathsTestCases))]
    public async Task Api_should_return_OK_with_OpenApi_document_containing_only_paths_in_requested_API_release(string docName,
        string pathPrefix)
    {
        // Arrange
        RestRequest restRequest = Get(Route).AddUrlSegment(nameof(docName), docName);

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        string[] pathsInDocument = ExtractAllPaths(restResponse.Content!);

        Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);

        Assert.NotEmpty(pathsInDocument);

        Assert.All(pathsInDocument, path => Assert.StartsWith(pathPrefix, path));
    }

    [Theory]
    [ClassData(typeof(DocumentInfoTestCases))]
    public async Task Api_should_return_OK_with_OpenApi_document_with_correct_document_info_for_requested_API_release(
        string docName,
        string expectedTitle,
        string expectedDescription,
        string expectedVersion)
    {
        // Arrange
        RestRequest restRequest = Get(Route).AddUrlSegment(nameof(docName), docName);

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        var (title, description, version) = ExtractDocumentInfo(restResponse.Content!);

        Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);

        Assert.Equal(expectedTitle, title);
        Assert.Equal(expectedDescription, description);
        Assert.Equal(expectedVersion, version);
    }

    private static string[] ExtractAllPaths(string jsonContent)
    {
        JsonDocument json = JsonDocument.Parse(jsonContent);
        JsonElement root = json.RootElement;

        return root.GetProperty("paths").EnumerateObject().Select(o => o.Name).ToArray();
    }

    private static (string Title, string Description, string Version) ExtractDocumentInfo(string jsonContent)
    {
        JsonDocument json = JsonDocument.Parse(jsonContent);
        JsonElement root = json.RootElement;
        JsonElement info = root.GetProperty("info");

        string title = info.GetProperty("title").GetString()!;
        string description = info.GetProperty("description").GetString()!;
        string version = info.GetProperty("version").GetString()!;

        return (title, description, version);
    }

    private sealed class ApiPathsTestCases : TheoryData<string, string>
    {
        public ApiPathsTestCases()
        {
            Add("admin-api-v0.1", "/admin/api/v0.1/");
            Add("admin-api-v0.2", "/admin/api/v0.2/");
            Add("public-api-v0.1", "/public/api/v0.1/");
        }
    }

    private sealed class DocumentInfoTestCases : TheoryData<string, string, string, string>
    {
        public DocumentInfoTestCases()
        {
            Add("admin-api-v0.1",
                "Eurocentric Admin API",
                "A web API for modelling the Eurovision Song Contest, 2016-present.",
                "0.1");

            Add("admin-api-v0.2",
                "Eurocentric Admin API",
                "A web API for modelling the Eurovision Song Contest, 2016-present.",
                "0.2");

            Add("public-api-v0.1",
                "Eurocentric Public API",
                "A web API for (over)analysing the Eurovision Song Contest, 2016-present.",
                "0.1");
        }
    }
}

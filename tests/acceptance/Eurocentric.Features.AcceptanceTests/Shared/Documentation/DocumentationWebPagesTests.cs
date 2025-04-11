using System.Net;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Documentation;

public sealed class DocumentationWebPagesTests(WebAppFixture webAppFixture) : AcceptanceTestBase(webAppFixture)
{
    private const string Route = "docs/{docName}";

    [Theory]
    [InlineData("admin-api-v0.1")]
    [InlineData("admin-api-v0.2")]
    [InlineData("admin-api-v1.0")]
    [InlineData("public-api-v0.1")]
    public async Task Api_should_return_OK_with_documentation_page_for_requested_document(string docName)
    {
        // Arrange
        RestRequest restRequest = new RestRequest(Route)
            .AddUrlSegment(nameof(docName), docName)
            .AddHeader("Accept", "text/html");

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);

        Assert.Contains($"\"title\":\"{docName}\"", restResponse.Content);
    }
}

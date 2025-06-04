using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Documentation;

public sealed class DocumentationWebPagesTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("admin-api-v0.1", "openapi/admin-api-v0.1.json")]
    [InlineData("admin-api-v0.2", "openapi/admin-api-v0.2.json")]
    [InlineData("public-api-v0.1", "openapi/public-api-v0.1.json")]
    [InlineData("public-api-v0.2", "openapi/public-api-v0.2.json")]
    public async Task Should_be_able_to_retrieve_documentation_web_page_name_without_using_API_key(
        string docName,
        string sourceUrl)
    {
        // Arrange
        RestRequest openApiRequest = new("docs/{docName}");

        openApiRequest.AddUrlSegment("docName", docName)
            .AddHeader("Accept", "text/html");

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(openApiRequest, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, string? html) =
            (problemOrResponse.AsResponse.StatusCode, problemOrResponse.AsResponse.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);

        Assert.NotNull(html);
        Assert.Contains(sourceUrl, html);
    }
}

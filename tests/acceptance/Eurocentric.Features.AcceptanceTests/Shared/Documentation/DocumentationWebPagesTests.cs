using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Documentation;

public sealed class DocumentationWebPagesTests : AcceptanceTestBase
{
    public DocumentationWebPagesTests(WebAppFixture fixture) : base(fixture) { }

    [Theory]
    [InlineData("admin-api-v0.1")]
    [InlineData("admin-api-v0.2")]
    [InlineData("admin-api-v1.0")]
    [InlineData("public-api-v0.1")]
    [InlineData("public-api-v0.2")]
    public async Task Should_return_status_code_200_with_requested_document_web_page_given_anonymous_request(string docName)
    {
        // Arrange
        RestRequest request = new("docs/{docName}");

        request.AddUrlSegment(nameof(docName), docName)
            .AddHeader("Accept", "text/html");

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        (HttpStatusCode statusCode, string? content) = (responseOrProblem.AsT0.StatusCode, responseOrProblem.AsT0.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);

        Assert.NotNull(content);
        Assert.NotEmpty(content);
    }
}

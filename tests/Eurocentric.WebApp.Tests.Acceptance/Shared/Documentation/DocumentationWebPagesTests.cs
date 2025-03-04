using System.Net;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.Documentation;

public static class DocumentationWebPagesTests
{
    private const string Route = "/docs";

    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("admin-api-v0.1")]
        [InlineData("admin-api-v0.2")]
        [InlineData("public-api-v0.1")]
        public async Task Should_return_200_with_HTML_when_documentation_page_requested(string documentName)
        {
            // Arrange
            RestRequest request = new RestRequest($"{Route}/{documentName}")
                .AddHeader("Accept", "text/html");

            // Act
            var (statusCode, content) = await SendAsync(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.OK),
                () => content.ShouldNotBeEmpty()
            );
        }
    }
}

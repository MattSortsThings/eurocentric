using System.Net;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.Documentation;

public static class DocumentationWebPagesTests
{
    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("admin-api-v0.1")]
        [InlineData("admin-api-v0.2")]
        [InlineData("public-api-v0.1")]
        public async Task Should_return_200_with_HTML_when_API_release_documentation_page_requested(string apiReleaseName)
        {
            // Arrange
            string resource = $"/docs/{apiReleaseName}";

            RestRequest request = new RestRequest(resource)
                .AddHeader("Accept", "text/html");

            // Act
            (HttpStatusCode statusCode, string content) = await SendAsync(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.OK),
                () => content.ShouldNotBeEmpty()
            );
        }
    }
}

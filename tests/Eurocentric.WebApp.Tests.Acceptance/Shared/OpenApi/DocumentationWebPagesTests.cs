using System.Net;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.OpenApi;

public static class DocumentationWebPagesTests
{
    public sealed class Api : AcceptanceTest
    {
        public Api(CleanWebAppFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("admin-api-v0.1")]
        [InlineData("public-api-v0.1")]
        public async Task Should_serve_documentation_web_page_for_each_API_release(string releaseGroupName)
        {
            // Arrange
            RestRequest request = new RestRequest($"/docs/{releaseGroupName}")
                .AddHeader("Accept", "text/html");

            // Act
            (HttpStatusCode statusCode, string? content) = await SendAsync(request);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.OK, statusCode),
                () => Assert.NotNull(content)
            );
        }
    }
}

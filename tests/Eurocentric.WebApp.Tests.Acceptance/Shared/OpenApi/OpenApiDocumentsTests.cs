using System.Net;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.OpenApi;

public static class OpenApiDocumentsTests
{
    public sealed class Api : AcceptanceTest
    {
        public Api(CleanWebAppFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("admin-api-v0.1.json")]
        [InlineData("public-api-v0.1.json")]
        public async Task Should_return_200_with_OpenAPI_JSON_document_when_document_requested(string docName)
        {
            // Arrange
            RestRequest request = Get("openapi/" + docName);

            // Act
            var (statusCode, content) = await SendAsync(request);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.OK, statusCode),
                () => Assert.Contains("\"openapi\": \"3.0.1\"", content)
            );
        }
    }
}

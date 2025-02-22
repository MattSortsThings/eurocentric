using System.Net;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.Documentation;

public static class OpenApiDocumentsTests
{
    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("admin-api-v0.1.json")]
        [InlineData("admin-api-v0.2.json")]
        [InlineData("public-api-v0.1.json")]
        public async Task Should_return_200_with_JSON_when_OpenAPI_document_requested(string documentName)
        {
            // Arrange
            string resource = $"/openapi/{documentName}";

            RestRequest request = new RestRequest(resource)
                .AddHeader("Accept", "application/json, application/problem+json");

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

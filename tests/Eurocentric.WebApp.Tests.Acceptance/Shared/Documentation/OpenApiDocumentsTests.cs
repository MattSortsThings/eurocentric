using System.Net;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.Documentation;

public static class OpenApiDocumentsTests
{
    private const string Route = "/openapi";

    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("admin-api-v0.1.json")]
        [InlineData("admin-api-v0.2.json")]
        [InlineData("admin-api-v1.0.json")]
        [InlineData("public-api-v0.1.json")]
        public async Task Should_return_200_with_OpenAPI_JSON_when_document_requested(string documentName)
        {
            // Arrange
            RestRequest request = Get($"{Route}/{documentName}");

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

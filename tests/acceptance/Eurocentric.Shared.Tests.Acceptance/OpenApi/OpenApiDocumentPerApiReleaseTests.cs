using System.Net;
using Eurocentric.Shared.Tests.Acceptance.Utils;
using Eurocentric.Tests.Utils.Fixtures;
using RestSharp;

namespace Eurocentric.Shared.Tests.Acceptance.OpenApi;

public static class OpenApiDocumentPerApiReleaseTests
{
    public sealed class OpenApi : SeededWebAppTests
    {
        public OpenApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData(Apis.Admin.V0.Latest.OpenApiDocName)]
        [InlineData(Apis.Public.V0.Latest.OpenApiDocName)]
        public async Task Should_serve_OpenAPI_document_per_API_release_with_no_authentication(string docName)
        {
            // Arrange
            RestRequest request = GetRequest.To("openapi/" + docName + ".json")
                .AddHeader("Accept", "application/json");

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}

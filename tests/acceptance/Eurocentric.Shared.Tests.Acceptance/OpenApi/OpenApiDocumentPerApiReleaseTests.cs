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
        public async Task Should_serve_open_api_document_per_api_release_with_no_authentication(string docName)
        {
            // Arrange
            RestRequest restRequest = Requests.Get.To("openapi/" + docName + ".json");

            // Act
            RestResponse result = await Sut.ExecuteAsync(restRequest, TestContext.Current.CancellationToken);

            // Assert
            result.ShouldHaveStatusCode(HttpStatusCode.OK);
        }
    }
}

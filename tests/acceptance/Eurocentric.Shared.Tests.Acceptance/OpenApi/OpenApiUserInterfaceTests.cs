using System.Net;
using Eurocentric.Shared.Tests.Acceptance.Utils;
using Eurocentric.Tests.Utils.Fixtures;
using RestSharp;

namespace Eurocentric.Shared.Tests.Acceptance.OpenApi;

public static class OpenApiUserInterfaceTests
{
    public sealed class Docs : SeededWebAppTests
    {
        public Docs(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData(Apis.Admin.V0.Latest.OpenApiDocName)]
        [InlineData(Apis.Public.V0.Latest.OpenApiDocName)]
        public async Task Should_serve_documentation_page_per_API_release_with_no_authentication(string docName)
        {
            // Arrange
            string resourceUri = "docs/" + docName;

            RestRequest request = new RestRequest(resourceUri)
                .AddHeader("Accept", "text/html");

            // Act
            RestResponse result = await Sut.ExecuteAsync(request, TestContext.Current.CancellationToken);

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}

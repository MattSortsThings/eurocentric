using System.Net;
using Eurocentric.Shared.Tests.Acceptance.Utils;
using Eurocentric.Tests.Utils.Attributes;
using Eurocentric.Tests.Utils.Fixtures;
using RestSharp;

namespace Eurocentric.Shared.Tests.Acceptance.Placeholders;

public static class PlaceholderTests
{
    [PlaceholderTest]
    public sealed class PublicApi : SeededWebAppTest
    {
        public PublicApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_return_404_given_request_to_non_existent_route()
        {
            // Arrange
            RestRequest restRequest = Requests.Get.To("public/api/v1.0/non-existent");

            // Act
            RestResponse result = await Sut.ExecuteAsync(restRequest, TestContext.Current.CancellationToken);

            // Assert
            result.ShouldHaveStatusCode(HttpStatusCode.NotFound);
        }
    }
}

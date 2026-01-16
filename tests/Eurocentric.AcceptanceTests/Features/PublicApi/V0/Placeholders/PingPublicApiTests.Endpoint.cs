using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V0.Placeholders;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Features.PublicApi.V0.Placeholders;

public static class PingPublicApiTests
{
    [Category("placeholder")]
    public sealed class Endpoint : AcceptanceTestBase
    {
        [Test]
        [Repeat(250)]
        public async Task Should_succeed_with_200_OK_and_fixed_response_body()
        {
            // Arrange
            RestRequest request = new("/public/api/v0/ping");

            // Act
            SuccessOrFailureRestResponse<PingPublicApiResponseBody> response =
                await SystemUnderTest.SendAsync<PingPublicApiResponseBody>(request);

            // Assert
            await Assert.That(response.IsSuccess).IsTrue();

            await Assert
                .That(response.GetSuccessRestResponse().Data)
                .HasProperty(pp => pp.ApiName, "Public API")
                .And.Member(pp => pp.Items, collection => collection.IsEquivalentTo(["Blobby", "Blobby", "Blobby"]));
        }
    }
}

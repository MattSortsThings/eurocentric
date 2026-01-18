using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V0.Placeholders;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Features.PublicApi.V0.Placeholders;

public static class PingPublicApiTests
{
    public sealed class Endpoint : AcceptanceTestBase
    {
        [Test]
        [Repeat(250)]
        public async Task Should_return_200_OK_and_fixed_response()
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
                .HasProperty(pa => pa.ApiName, "Public API")
                .And.Member(
                    pa => pa.Items,
                    collection => collection.Count().IsEqualTo(3).And.ContainsOnly(v => v == "Blobby")
                );
        }
    }
}

using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V0.Placeholders;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Features.AdminApi.V0.Placeholders;

public static class PingAdminApiTests
{
    [Category("placeholder")]
    public sealed class Endpoint : AcceptanceTestBase
    {
        [Test]
        [Repeat(250)]
        public async Task Should_succeed_with_200_OK_and_fixed_response_body()
        {
            // Arrange
            RestRequest request = new("/admin/api/v0/ping");

            // Act
            SuccessOrFailureRestResponse<PingAdminApiResponseBody> response =
                await SystemUnderTest.SendAsync<PingAdminApiResponseBody>(request);

            // Assert
            await Assert.That(response.IsSuccess).IsTrue();

            await Assert
                .That(response.GetSuccessRestResponse().Data)
                .HasProperty(pa => pa.ApiName, "Admin API")
                .And.Member(pa => pa.Items, collection => collection.IsEquivalentTo(["Blobby", "Blobby", "Blobby"]));
        }
    }
}

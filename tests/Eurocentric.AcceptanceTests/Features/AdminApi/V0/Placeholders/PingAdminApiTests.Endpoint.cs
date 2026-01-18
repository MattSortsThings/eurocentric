using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils.Contracts;
using Eurocentric.Apis.Admin.V0.Placeholders;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Features.AdminApi.V0.Placeholders;

public static class PingAdminApiTests
{
    [NotInParallel("AdminApi.V0.Placeholders.PingAdminApiTests")]
    public sealed class Endpoint : AcceptanceTestBase
    {
        [Test]
        [Repeat(150)]
        public async Task Should_return_200_OK_and_fixed_response()
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
                .And.Member(
                    pa => pa.Items,
                    collection => collection.Count().IsEqualTo(4).And.ContainsOnly(v => v == "Blobby")
                );
        }

        [Test]
        [Repeat(150)]
        public async Task Should_also_return_200_OK_and_fixed_response()
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
                .And.Member(
                    pa => pa.Items,
                    collection => collection.Count().IsEqualTo(4).And.ContainsOnly(v => v == "Blobby")
                );
        }
    }
}

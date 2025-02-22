using System.Net;
using Eurocentric.AdminApi.V0.Calculations.GetCalculation;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.AdminApi.V0.Calculations;

public static class GetCalculationTests
{
    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_200_with_calculation_given_its_ID()
        {
            // Arrange
            Guid calculationId = Guid.Parse("54e3df94-6b6a-45aa-8be0-3bd5f6503cc3");

            RestRequest request = Get($"admin/api/v0.2/calculations/{calculationId}")
                .UseAdminApiKey();

            // Act
            (HttpStatusCode statusCode, GetCalculationResult result) = await SendAsync<GetCalculationResult>(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.OK),
                () => Assert.Equal(calculationId, result.Calculation.Id)
            );
        }
    }
}

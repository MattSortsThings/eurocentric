using System.Net;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.AdminApi.V0.Calculations;

public static class GetCalculationTests
{
    private const string Route = "/admin/api/v0.1/calculations";

    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_200_with_requested_calculation_given_valid_request()
        {
            // Arrange
            Guid calculationId = Guid.Parse("174b7955-dbaf-421f-aa91-d996c699c48f");

            RestRequest request = Get($"{Route}/{calculationId}");

            // Act
            (HttpStatusCode statusCode, CreateCalculationResult result) = await SendAsync<CreateCalculationResult>(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.OK),
                () => Assert.Equal(calculationId, result.Calculation.Id)
            );
        }
    }
}

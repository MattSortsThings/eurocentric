using System.Net;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.AdminApi.V0.Calculations.Models;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.AdminApi.V0.Calculations;

public static class CreateCalculationTests
{
    public sealed class AdminApiV0 : AcceptanceTest
    {
        public AdminApiV0(CleanWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_return_200_with_calculation_given_valid_request()
        {
            // Arrange
            RestRequest request = Post("admin/api/v0.1/calculations")
                .AddJsonBody(new CreateCalculationCommand { X = 10, Y = 3, Operation = Operation.Modulus });

            object expected = new { Calculation = new { X = 10, Y = 3, Operation = Operation.Modulus, Result = 1 } };

            // Act
            var (statusCode, result) = await SendAsync<CreateCalculationResult>(request);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.OK, statusCode),
                () => Assert.Equivalent(expected, result)
            );
        }
    }
}

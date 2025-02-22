using System.Net;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.AdminApi.V0.Calculations.Models;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.AdminApi.V0.Calculations;

public static class CreateCalculationTests
{
    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_201_with_created_calculation_and_its_location_given_valid_request()
        {
            // Arrange
            CreateCalculationCommand command = new() { X = 5, Y = 10, Operation = Operation.Product };

            object expectedResult = new { Calculation = new { X = 5, Y = 10, Operation = Operation.Product, Output = 50 } };

            RestRequest request = Post("admin/api/v0.2/calculations")
                .UseAdminApiKey()
                .AddJsonBody(command);

            // Act
            (HttpStatusCode statusCode, CreateCalculationResult result, string location) =
                await SendAsync<CreateCalculationResult>(request);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.Created, statusCode),
                () => Assert.Equivalent(expectedResult, result),
                () => Assert.EndsWith($"admin/api/v0.2/calculations/{result.Calculation.Id}", location)
            );
        }
    }
}

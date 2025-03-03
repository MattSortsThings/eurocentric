using System.Net;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.AdminApi.V0.Calculations.Models;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.AdminApi.V0.Calculations;

public static class CreateCalculationTests
{
    private const string Route = "/admin/api/v0.2/calculations";

    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_201_with_created_calculation_and_location_given_valid_request()
        {
            // Arrange
            CreateCalculationCommand command = new() { X = 10, Y = 100, Operation = Operation.Product };

            RestRequest request = Post(Route).AddJsonBody(command);

            // Act
            (HttpStatusCode statusCode, CreateCalculationResult result, string location) =
                await SendAsync<CreateCalculationResult>(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.Created),
                () => Assert.Equivalent(command, result.Calculation),
                () => location.ShouldEndWith($"{Route}/{result.Calculation.Id}")
            );
        }
    }
}

using ErrorOr;
using Eurocentric.AdminApi.Tests.Integration.Utils;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.AdminApi.V0.Calculations.Models;

namespace Eurocentric.AdminApi.Tests.Integration.V0.Calculations;

public static class CreateCalculationTests
{
    public sealed class Handler(CleanWebAppFixture fixture) : IntegrationTest(fixture)
    {
        [Theory]
        [InlineData(1, 1, Operation.Product, 1)]
        [InlineData(1, 1, Operation.Modulus, 0)]
        [InlineData(10, 4, Operation.Product, 40)]
        [InlineData(10, 4, Operation.Modulus, 2)]
        public async Task Should_return_result_given_valid_command(int x, int y, Operation operation, int expectedOutput)
        {
            // Arrange
            CreateCalculationCommand command = new() { X = x, Y = y, Operation = operation };

            object expectedResult = new { Calculation = new { X = x, Y = y, Operation = operation, Output = expectedOutput } };

            // Act
            ErrorOr<CreateCalculationResult> errorsOrResult = await SendAsync(command);

            // Assert
            Assert.Multiple(
                () => Assert.False(errorsOrResult.IsError),
                () => Assert.Equivalent(expectedResult, errorsOrResult.Value)
            );
        }
    }
}

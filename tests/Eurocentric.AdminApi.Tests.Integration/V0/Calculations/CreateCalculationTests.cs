using ErrorOr;
using Eurocentric.AdminApi.Tests.Integration.Utils;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.AdminApi.V0.Calculations.Models;

namespace Eurocentric.AdminApi.Tests.Integration.V0.Calculations;

public static class CreateCalculationTests
{
    public sealed class Handler : IntegrationTest
    {
        public Handler(CleanWebAppFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData(2, 3, Operation.Product, 6)]
        [InlineData(10, 3, Operation.Product, 30)]
        [InlineData(10, 3, Operation.Modulus, 1)]
        [InlineData(7, 7, Operation.Modulus, 0)]
        public async Task Should_return_result_given_valid_command(int x, int y, Operation operation, int result)
        {
            // Arrange
            CreateCalculationCommand command = new() { X = x, Y = y, Operation = operation };

            object expectedResult = new { Calculation = new { X = x, Y = y, Operation = operation, Result = result } };

            // Act
            ErrorOr<CreateCalculationResult> errorOrResult = await SendAsync(command);

            // Assert
            Assert.Multiple(
                () => Assert.False(errorOrResult.IsError),
                () => Assert.Equivalent(expectedResult, errorOrResult.Value)
            );
        }
    }
}

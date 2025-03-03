using ErrorOr;
using Eurocentric.AdminApi.Tests.Integration.Utils;
using Eurocentric.AdminApi.Tests.Integration.Utils.Assertions;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.AdminApi.V0.Calculations.Models;

namespace Eurocentric.AdminApi.Tests.Integration.V0.Calculations;

public static class CreateCalculationTests
{
    public sealed class AppPipeline(CleanWebAppFixture fixture) : IntegrationTest(fixture)
    {
        [Theory]
        [ClassData(typeof(HappyPathTestCases))]
        public async Task Should_return_created_calculation_given_valid_command(int x, int y, Operation operation, int output)
        {
            // Arrange
            CreateCalculationCommand command = new() { X = x, Y = y, Operation = operation };

            object expectedResult = new { Calculation = new { X = x, Y = y, Operation = operation, Output = output } };

            // Act
            ErrorOr<CreateCalculationResult> errorsOrResult = await SendAsync(command);

            // Assert
            Assert.Multiple(
                () => errorsOrResult.ShouldNotBeError(),
                () => Assert.Equivalent(expectedResult, errorsOrResult.Value)
            );
        }

        private sealed class HappyPathTestCases : TheoryData<int, int, Operation, int>
        {
            public HappyPathTestCases()
            {
                Add(0, 0, Operation.Product, 0);
                Add(10, 9, Operation.Product, 90);
                Add(10, 9, Operation.Modulus, 1);
            }
        }
    }
}

using Eurocentric.AdminApi.Tests.Integration.TestUtils;
using Eurocentric.AdminApi.V0.Calculations.Common;
using Eurocentric.AdminApi.V0.CreateCalculation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(TestCaseSerializer),
    typeof(CreateCalculationRequest),
    typeof(CreateCalculationResponse))]

namespace Eurocentric.AdminApi.Tests.Integration.V0.Calculations;

public static class CreateCalculationTests
{
    public sealed class Feature : IntegrationTest
    {
        public Feature(CleanWebAppFixture webAppFixture) : base(webAppFixture)
        {
        }

        [Theory]
        [ClassData(typeof(TestCases))]
        public async Task Should_return_200_with_response_given_valid_request(CreateCalculationRequest request,
            CreateCalculationResponse expectedResponse)
        {
            // Act
            Ok<CreateCalculationResponse> result = await CreateCalculationEndpoint.ExecuteAsync(request,
                Sender,
                TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(StatusCodes.Status200OK, result.StatusCode),
                () => Assert.Equivalent(expectedResponse, result.Value)
            );
        }
    }

    private sealed class TestCases : TheoryData<CreateCalculationRequest, CreateCalculationResponse>
    {
        public TestCases()
        {
            Add(new CreateCalculationRequest { X = 1, Y = 1, Operation = Operation.Product },
                new CreateCalculationResponse(new Calculation(1, 1, Operation.Product, 1)));
        }
    }
}

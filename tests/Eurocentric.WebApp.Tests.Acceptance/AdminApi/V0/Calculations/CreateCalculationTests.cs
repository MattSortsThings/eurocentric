using System.Net;
using Eurocentric.AdminApi.V0.Calculations.Common;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.WebApp.Tests.Acceptance.TestUtils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.AdminApi.V0.Calculations;

public static class CreateCalculationTests
{
    public sealed class Endpoint : AcceptanceTest
    {
        private const string Resource = "admin/api/v0.1/calculations";


        public Endpoint(CleanWebAppFixture webAppFixture) : base(webAppFixture)
        {
        }

        [Fact]
        public async Task Should_return_200_with_response_given_valid_request()
        {
            // Arrange
            RestRequest request = new RestRequest(Resource, Method.Post)
                .AddHeader("Accept", "application/json")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(new CreateCalculationRequest { X = 10, Y = 5, Operation = Operation.Product });

            // Act
            RestResponse<CreateCalculationResponse> response =
                await Sut.ExecuteAsync<CreateCalculationResponse>(request, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}

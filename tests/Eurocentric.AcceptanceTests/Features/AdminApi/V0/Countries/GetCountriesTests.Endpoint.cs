using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils.Contracts;
using Eurocentric.Apis.Admin.V0.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Features.AdminApi.V0.Countries;

public static class GetCountriesTests
{
    private const string ConstraintKey = "AdminApi.V0.Countries.GetCountriesTests";

    [NotInParallel(ConstraintKey)]
    public sealed class Endpoint : AcceptanceTestBase
    {
        [Test]
        [Arguments("v0.1")]
        [Arguments("v0.2")]
        public async Task Should_return_200_OK_and_response_body(string apiVersion)
        {
            // Arrange
            RestRequest request = RestRequests
                .Get("/admin/api/{apiVersion}/countries")
                .AddUrlSegment("apiVersion", apiVersion);

            // Act
            SuccessOrFailureRestResponse<GetCountriesResponseBody> response =
                await SystemUnderTest.SendAsync<GetCountriesResponseBody>(request);

            // Assert
            await Assert.That(response.IsSuccess).IsTrue();
        }
    }
}

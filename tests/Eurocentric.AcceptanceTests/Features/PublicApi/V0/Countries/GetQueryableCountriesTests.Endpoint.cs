using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils.Contracts;
using Eurocentric.Apis.Public.V0.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Features.PublicApi.V0.Countries;

public static class GetQueryableCountriesTests
{
    private const string ConstraintKey = "PublicApi.V0.Countries.GetQueryableCountriesTests";

    [NotInParallel(ConstraintKey)]
    public sealed class Endpoint : AcceptanceTestBase
    {
        [Test]
        [Arguments("v0.1")]
        public async Task Should_return_200_OK_and_response_body(string apiVersion)
        {
            // Arrange
            RestRequest request = RestRequests
                .Get("/public/api/{apiVersion}/countries")
                .AddUrlSegment("apiVersion", apiVersion);

            // Act
            SuccessOrFailureRestResponse<GetQueryableCountriesResponseBody> response =
                await SystemUnderTest.SendAsync<GetQueryableCountriesResponseBody>(request);

            // Assert
            await Assert.That(response.IsSuccess).IsTrue();
        }
    }
}

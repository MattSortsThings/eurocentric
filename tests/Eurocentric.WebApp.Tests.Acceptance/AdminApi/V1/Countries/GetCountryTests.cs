using System.Net;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.AdminApi.V1.Countries;

public static class GetCountryTests
{
    private const string Route = "/admin/api/v1.0/countries";

    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_200_with_placeholder_country_given_any_request()
        {
            // Arrange
            Guid guid = Guid.NewGuid();

            RestRequest request = Get($"{Route}/{guid}").UseAdminApiKey();

            // Act
            var (statusCode, _) = await SendAsync(request);

            // Assert
            statusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}

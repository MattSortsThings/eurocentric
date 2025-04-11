using System.Net;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountry;
using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class GetCountryTests(WebAppFixture webAppFixture) : AcceptanceTestBase(webAppFixture)
{
    private const string Route = "admin/api/v1.0/countries/{countryId}";

    [Fact]
    public async Task AdminApi_should_return_OK_with_placeholder_country_when_any_country_requested()
    {
        // Arrange
        Guid countryId = Guid.Parse("586c9ff9-3746-4dba-ac41-58714b04cccc");

        RestRequest restRequest = Get(Route).UseSecretApiKey().AddUrlSegment(nameof(countryId), countryId);

        // Act
        var (statusCode, response, _) =
            await Sut.SendAsync<GetCountryResponse>(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);

        Assert.Equal(countryId, response.Country.Id);
    }
}

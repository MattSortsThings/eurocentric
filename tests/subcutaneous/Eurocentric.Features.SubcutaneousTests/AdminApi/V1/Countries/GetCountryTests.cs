using Eurocentric.Features.AdminApi.V1.Countries.GetCountry;
using Eurocentric.Features.SubcutaneousTests.Utils;
using Eurocentric.TestUtils.WebAppFixtures;

namespace Eurocentric.Features.SubcutaneousTests.AdminApi.V1.Countries;

public sealed class GetCountryTests(WebAppFixture webAppFixture) : SubcutaneousTestBase(webAppFixture)
{
    [Fact]
    public async Task App_should_return_OK_with_placeholder_country_when_any_country_requested()
    {
        // Arrange
        Guid countryId = Guid.Parse("586c9ff9-3746-4dba-ac41-58714b04cccc");

        GetCountryQuery query = new(countryId);

        // Act
        var (isError, response, _) = await Sut.SendAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(isError);

        Assert.NotNull(response);
        Assert.Equal(countryId, response.Country.Id);
    }
}

using Eurocentric.Domain.Placeholders;
using Eurocentric.Features.PublicApi.V0.Stations.GetStations;
using Eurocentric.Features.SubcutaneousTests.Utils;
using Eurocentric.TestUtils.WebAppFixtures;

namespace Eurocentric.Features.SubcutaneousTests.PublicApi.V0.Stations;

public sealed class GetStationsTests(WebAppFixture webAppFixture) : SubcutaneousTestBase(webAppFixture)
{
    [Theory]
    [InlineData(Line.Jubilee)]
    [InlineData(Line.Metropolitan)]
    [InlineData(Line.Northern)]
    public async Task App_should_return_all_stations_matching_query(Line line)
    {
        // Arrange
        GetStationsQuery query = new() { Line = line };

        // Act
        (bool isError, GetStationsResponse response, _) = await Sut.SendAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(isError);

        Assert.NotEmpty(response.Stations);
        Assert.All(response.Stations, station => Assert.Equal(line, station.Line));
    }
}

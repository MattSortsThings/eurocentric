using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Stations.GetStation;
using Eurocentric.Features.SubcutaneousTests.Utils;
using Eurocentric.TestUtils.WebAppFixtures;

namespace Eurocentric.Features.SubcutaneousTests.AdminApi.V0.Stations;

public sealed class GetStationTests(WebAppFixture webAppFixture) : SubcutaneousTestBase(webAppFixture)
{
    [Fact]
    public async Task App_should_return_requested_station_when_existing_station_requested()
    {
        // Arrange
        const int stationId = 1;

        GetStationQuery query = new(stationId);

        // Act
        var (isError, response, _) = await Sut.SendAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(isError);

        Assert.Equal(stationId, response.Station.Id);
    }

    [Fact]
    public async Task App_should_return_NotFound_error_when_requested_station_does_not_exist()
    {
        // Arrange
        const int stationId = 0;

        GetStationQuery query = new(stationId);

        // Act
        var (isError, response, firstError) = await Sut.SendAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(isError);

        Assert.Null(response);

        Assert.Equal(ErrorType.NotFound, firstError.Type);
    }
}

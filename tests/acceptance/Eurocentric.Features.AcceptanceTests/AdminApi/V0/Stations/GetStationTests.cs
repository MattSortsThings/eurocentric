using System.Net;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Stations.GetStation;
using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Stations;

public sealed class GetStationTests(WebAppFixture webAppFixture) : AcceptanceTestBase(webAppFixture)
{
    private const string Route = "admin/api/v0.2/stations/{stationId}";

    [Fact]
    public async Task AdminApi_should_return_OK_with_requested_station_when_requested_station_exists()
    {
        // Arrange
        const int stationId = 1;

        RestRequest restRequest = Get(Route).UseSecretApiKey().AddUrlSegment(nameof(stationId), stationId);

        // Act
        (HttpStatusCode statusCode, GetStationResponse response, _) =
            await Sut.SendAsync<GetStationResponse>(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);

        Assert.Equal(stationId, response.Station.Id);
    }

    [Fact]
    public async Task AdminApi_should_return_NotFound_when_requested_station_does_not_exist()
    {
        // Arrange
        const int stationId = 0;

        RestRequest restRequest = Get(Route).UseSecretApiKey().AddUrlSegment(nameof(stationId), stationId);

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, restResponse.StatusCode);
    }
}

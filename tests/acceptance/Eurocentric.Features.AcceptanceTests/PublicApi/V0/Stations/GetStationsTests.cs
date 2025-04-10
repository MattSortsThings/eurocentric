using System.Net;
using Eurocentric.Domain.Placeholders;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V0.Stations.GetStations;
using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Stations;

public sealed class GetStationsTests(WebAppFixture webAppFixture) : AcceptanceTestBase(webAppFixture)
{
    private const string Route = "public/api/v0.1/stations";

    [Theory]
    [InlineData(Line.Jubilee)]
    [InlineData(Line.Metropolitan)]
    [InlineData(Line.Northern)]
    public async Task PublicApi_should_return_OK_with_all_stations_matching_query(Line line)
    {
        // Arrange
        RestRequest restRequest = Get(Route).AddQueryParameter(nameof(line), line);

        // Act
        (HttpStatusCode statusCode, GetStationsResponse response, _) =
            await Sut.SendAsync<GetStationsResponse>(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);

        Assert.NotEmpty(response.Stations);
        Assert.All(response.Stations, station => Assert.Equal(line, station.Line));
    }
}

using System.Net;
using Eurocentric.PublicApi.V0.Stations.GetStations;
using Eurocentric.PublicApi.V0.Stations.Models;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.PublicApi.V0.Stations;

public static class GetStationsTests
{
    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_200_with_requested_stations_given_valid_request()
        {
            // Arrange
            const Line line = Line.HammersmithAndCity;

            RestRequest request = Get("public/api/v0.1/stations")
                .AddQueryParameter("line", line)
                .UsePublicApiKey();

            // Act
            (HttpStatusCode statusCode, GetStationsResult result) = await SendAsync<GetStationsResult>(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.OK),
                () => Assert.NotEmpty(result.Stations),
                () => Assert.All(result.Stations, station => Assert.Equal(line, station.Line))
            );
        }
    }
}

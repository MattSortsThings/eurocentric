using System.Net;
using Eurocentric.PublicApi.Stations.GetStations;
using Eurocentric.PublicApi.Stations.Models;
using Eurocentric.Tests.Assertions;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.PublicApi.V0.Stations;

public static class GetStationsTests
{
    private const string Route = "/public/api/v0.1/stations";

    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_200_with_stations_given_valid_request()
        {
            // Arrange
            const Line line = Line.Jubilee;

            RestRequest request = Get(Route).AddQueryParameter(nameof(line), line).UsePublicApiKey();

            // Act
            (HttpStatusCode statusCode, GetStationsResult result) = await SendAsync<GetStationsResult>(request);

            // Assert
            statusCode.ShouldBe(HttpStatusCode.OK);
            result.Stations.ShouldNotBeEmpty();
        }
    }
}

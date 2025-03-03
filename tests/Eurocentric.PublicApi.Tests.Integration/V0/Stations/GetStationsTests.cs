using ErrorOr;
using Eurocentric.PublicApi.Stations.GetStations;
using Eurocentric.PublicApi.Stations.Models;
using Eurocentric.PublicApi.Tests.Integration.Utils;
using Eurocentric.PublicApi.Tests.Integration.Utils.Assertions;

namespace Eurocentric.PublicApi.Tests.Integration.V0.Stations;

public static class GetStationsTests
{
    public sealed class AppPipeline(SeededWebAppFixture fixture) : IntegrationTest(fixture)
    {
        [Theory]
        [InlineData(Line.HammersmithAndCity)]
        [InlineData(Line.Jubilee)]
        [InlineData(Line.Northern)]
        public async Task Should_return_requested_stations_given_valid_query(Line line)
        {
            // Arrange
            GetStationsQuery query = new() { Line = line };

            // Act
            ErrorOr<GetStationsResult> errorsOrResult = await SendAsync(query);

            // Assert
            Assert.Multiple(
                () => errorsOrResult.ShouldNotBeError(),
                () => Assert.NotEmpty(errorsOrResult.Value.Stations),
                () => Assert.All(errorsOrResult.Value.Stations, station => Assert.Equal(line, station.Line))
            );
        }
    }
}

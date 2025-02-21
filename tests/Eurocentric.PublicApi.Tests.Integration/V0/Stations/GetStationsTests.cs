using ErrorOr;
using Eurocentric.PublicApi.Tests.Integration.Utils;
using Eurocentric.PublicApi.V0.Stations.GetStations;
using Eurocentric.PublicApi.V0.Stations.Models;

namespace Eurocentric.PublicApi.Tests.Integration.V0.Stations;

public static class GetStationsTests
{
    public sealed class Handler(SeededWebAppFixture fixture) : IntegrationTest(fixture)
    {
        [Theory]
        [InlineData(Line.Northern)]
        [InlineData(Line.HammersmithAndCity)]
        [InlineData(Line.Jubilee)]
        public async Task Should_return_result_given_valid_query(Line line)
        {
            // Arrange
            GetStationsQuery query = new() { Line = line };

            // Act
            ErrorOr<GetStationsResult> errorsOrResult = await SendAsync(query);

            // Assert
            Assert.Multiple(
                () => Assert.False(errorsOrResult.IsError),
                () => Assert.NotEmpty(errorsOrResult.Value.Stations),
                () => Assert.All(errorsOrResult.Value.Stations, station => Assert.Equal(line, station.Line))
            );
        }
    }
}

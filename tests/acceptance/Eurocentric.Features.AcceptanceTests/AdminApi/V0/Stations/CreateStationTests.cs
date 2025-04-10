using System.Net;
using Eurocentric.Domain.Placeholders;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Stations.CreateStation;
using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Stations;

public sealed class CreateStationTests(WebAppFixture webAppFixture) : AcceptanceTestBase(webAppFixture)
{
    private const string Route = "admin/api/v0.2/stations";

    [Fact]
    public async Task AdminApi_should_return_Created_with_CreateStationResponse_given_valid_request()
    {
        // Arrange
        CreateStationCommand request = new() { Name = "Oval", Line = Line.Northern };

        RestRequest restRequest = Post(Route).AddJsonBody(request);

        // Act
        (HttpStatusCode statusCode, CreateStationResponse response, IReadOnlyCollection<HeaderParameter> headers) =
            await Sut.SendAsync<CreateStationResponse>(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Created, statusCode);

        Assert.Equal("Oval", response.Station.Name);
        Assert.Equal(Line.Northern, response.Station.Line);

        Assert.Single(headers, header =>
            header.Name == "Location" && header.Value.EndsWith($"/admin/api/v0.2/stations/{response.Station.Id}"));
    }

    [Fact]
    public async Task AdminApi_should_return_BadRequest_when_request_Line_property_has_invalid_enum_int_value()
    {
        // Arrange
        RestRequest restRequest = Post(Route).AddJsonBody(new { Name = "Name", Line = 999999 });

        // Act
        RestResponse restResponse = await Sut.SendAsync(restRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, restResponse.StatusCode);
    }
}

using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public static class GetCountryTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_retrieve_dummy_country(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            admin.Given_I_want_to_retrieve_the_country_with_ID("5b31ad46-e8dc-42d0-aac5-d7f6955e2d6d");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_country_ID_should_be("5b31ad46-e8dc-42d0-aac5-d7f6955e2d6d");
        }
    }

    private sealed class Admin : AdminActor<GetCountryResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_the_country_with_ID(string countryId) =>
            Request = RequestFactory.Countries.GetCountryAsync(Guid.Parse(countryId));

        public void Then_the_retrieved_country_ID_should_be(string countryId)
        {
            Assert.NotNull(ResponseObject);

            Guid expectedId = Guid.Parse(countryId);

            Assert.Equal(expectedId, ResponseObject.Country.Id);
        }
    }
}

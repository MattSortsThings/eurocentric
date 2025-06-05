using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class GetCountryTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_dummy_country_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        admin.Given_I_want_to_retrieve_the_country_with_ID("40a4ce1c-34f2-4a84-abeb-d3e400cd01d3");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_country_should_have_the_ID("40a4ce1c-34f2-4a84-abeb-d3e400cd01d3");
    }

    private sealed class AdminActor : ActorWithResponse<GetCountryResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver, IWebAppFixtureBackDoor backDoor) : base(apiDriver)
        {
            BackDoor = backDoor;
        }

        private IWebAppFixtureBackDoor BackDoor { get; }

        public void Given_I_want_to_retrieve_the_country_with_ID(string countryId)
        {
            Guid targetId = Guid.Parse(countryId);
            SendMyRequest = apiDriver => apiDriver.Countries.GetCountry(targetId, TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_country_should_have_the_ID(string countryId)
        {
            Assert.NotNull(ResponseObject);

            Guid expectedCountryId = Guid.Parse(countryId);

            Assert.Equal(expectedCountryId, ResponseObject.Country.Id);
        }
    }
}

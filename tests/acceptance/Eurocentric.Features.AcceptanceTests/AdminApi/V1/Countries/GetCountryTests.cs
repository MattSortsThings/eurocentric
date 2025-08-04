using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountry;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class GetCountryTests : SerialCleanAcceptanceTest
{
    [Test]
    [Arguments("v1.0")]
    public async Task Endpoint_should_retrieve_dummy_country(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_country_with_ID("84662458-b362-41a8-bb2f-ed03272bfc91");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_country_should_have_the_ID("84662458-b362-41a8-bb2f-ed03272bfc91");
    }

    private sealed class AdminActor : AdminActorWithResponse<GetCountryResponse>
    {
        public AdminActor(IApiDriver apiDriver) : base(apiDriver)
        {
        }

        public void Given_I_want_to_retrieve_the_country_with_ID(string countryId)
        {
            Guid myCountryId = Guid.Parse(countryId);
            Request = ApiDriver.RequestFactory.Countries.GetCountry(myCountryId);
        }

        public async Task Then_the_retrieved_country_should_have_the_ID(string countryId)
        {
            Guid expectedCountryId = Guid.Parse(countryId);

            GetCountryResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.Country.Id).IsEqualTo(expectedCountryId);
        }
    }
}

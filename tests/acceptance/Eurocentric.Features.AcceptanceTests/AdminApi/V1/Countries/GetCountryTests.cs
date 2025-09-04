using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.DataSourceGenerators;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountry;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class GetCountryTests : SerialCleanAcceptanceTest
{
    [Test]
    [AdminApiV1Point0AndUp]
    public async Task Endpoint_should_retrieve_dummy_country(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_country_with_ID("875e7093-3130-4fa1-a4a2-23fb37f2a41a");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_country_should_have_ID("875e7093-3130-4fa1-a4a2-23fb37f2a41a");
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<GetCountryResponse>(apiDriver)
    {
        public void Given_I_want_to_retrieve_the_country_with_ID(string countryId)
        {
            Guid targetCountryId = Guid.Parse(countryId);

            Request = ApiDriver.RequestFactory.Countries.GetCountry(targetCountryId);
        }

        public async Task Then_the_retrieved_country_should_have_ID(string countryId)
        {
            GetCountryResponse response = await Assert.That(ResponseBody).IsNotNull();

            Guid expectedCountryId = Guid.Parse(countryId);

            Guid actualCountryId = response.Country.Id;

            await Assert.That(actualCountryId).IsEqualTo(expectedCountryId);
        }
    }
}

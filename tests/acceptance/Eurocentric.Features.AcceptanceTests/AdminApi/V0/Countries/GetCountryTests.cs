using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils.DataSourceGenerators;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils.Helpers.Countries;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Countries.GetCountry;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Countries;

public sealed class GetCountryTests : CleanSerialAcceptanceTest
{
    [Test]
    [AdminApiV0Point1AndUp]
    public async Task Endpoint_should_retrieve_requested_country(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");

        await admin.Given_I_want_to_retrieve_my_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_country_should_be_my_country();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<GetCountryResponse>(apiDriver)
    {
        private Country? Country { get; set; }

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "") =>
            Country = await ApiDriver.CreateSingleCountryAsync(countryCode, countryName);

        public async Task Given_I_want_to_retrieve_my_country()
        {
            Country country = await Assert.That(Country).IsNotNull();

            Request = ApiDriver.RequestFactory.Countries.GetCountry(country.Id);
        }

        public async Task Then_the_retrieved_country_should_be_my_country()
        {
            GetCountryResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            Country expectedCountry = await Assert.That(Country).IsNotNull();

            Country actualCountry = responseBody.Country;

            await Assert.That(actualCountry).IsEqualTo(expectedCountry, new CountryEqualityComparer());
        }
    }
}

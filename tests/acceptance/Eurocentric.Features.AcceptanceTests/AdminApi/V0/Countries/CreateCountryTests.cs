using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils.DataSourceGenerators;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Countries.CreateCountry;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Countries;

public sealed class CreateCountryTests : SerialCleanAcceptanceTest
{
    [Test]
    [AdminApiV0Point1AndUp]
    public async Task Endpoint_should_create_country(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(
            countryCode: "GB",
            countryName: "United Kingdom",
            countryType: "Real");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_created_country_should_match(
            countryCode: "GB",
            countryName: "United Kingdom");
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<CreateCountryResponse>(apiDriver)
    {
        public void Given_I_want_to_create_a_country(string countryName = "", string countryCode = "", string countryType = "")
        {
            CreateCountryRequest requestBody = new()
            {
                CountryCode = countryCode, CountryName = countryName, CountryType = Enum.Parse<CountryType>(countryType)
            };

            Request = ApiDriver.RequestFactory.Countries.CreateCountry(requestBody);
        }

        public async Task Then_the_created_country_should_match(string countryName = "", string countryCode = "")
        {
            CreateCountryResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            Country createdCountry = responseBody.Country;

            await Assert.That(createdCountry).HasMember(country => country.CountryCode).EqualTo(countryCode)
                .And.HasMember(country => country.CountryName).EqualTo(countryName);
        }
    }
}

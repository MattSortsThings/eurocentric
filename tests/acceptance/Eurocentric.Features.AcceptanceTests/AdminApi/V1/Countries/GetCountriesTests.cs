using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountries;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class GetCountriesTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_all_existing_countries_in_country_code_order(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_created_a_country(countryCode: "AT", countryName: "Austria");
        await admin.Given_I_have_created_a_country(countryCode: "XX", countryName: "Rest of the World");
        await admin.Given_I_have_created_a_country(countryCode: "BA", countryName: "Bosnia & Herzegovina");
        await admin.Given_I_have_created_a_country(countryCode: "AU", countryName: "Australia");

        admin.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_countries_should_be_my_countries_in_country_code_order();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_empty_list_when_no_countries_exist(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_countries_should_be_an_empty_list();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<GetCountriesResponse>(apiDriver)
    {
        private List<Country> Countries { get; } = [];

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "")
        {
            Country myCountry = await ApiDriver.CreateSingleCountryAsync(countryCode: countryCode, countryName: countryName);
            Countries.Add(myCountry);
        }

        public void Given_I_want_to_retrieve_all_existing_countries() =>
            Request = ApiDriver.RequestFactory.Countries.GetCountries();

        public async Task Then_the_retrieved_countries_should_be_my_countries_in_country_code_order()
        {
            IOrderedEnumerable<Country> expectedCountries = Countries.OrderBy(country => country.CountryCode);

            GetCountriesResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.Countries)
                .IsEquivalentTo(expectedCountries, new CountryEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_countries_should_be_an_empty_list()
        {
            GetCountriesResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.Countries).IsEmpty();
        }
    }
}

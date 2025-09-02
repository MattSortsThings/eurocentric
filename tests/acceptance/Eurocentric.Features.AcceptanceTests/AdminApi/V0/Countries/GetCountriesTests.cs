using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils.DataSourceGenerators;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils.Helpers.Countries;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Countries.GetCountries;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Countries;

public sealed class GetCountriesTests : CleanSerialAcceptanceTest
{
    [Test]
    [AdminApiV0Point2AndUp]
    public async Task Endpoint_should_retrieve_all_existing_countries_in_country_code_order(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "AU", countryName: "Australia");
        await admin.Given_I_have_created_a_country(countryCode: "FR", countryName: "France");
        await admin.Given_I_have_created_a_country(countryCode: "AT", countryName: "Austria");
        await admin.Given_I_have_created_a_country(countryCode: "BA", countryName: "Bosnia & Herzegovina");

        admin.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_countries_should_be_my_countries_in_country_code_order();
    }

    [Test]
    [AdminApiV0Point2AndUp]
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
            Country country = await ApiDriver.CreateSingleCountryAsync(countryCode, countryName);
            Countries.Add(country);
        }

        public void Given_I_want_to_retrieve_all_existing_countries() =>
            Request = ApiDriver.RequestFactory.Countries.GetCountries();

        public async Task Then_the_retrieved_countries_should_be_my_countries_in_country_code_order()
        {
            GetCountriesResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            IOrderedEnumerable<Country> expectedCountries = Countries.OrderBy(country => country.CountryCode);

            IOrderedEnumerable<Country> actualCountries = responseBody.Countries.OrderBy(country => country.CountryCode);

            await Assert.That(actualCountries).IsEquivalentTo(expectedCountries,
                new CountryEqualityComparer(),
                CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_countries_should_be_an_empty_list()
        {
            GetCountriesResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.Countries).IsEmpty();
        }
    }
}

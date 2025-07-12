using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public static class GetCountriesTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_retrieve_all_existing_countries_in_country_code_order(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
            await admin.Given_I_have_created_a_country(countryCode: "XX", countryName: "Rest of the World");
            await admin.Given_I_have_created_a_country(countryCode: "CZ", countryName: "Czechia");
            await admin.Given_I_have_created_a_country(countryCode: "AT", countryName: "Austria");
            await admin.Given_I_have_created_a_country(countryCode: "CH", countryName: "Switzerland");
            admin.Given_I_want_to_retrieve_all_existing_countries();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_countries_should_be_my_countries_in_country_code_order();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_retrieve_empty_list_when_no_countries_exist(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            admin.Given_I_want_to_retrieve_all_existing_countries();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_countries_should_be_an_empty_list();
        }
    }

    private sealed class Admin : AdminActor<GetCountriesResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_all_existing_countries() => Request = RequestFactory.Countries.GetCountries();

        public void Then_the_retrieved_countries_should_be_my_countries_in_country_code_order()
        {
            Assert.NotNull(ResponseObject);

            IOrderedEnumerable<Country> expectedCountries = GivenCountries.OrderBy(country => country.CountryCode);

            Assert.Equal(expectedCountries, ResponseObject.Countries, CountryEquality.Compare);
        }

        public void Then_the_retrieved_countries_should_be_an_empty_list()
        {
            Assert.NotNull(ResponseObject);

            Assert.Empty(ResponseObject.Countries);
        }
    }
}

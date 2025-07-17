using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Responses;
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
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
            await admin.Given_I_have_created_a_country(countryCode: "AT", countryName: "Austria");
            await admin.Given_I_have_created_a_country(countryCode: "CZ", countryName: "Czechia");
            await admin.Given_I_have_created_a_country(countryCode: "BA", countryName: "Bosnia & Herzegovina");
            await admin.Given_I_have_created_a_country(countryCode: "AU", countryName: "Australia");
            admin.Given_I_want_to_retrieve_all_existing_countries();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_countries_should_be_all_my_countries_in_country_code_order();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_retrieve_empty_list_when_no_countries_exist(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            admin.Given_I_want_to_retrieve_all_existing_countries();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_countries_should_be_an_empty_list();
        }
    }

    private sealed class Admin : AdminActorWithResponse<GetCountriesResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, IRequestFactory requestFactory) :
            base(restClient, backDoor, requestFactory)
        {
        }

        public void Given_I_want_to_retrieve_all_existing_countries() => Request = RequestFactory.Countries.GetCountries();

        public void Then_the_retrieved_countries_should_be_all_my_countries_in_country_code_order()
        {
            Assert.NotNull(ResponseObject);

            IOrderedEnumerable<Country> expectedCountries = GivenCountries.GetAll()
                .OrderBy(country => country.CountryCode);

            Country[] retrievedCountries = ResponseObject.Countries;

            Assert.Equal(expectedCountries, retrievedCountries, new CountryEqualityComparer());
        }

        public void Then_the_retrieved_countries_should_be_an_empty_list()
        {
            Assert.NotNull(ResponseObject);

            Assert.Empty(ResponseObject.Countries);
        }
    }
}

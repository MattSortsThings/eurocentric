using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Countries;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public static class CreateCountryTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_create_and_return_country_scenario_1(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            admin.Given_I_want_to_create_a_country(countryCode: "AT", countryName: "Austria");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(201);
            admin.Then_the_created_country_should_match(countryCode: "AT", countryName: "Austria");
            await admin.Then_the_created_country_should_be_retrievable_by_its_ID();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_create_and_return_country_scenario_2(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            admin.Given_I_want_to_create_a_country(countryCode: "BA", countryName: "Bosnia & Herzegovina");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(201);
            admin.Then_the_created_country_should_match(countryCode: "BA", countryName: "Bosnia & Herzegovina");
            await admin.Then_the_created_country_should_be_retrievable_by_its_ID();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_create_and_return_country_scenario_3(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            admin.Given_I_want_to_create_a_country(countryCode: "XX", countryName: "Rest of the World");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(201);
            admin.Then_the_created_country_should_match(countryCode: "XX", countryName: "Rest of the World");
            await admin.Then_the_created_country_should_be_retrievable_by_its_ID();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_country_code_conflict(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
            admin.Given_I_want_to_create_a_country_with_country_code("GB");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(409);
            admin.Then_the_response_problem_details_should_match(status: 409,
                title: "Country code conflict",
                detail: "A country already exists with the provided country code.");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "countryCode", value: "GB");
            await admin.Then_my_given_country_should_be_the_only_existing_country();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_illegal_country_code_value(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            admin.Given_I_want_to_create_a_country_with_country_code("0");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal country code value",
                detail: "Country code value must be a string of 2 upper-case letters.");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "countryCode", value: "0");
            await admin.Then_no_countries_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_illegal_country_name_value(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            admin.Given_I_want_to_create_a_country_with_country_name(" ");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal country name value",
                detail: "Country name value must be a non-empty, non-whitespace string of no more than 200 characters.");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "countryName", value: " ");
            await admin.Then_no_countries_should_exist();
        }
    }

    private sealed class Admin : AdminActor<CreateCountryResponse>
    {
        private const string DefaultCountryCode = "AA";
        private const string DefaultCountryName = "CountryName";

        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
        {
        }

        public void Given_I_want_to_create_a_country(string countryName = "", string countryCode = "")
        {
            CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryName };

            Request = RequestFactory.Countries.CreateCountry(requestBody);
        }

        public void Given_I_want_to_create_a_country_with_country_code(string countryCode)
        {
            CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = DefaultCountryName };

            Request = RequestFactory.Countries.CreateCountry(requestBody);
        }

        public void Given_I_want_to_create_a_country_with_country_name(string countryName)
        {
            CreateCountryRequest requestBody = new() { CountryCode = DefaultCountryCode, CountryName = countryName };

            Request = RequestFactory.Countries.CreateCountry(requestBody);
        }

        public void Then_the_created_country_should_match(string countryName = "", string countryCode = "")
        {
            Assert.NotNull(ResponseObject);

            Country createdCountry = ResponseObject.Country;

            Assert.Equal(countryName, createdCountry.CountryName);
            Assert.Equal(countryCode, createdCountry.CountryCode);
        }

        public async Task Then_the_created_country_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(ResponseObject);

            Country createdCountry = ResponseObject.Country;
            Country retrievedCountry = await GetCountryByIdAsync(createdCountry.Id);

            Assert.Equal(createdCountry, retrievedCountry, CountryEquality.Compare);
        }

        public async Task Then_my_given_country_should_be_the_only_existing_country()
        {
            Country[] existingCountries = await GetAllCountriesAsync();

            Assert.Equal(GivenCountries, existingCountries, CountryEquality.Compare);
        }

        public async Task Then_no_countries_should_exist()
        {
            Country[] existingCountries = await GetAllCountriesAsync();

            Assert.Empty(existingCountries);
        }

        private async Task<Country> GetCountryByIdAsync(Guid countryId)
        {
            RestRequest request = RequestFactory.Countries.GetCountry(countryId);

            ProblemOrResponse<GetCountryResponse> problemOrResponse =
                await RestClient.SendAsync<GetCountryResponse>(request, TestContext.Current.CancellationToken);

            return problemOrResponse.AsResponse.Data!.Country;
        }

        private async Task<Country[]> GetAllCountriesAsync()
        {
            RestRequest request = RequestFactory.Countries.GetCountries();

            ProblemOrResponse<GetCountriesResponse> problemOrResponse =
                await RestClient.SendAsync<GetCountriesResponse>(request, TestContext.Current.CancellationToken);

            return problemOrResponse.AsResponse.Data!.Countries;
        }
    }
}

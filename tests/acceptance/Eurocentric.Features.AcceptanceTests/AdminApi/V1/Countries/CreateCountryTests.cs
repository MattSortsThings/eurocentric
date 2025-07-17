using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Responses;
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
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

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
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

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
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

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
        public async Task Should_fail_on_country_with_non_unique_country_code(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

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
            admin.Then_the_response_problem_details_should_have_a_countryCode_extension_with("GB");
            await admin.Then_my_given_country_should_be_the_only_existing_country();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_illegal_country_code_value(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            admin.Given_I_want_to_create_a_country_with_country_code("999999");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal country code value",
                detail: "Country code value must be a string of 2 upper-case letters.");
            admin.Then_the_response_problem_details_should_have_a_countryCode_extension_with("999999");
            await admin.Then_there_should_be_no_existing_countries();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_illegal_country_name_value(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            admin.Given_I_want_to_create_a_country_with_country_name(" ");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal country name value",
                detail: "Country name value must be a non-empty, non-whitespace string of no more than 200 characters.");
            admin.Then_the_response_problem_details_should_have_a_countryName_extension_with(" ");
            await admin.Then_there_should_be_no_existing_countries();
        }
    }

    private sealed class Admin : AdminActorWithResponse<CreateCountryResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, IRequestFactory requestFactory) :
            base(restClient, backDoor, requestFactory)
        {
        }

        public void Given_I_want_to_create_a_country(string countryName = "", string countryCode = "")
        {
            CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryName };

            Request = RequestFactory.Countries.CreateCountry(requestBody);
        }

        public void Given_I_want_to_create_a_country_with_country_code(string countryCode)
        {
            CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = DefaultValues.CountryName };

            Request = RequestFactory.Countries.CreateCountry(requestBody);
        }

        public void Given_I_want_to_create_a_country_with_country_name(string countryName)
        {
            CreateCountryRequest requestBody = new() { CountryCode = DefaultValues.CountryCode, CountryName = countryName };

            Request = RequestFactory.Countries.CreateCountry(requestBody);
        }

        public void Then_the_created_country_should_match(string countryName = "", string countryCode = "")
        {
            Assert.NotNull(ResponseObject);

            Country createdCountry = ResponseObject.Country;

            Assert.Equal(countryCode, createdCountry.CountryCode);
            Assert.Equal(countryName, createdCountry.CountryName);
        }

        public void Then_the_response_problem_details_should_have_a_countryCode_extension_with(string countryCode)
        {
            Assert.NotNull(ResponseProblemDetails);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "countryCode", Value: JsonElement je }
                                                                      && je.GetString() == countryCode);
        }

        public void Then_the_response_problem_details_should_have_a_countryName_extension_with(string countryName)
        {
            Assert.NotNull(ResponseProblemDetails);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "countryName", Value: JsonElement je }
                                                                      && je.GetString() == countryName);
        }

        public async Task Then_the_created_country_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(ResponseObject);

            Country createdCountry = ResponseObject.Country;
            Country retrievedCountry = await GetExistingCountryByIdAsync(createdCountry.Id);

            Assert.Equal(createdCountry, retrievedCountry, new CountryEqualityComparer());
        }

        public async Task Then_my_given_country_should_be_the_only_existing_country()
        {
            Country expectedCountry = GivenCountries.GetSingle();
            Country[] existingCountries = await GetAllExistingCountriesAsync();

            Country actualCountry = Assert.Single(existingCountries);

            Assert.Equal(expectedCountry, actualCountry, new CountryEqualityComparer());
        }

        public async Task Then_there_should_be_no_existing_countries()
        {
            Country[] existingCountries = await GetAllExistingCountriesAsync();

            Assert.Empty(existingCountries);
        }

        private async Task<Country> GetExistingCountryByIdAsync(Guid countryId)
        {
            RestRequest request = RequestFactory.Countries.GetCountry(countryId);

            ProblemOrResponse<GetCountryResponse> problemOrResponse =
                await RestClient.SendAsync<GetCountryResponse>(request, TestContext.Current.CancellationToken);

            return problemOrResponse.AsResponse.Data!.Country;
        }

        private async Task<Country[]> GetAllExistingCountriesAsync()
        {
            RestRequest request = RequestFactory.Countries.GetCountries();

            ProblemOrResponse<GetCountriesResponse> problemOrResponse =
                await RestClient.SendAsync<GetCountriesResponse>(request, TestContext.Current.CancellationToken);

            return problemOrResponse.AsResponse.Data!.Countries;
        }
    }
}

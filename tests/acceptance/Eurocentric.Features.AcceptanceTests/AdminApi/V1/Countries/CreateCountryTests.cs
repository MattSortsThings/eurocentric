using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.DataSourceGenerators;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.Helpers.Countries;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class CreateCountryTests : SerialCleanAcceptanceTest
{
    [Test]
    [AdminApiV1Point0AndUp]
    public async Task Endpoint_should_create_and_return_country_scenario_1(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: "AT", countryName: "Austria");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_created_country_should_match(countryCode: "AT", countryName: "Austria");
        await admin.Then_the_created_country_should_be_retrievable_by_its_ID();
    }

    [Test]
    [AdminApiV1Point0AndUp]
    public async Task Endpoint_should_create_and_return_country_scenario_2(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: "BA", countryName: "Bosnia & Herzegovina");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_created_country_should_match(countryCode: "BA", countryName: "Bosnia & Herzegovina");
        await admin.Then_the_created_country_should_be_retrievable_by_its_ID();
    }

    [Test]
    [AdminApiV1Point0AndUp]
    public async Task Endpoint_should_create_and_return_country_scenario_3(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: "XX", countryName: "Rest of the World");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_created_country_should_match(countryCode: "XX", countryName: "Rest of the World");
        await admin.Then_the_created_country_should_be_retrievable_by_its_ID();
    }

    [Test]
    [AdminApiV1Point0AndUp]
    public async Task Endpoint_should_fail_on_non_unique_country_code(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");

        admin.Given_I_want_to_create_a_country_with_country_code("GB");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Country code conflict",
            detail: "A country already exists with the provided country code.");
        await admin.Then_the_response_problem_details_extensions_should_include(key: "countryCode", value: "GB");
        await admin.Then_my_existing_country_should_be_the_only_country_in_the_system();
    }

    [Test]
    [AdminApiV1Point0AndUp]
    public async Task Endpoint_should_fail_on_illegal_country_code_value(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country_with_country_code("!");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal country code value",
            detail: "Country code value must be a string of 2 upper-case letters.");
        await admin.Then_the_response_problem_details_extensions_should_include(key: "countryCode", value: "!");
        await admin.Then_no_countries_should_exist_in_the_system();
    }

    [Test]
    [AdminApiV1Point0AndUp]
    public async Task Endpoint_should_fail_on_illegal_country_name_value(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country_with_country_name("  ");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal country name value",
            detail: "Country name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        await admin.Then_the_response_problem_details_extensions_should_include(key: "countryName", value: "  ");
        await admin.Then_no_countries_should_exist_in_the_system();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<CreateCountryResponse>(apiDriver)
    {
        private Country? ExistingCountry { get; set; }

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "")
        {
            Country country = await ApiDriver.CreateSingleCountryAsync(countryCode: countryCode, countryName: countryName);

            ExistingCountry = country;
        }

        public void Given_I_want_to_create_a_country(string countryName = "", string countryCode = "")
        {
            CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryName };

            Request = ApiDriver.RequestFactory.Countries.CreateCountry(requestBody);
        }

        public void Given_I_want_to_create_a_country_with_country_code(string countryCode)
        {
            CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = TestDefaults.CountryName };

            Request = ApiDriver.RequestFactory.Countries.CreateCountry(requestBody);
        }

        public void Given_I_want_to_create_a_country_with_country_name(string countryName)
        {
            CreateCountryRequest requestBody = new() { CountryCode = TestDefaults.CountryCode, CountryName = countryName };

            Request = ApiDriver.RequestFactory.Countries.CreateCountry(requestBody);
        }

        public async Task Then_the_created_country_should_match(string countryName = "", string countryCode = "")
        {
            CreateCountryResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.Country)
                .HasMember(country => country.CountryCode).EqualTo(countryCode)
                .And.HasMember(country => country.CountryName).EqualTo(countryName);
        }

        public async Task Then_the_created_country_should_be_retrievable_by_its_ID()
        {
            CreateCountryResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            Country createdCountry = responseBody.Country;

            Country retrievedCountry = await ApiDriver.GetSingleCountryAsync(createdCountry.Id);

            await Assert.That(retrievedCountry).IsEqualTo(createdCountry, new CountryEqualityComparer());
        }

        public async Task Then_my_existing_country_should_be_the_only_country_in_the_system()
        {
            Country existingCountry = await Assert.That(ExistingCountry).IsNotNull();

            Country[] retrievedCountries = await ApiDriver.GetAllCountriesAsync();

            Country? singleRetrievedCountry = await Assert.That(retrievedCountries).HasSingleItem();

            await Assert.That(singleRetrievedCountry).IsEqualTo(existingCountry, new CountryEqualityComparer());
        }

        public async Task Then_no_countries_should_exist_in_the_system()
        {
            Country[] retrievedCountries = await ApiDriver.GetAllCountriesAsync();

            await Assert.That(retrievedCountries).IsEmpty();
        }
    }
}

using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class CreateCountryTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_country_scenario_1(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: "AT", countryName: "Austria");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_country_should_match(countryCode: "AT",
            countryName: "Austria",
            participatingContests: 0);
        await admin.Then_the_created_country_should_be_retrievable_by_its_ID();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_country_scenario_2(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: "BA", countryName: "Bosnia & Herzegovina");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_country_should_match(countryCode: "BA",
            countryName: "Bosnia & Herzegovina",
            participatingContests: 0);
        await admin.Then_the_created_country_should_be_retrievable_by_its_ID();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_country_scenario_3(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: "XX", countryName: "Rest of the World");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_country_should_match(countryCode: "XX",
            countryName: "Rest of the World",
            participatingContests: 0);
        await admin.Then_the_created_country_should_be_retrievable_by_its_ID();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_country_with_non_unique_country_code(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country_with_country_code("GB");
        admin.Given_I_want_to_create_a_country_with_country_code("GB");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Country code conflict",
            detail: "A country already exists with the provided country code.");
        admin.Then_the_problem_details_extensions_should_contain("countryCode", "GB");
        await admin.Then_my_existing_country_should_be_the_only_existing_country();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_country_with_illegal_country_code_value(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country_with_country_code("0");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal country code value",
            detail: "Country code value must be a string of 2 upper-case letters.");
        admin.Then_the_problem_details_extensions_should_contain("countryCode", "0");
        await admin.Then_there_should_be_no_existing_countries();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_country_with_illegal_country_name_value(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country_with_country_name(" ");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal country name value",
            detail: "Country name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("countryName", " ");
        await admin.Then_there_should_be_no_existing_countries();
    }

    private sealed class AdminActor : ActorWithResponse<CreateCountryResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        private Country? MyExistingCountry { get; set; }

        public async Task Given_I_have_created_a_country_with_country_code(string countryCode) =>
            MyExistingCountry = await ApiDriver.Countries.CreateACountryAsync(countryCode: countryCode,
                cancellationToken: TestContext.Current.CancellationToken);

        public void Given_I_want_to_create_a_country(string countryName = "CountryName", string countryCode = "AA")
        {
            CreateCountryRequest request = new() { CountryCode = countryCode, CountryName = countryName };

            SendMyRequest = apiDriver => apiDriver.Countries.CreateCountry(request, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_country_with_country_code(string countryCode)
        {
            CreateCountryRequest request = new() { CountryCode = countryCode, CountryName = "CountryName" };

            SendMyRequest = apiDriver => apiDriver.Countries.CreateCountry(request, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_country_with_country_name(string countryName)
        {
            CreateCountryRequest request = new() { CountryCode = "AA", CountryName = countryName };

            SendMyRequest = apiDriver => apiDriver.Countries.CreateCountry(request, TestContext.Current.CancellationToken);
        }

        public void Then_the_created_country_should_match(string countryName = "", string countryCode = "",
            int participatingContests = 0)
        {
            Assert.NotNull(ResponseObject);

            Country createdCountry = ResponseObject.Country;

            Assert.Equal(countryName, createdCountry.CountryName);
            Assert.Equal(countryCode, createdCountry.CountryCode);
            Assert.Equal(participatingContests, createdCountry.ParticipatingContests.Length);
        }

        public async Task Then_the_created_country_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(ResponseObject);

            Country createdCountry = ResponseObject.Country;

            Country retrievedCountry =
                await ApiDriver.Countries.GetACountryAsync(createdCountry.Id, TestContext.Current.CancellationToken);

            Assert.Equal(createdCountry, retrievedCountry, new CountryEqualityComparer());
        }

        public async Task Then_there_should_be_no_existing_countries()
        {
            Country[] existingCountries = await ApiDriver.Countries.GetAllCountriesAsync(TestContext.Current.CancellationToken);

            Assert.Empty(existingCountries);
        }

        public async Task Then_my_existing_country_should_be_the_only_existing_country()
        {
            Assert.NotNull(MyExistingCountry);

            Country[] existingCountries = await ApiDriver.Countries.GetAllCountriesAsync(TestContext.Current.CancellationToken);

            Assert.Single(existingCountries);
            Assert.Contains(MyExistingCountry, existingCountries, new CountryEqualityComparer());
        }
    }
}

using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Features.Countries;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Countries;

[Category("admin-api")]
public sealed class CreateCountryTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_create_and_return_country_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: "AT", countryName: "Austria");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_response_headers_should_include_the_created_country_location_for_API_version(apiVersion);
        await admin.Then_the_created_country_should_match(countryCode: "AT", countryName: "Austria");
        await admin.Then_the_created_country_should_have_no_contest_roles();
        await admin.Then_the_created_country_should_be_the_only_existing_country();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_create_and_return_country_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: "BA", countryName: "Bosnia & Herzegovina");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_response_headers_should_include_the_created_country_location_for_API_version(apiVersion);
        await admin.Then_the_created_country_should_match(countryCode: "BA", countryName: "Bosnia & Herzegovina");
        await admin.Then_the_created_country_should_have_no_contest_roles();
        await admin.Then_the_created_country_should_be_the_only_existing_country();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_create_and_return_country_scenario_3(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: "XX", countryName: "Rest of the World");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_response_headers_should_include_the_created_country_location_for_API_version(apiVersion);
        await admin.Then_the_created_country_should_match(countryCode: "XX", countryName: "Rest of the World");
        await admin.Then_the_created_country_should_have_no_contest_roles();
        await admin.Then_the_created_country_should_be_the_only_existing_country();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_illegal_country_code_value(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country_with_country_code("!");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal country code value",
            detail: "Country code value must be a string of 2 upper-case letters."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(key: "countryCode", value: "!");
        await admin.Then_there_should_be_no_existing_countries();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_illegal_country_name_value(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country_with_country_name(" ");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal country name value",
            detail: "Country name value must be a non-empty, non-whitespace string of no more than 200 characters."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(key: "countryName", value: " ");
        await admin.Then_there_should_be_no_existing_countries();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_country_code_conflict(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");

        await admin.Given_I_want_to_create_a_country_with_the_same_country_code_as_my_existing_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Country code conflict",
            detail: "A country already exists with the provided country code."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(key: "countryCode", value: "GB");
        await admin.Then_my_existing_country_should_be_the_only_existing_country();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<CreateCountryResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private Country? ExistingCountry { get; set; }

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "") =>
            ExistingCountry = await Kernel.CreateACountryAsync(countryCode: countryCode, countryName: countryName);

        public void Given_I_want_to_create_a_country(string countryName = "", string countryCode = "")
        {
            CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryName };

            Request = Kernel.Requests.Countries.CreateCountry(requestBody);
        }

        public void Given_I_want_to_create_a_country_with_country_name(string countryName)
        {
            CreateCountryRequest requestBody = new() { CountryCode = "AA", CountryName = countryName };

            Request = Kernel.Requests.Countries.CreateCountry(requestBody);
        }

        public void Given_I_want_to_create_a_country_with_country_code(string countryCode)
        {
            CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = "CountryName" };

            Request = Kernel.Requests.Countries.CreateCountry(requestBody);
        }

        public async Task Given_I_want_to_create_a_country_with_the_same_country_code_as_my_existing_country()
        {
            string existingCountryCode = await Assert.That(ExistingCountry?.CountryCode).IsNotNull();

            CreateCountryRequest requestBody = new() { CountryCode = existingCountryCode, CountryName = "CountryName" };

            Request = Kernel.Requests.Countries.CreateCountry(requestBody);
        }

        public async Task Then_the_response_headers_should_include_the_created_country_location_for_API_version(
            string apiVersion
        )
        {
            Guid createdCountryId = await Assert.That(SuccessResponse?.Data?.Country.Id).IsNotNull();

            string expectedLocationSuffix = $"/admin/api/{apiVersion}/countries/{createdCountryId}";

            await Assert
                .That(SuccessResponse?.Headers)
                .Contains(headerParameter =>
                    headerParameter.Name == "Location" && headerParameter.Value.EndsWith(expectedLocationSuffix)
                );
        }

        public async Task Then_the_created_country_should_match(string countryName = "", string countryCode = "")
        {
            await Assert
                .That(SuccessResponse?.Data?.Country)
                .HasProperty(country => country.CountryCode, countryCode)
                .And.HasProperty(country => country.CountryName, countryName);
        }

        public async Task Then_the_created_country_should_have_no_contest_roles()
        {
            Country createdCountry = await Assert.That(SuccessResponse?.Data?.Country).IsNotNull();

            await Assert.That(createdCountry.ContestRoles).IsEmpty();
        }

        public async Task Then_there_should_be_no_existing_countries()
        {
            Country[] existingCountries = await Kernel.GetAllCountriesAsync();

            await Assert.That(existingCountries).IsEmpty();
        }

        public async Task Then_the_created_country_should_be_the_only_existing_country()
        {
            Country createdCountry = await Assert.That(SuccessResponse?.Data?.Country).IsNotNull();

            Country[] existingCountries = await Kernel.GetAllCountriesAsync();

            await Assert
                .That(existingCountries)
                .HasSingleItem()
                .And.Contains(createdCountry, new CountryEqualityComparer());
        }

        public async Task Then_my_existing_country_should_be_the_only_existing_country()
        {
            Country existingCountry = await Assert.That(ExistingCountry).IsNotNull();

            Country[] existingCountries = await Kernel.GetAllCountriesAsync();

            await Assert
                .That(existingCountries)
                .HasSingleItem()
                .And.Contains(existingCountry, new CountryEqualityComparer());
        }
    }
}

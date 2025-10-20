using Eurocentric.AcceptanceTests.Functional.AdminApi.V0.Countries.TestUtils;
using Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils;
using Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils.Attributes;
using Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils.Extensions;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V0.Dtos.Countries;
using Eurocentric.Apis.Admin.V0.Enums;
using Eurocentric.Apis.Admin.V0.Features.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.Countries;

[Category("admin-api")]
public sealed class CreateCountryTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_create_and_return_country_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: "AT", countryName: "Austria");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_created_country_should_match(countryCode: "AT", countryName: "Austria");
        await admin.Then_the_created_country_should_have_no_contest_roles();
        await admin.Then_the_created_country_should_be_retrievable_from_its_location();
    }

    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_create_and_return_country_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: "BA", countryName: "Bosnia & Herzegovina");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_created_country_should_match(countryCode: "BA", countryName: "Bosnia & Herzegovina");
        await admin.Then_the_created_country_should_have_no_contest_roles();
        await admin.Then_the_created_country_should_be_retrievable_from_its_location();
    }

    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_create_and_return_country_scenario_3(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: "XX", countryName: "Rest of the World");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_created_country_should_match(countryCode: "XX", countryName: "Rest of the World");
        await admin.Then_the_created_country_should_have_no_contest_roles();
        await admin.Then_the_created_country_should_be_retrievable_from_its_location();
    }

    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_fail_on_illegal_country_code_value(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country_with_country_code("!");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(500);
        await admin.Then_no_countries_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_fail_on_illegal_country_name_value(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_create_a_country_with_country_name(" ");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(500);
        await admin.Then_no_countries_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_fail_on_country_code_conflict(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");

        await admin.Given_I_want_to_create_a_country_with_the_same_country_code_as_my_existing_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(500);
        await admin.Then_my_existing_country_should_be_the_only_existing_country_in_the_system();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<CreateCountryResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private Country? ExistingCountry { get; set; }

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "")
        {
            Country createdCountry = await Kernel.CreateACountryAsync(
                countryCode: countryCode,
                countryName: countryName,
                cancellationToken: TestContext.Current!.CancellationToken
            );

            ExistingCountry = createdCountry;
        }

        public void Given_I_want_to_create_a_country(string countryName = "", string countryCode = "")
        {
            CreateCountryRequest requestBody = new()
            {
                CountryType = CountryType.Real,
                CountryCode = countryCode,
                CountryName = countryName,
            };

            Request = Kernel.Requests.Countries.CreateCountry(requestBody);
        }

        public void Given_I_want_to_create_a_country_with_country_name(string countryName)
        {
            CreateCountryRequest requestBody = new()
            {
                CountryType = CountryType.Real,
                CountryCode = "AA",
                CountryName = countryName,
            };

            Request = Kernel.Requests.Countries.CreateCountry(requestBody);
        }

        public void Given_I_want_to_create_a_country_with_country_code(string countryCode)
        {
            CreateCountryRequest requestBody = new()
            {
                CountryType = CountryType.Real,
                CountryCode = countryCode,
                CountryName = "CountryName",
            };

            Request = Kernel.Requests.Countries.CreateCountry(requestBody);
        }

        public async Task Given_I_want_to_create_a_country_with_the_same_country_code_as_my_existing_country()
        {
            string existingCountryCode = await Assert.That(ExistingCountry?.CountryCode).IsNotNull();

            CreateCountryRequest requestBody = new()
            {
                CountryType = CountryType.Real,
                CountryCode = existingCountryCode,
                CountryName = "CountryName",
            };

            Request = Kernel.Requests.Countries.CreateCountry(requestBody);
        }

        public async Task Then_the_created_country_should_match(string countryName = "", string countryCode = "")
        {
            Country createdCountry = await Assert.That(SuccessResponse?.Data?.Country).IsNotNull();

            await Assert.That(createdCountry.CountryCode).IsEqualTo(countryCode);
            await Assert.That(createdCountry.CountryName).IsEqualTo(countryName);
        }

        public async Task Then_the_created_country_should_have_no_contest_roles()
        {
            Country createdCountry = await Assert.That(SuccessResponse?.Data?.Country).IsNotNull();

            await Assert.That(createdCountry.ContestRoles).IsEmpty();
        }

        public async Task Then_the_created_country_should_be_retrievable_from_its_location()
        {
            Country createdCountry = await Assert.That(SuccessResponse?.Data?.Country).IsNotNull();

            string location = await Assert.That(SuccessResponse?.Headers?.ExtractLocation()).IsNotNull();

            Country retrievedCountry = await RetrieveCountryAsync(location, TestContext.Current!.CancellationToken);

            await Assert.That(retrievedCountry).IsEqualTo(createdCountry, new CountryEqualityComparer());
        }

        public async Task Then_no_countries_should_exist_in_the_system()
        {
            Country[] existingCountries = await Kernel.GetAllCountriesAsync(TestContext.Current!.CancellationToken);

            await Assert.That(existingCountries).IsEmpty();
        }

        public async Task Then_my_existing_country_should_be_the_only_existing_country_in_the_system()
        {
            Country expectedExistingCountry = await Assert.That(ExistingCountry).IsNotNull();

            Country[] existingCountries = await Kernel.GetAllCountriesAsync(TestContext.Current!.CancellationToken);

            Country actualExistingCountry = await Assert.That(existingCountries).HasSingleItem();

            await Assert.That(actualExistingCountry).IsEqualTo(expectedExistingCountry, new CountryEqualityComparer());
        }

        private async Task<Country> RetrieveCountryAsync(string location, CancellationToken cancellationToken)
        {
            RestRequest getCountryRequest = MapToGetRequest(location);

            ProblemOrResponse<GetCountryResponse> getCountryResponse =
                await Kernel.Client.SendAsync<GetCountryResponse>(getCountryRequest, cancellationToken);

            Country retrievedCountry = await Assert.That(getCountryResponse.AsResponse.Data?.Country).IsNotNull();

            return retrievedCountry;
        }

        private static RestRequest MapToGetRequest(string location)
        {
            string route = location.Replace("http://localhost/", "/");

            return new RestRequest(route);
        }
    }
}

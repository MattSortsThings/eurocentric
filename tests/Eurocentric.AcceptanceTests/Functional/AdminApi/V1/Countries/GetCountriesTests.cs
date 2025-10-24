using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Countries.TestUtils;
using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Features.Countries;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Countries;

[Category("admin-api")]
public sealed class GetCountriesTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_all_existing_countries_in_country_code_order(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_created_a_country(countryCode: "AU", countryName: "Australia");
        await admin.Given_I_have_created_a_country(countryCode: "XX", countryName: "Rest of the World");
        await admin.Given_I_have_created_a_country(countryCode: "AT", countryName: "Austria");
        await admin.Given_I_have_created_a_country(countryCode: "CZ", countryName: "Czechia");

        admin.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_countries_should_be_my_countries_in_country_code_order();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_list_when_no_countries_exist(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_countries_should_be_an_empty_list();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetCountriesResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private List<Country> ExistingCountries { get; } = [];

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "")
        {
            Country country = await Kernel.CreateACountryAsync(countryCode: countryCode, countryName: countryName);

            ExistingCountries.Add(country);
        }

        public void Given_I_want_to_retrieve_all_existing_countries() =>
            Request = Kernel.Requests.Countries.GetCountries();

        public async Task Then_the_retrieved_countries_should_be_my_countries_in_country_code_order()
        {
            IOrderedEnumerable<Country> expectedCountries = ExistingCountries.OrderBy(country => country.CountryCode);

            await Assert
                .That(SuccessResponse?.Data?.Countries)
                .IsNotNull()
                .And.IsEquivalentTo(expectedCountries, new CountryEqualityComparer())
                .And.IsOrderedBy(country => country.CountryCode);
        }

        public async Task Then_the_retrieved_countries_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Countries).IsNotNull().And.IsEmpty();
    }
}

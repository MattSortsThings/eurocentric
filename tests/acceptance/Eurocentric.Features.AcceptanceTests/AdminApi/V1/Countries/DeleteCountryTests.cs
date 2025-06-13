using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class DeleteCountryTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_delete_country_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        admin.Given_I_want_to_delete_my_country_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_there_should_be_no_existing_countries();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_delete_country_with_participating_contest_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_created_some_additional_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_contest_in_which_my_country_and_all_additional_countries_participate();
        admin.Given_I_want_to_delete_my_country_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Cannot delete country",
            detail: "A country may only be deleted if it participates in no contests.");
        admin.Then_the_problem_details_extensions_should_contain_my_country_ID_with_key("countryId");
        await admin.Then_my_country_should_be_retrievable_by_its_ID();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_delete_non_existent_country_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_deleted_my_country();
        admin.Given_I_want_to_delete_my_country_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_404_NotFound();
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Country not found",
            detail: "No country exists with the provided country ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_country_ID_with_key("countryId");
    }

    private sealed class AdminActor : ActorWithoutResponse
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        private Country? MyCountry { get; set; }

        private List<Country> MyAdditionalCountries { get; } = [];

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "") =>
            MyCountry = await ApiDriver.Countries.CreateACountryAsync(countryCode: countryCode,
                countryName: countryName,
                cancellationToken: TestContext.Current.CancellationToken);

        public async Task Given_I_have_created_some_additional_countries(params string[] countryCodes)
        {
            Country[] countries =
                await ApiDriver.Countries.CreateMultipleCountriesAsync(countryCodes, TestContext.Current.CancellationToken);

            MyAdditionalCountries.AddRange(countries);
        }

        public async Task Given_I_have_deleted_my_country()
        {
            Assert.NotNull(MyCountry);

            await ApiDriver.Countries.DeleteACountryAsync(MyCountry.Id, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_delete_my_country_by_its_ID()
        {
            Assert.NotNull(MyCountry);

            Guid myCountryId = MyCountry.Id;

            SendMyRequest = apiDriver => apiDriver.Countries.DeleteCountry(myCountryId, TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_created_a_contest_in_which_my_country_and_all_additional_countries_participate()
        {
            Assert.NotNull(MyCountry);

            Guid[] countryIds = MyAdditionalCountries.Select(country => country.Id)
                .Prepend(MyCountry.Id)
                .ToArray();

            _ = await ApiDriver.Contests.CreateAContestAsync(contestFormat: ContestFormat.Stockholm,
                contestYear: 2025,
                cityName: "CityName",
                group0CountryId: null,
                group1CountryIds: countryIds.Take(3),
                group2CountryIds: countryIds.Skip(3),
                cancellationToken: TestContext.Current.CancellationToken);

            Country[] retrievedCountries = await ApiDriver.Countries.GetAllCountriesAsync(TestContext.Current.CancellationToken);

            Guid myCountryId = MyCountry.Id;
            MyCountry = null;
            MyAdditionalCountries.Clear();

            foreach (Country country in retrievedCountries)
            {
                if (country.Id == myCountryId)
                {
                    MyCountry = country;
                }
                else
                {
                    MyAdditionalCountries.Add(country);
                }
            }
        }

        public async Task Then_there_should_be_no_existing_countries()
        {
            Country[] existingCountries = await ApiDriver.Countries.GetAllCountriesAsync(TestContext.Current.CancellationToken);

            Assert.Empty(existingCountries);
        }

        public void Then_the_problem_details_extensions_should_contain_my_country_ID_with_key(string key)
        {
            Assert.NotNull(MyCountry);

            Then_the_problem_details_extensions_should_contain(key, MyCountry.Id);
        }

        public async Task Then_my_country_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(MyCountry);

            Country retrievedCountry =
                await ApiDriver.Countries.GetACountryAsync(MyCountry.Id, TestContext.Current.CancellationToken);

            Assert.Equal(MyCountry, retrievedCountry, new CountryEqualityComparer());
        }
    }
}

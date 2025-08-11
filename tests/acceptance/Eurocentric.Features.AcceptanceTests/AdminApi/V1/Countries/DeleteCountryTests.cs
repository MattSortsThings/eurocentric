using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class DeleteCountryTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_delete_requested_country(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");

        await admin.Given_I_want_to_delete_my_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_my_country_should_not_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_non_existent_country_requested(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_deleted_my_country();

        await admin.Given_I_want_to_delete_my_deleted_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_404_NotFound();
        await admin.Then_the_response_problem_details_should_match(status: 404,
            title: "Country not found",
            detail: "No country exists with the provided country ID.");
        await admin.Then_the_response_problem_details_extensions_should_include_my_deleted_country_ID();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_requested_country_participating_in_any_contest(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_created_some_additional_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_country_and_all_my_additional_countries();

        await admin.Given_I_want_to_delete_my_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Country deletion blocked",
            detail: "The country cannot be deleted because it participates in one or more contests.");
        await admin.Then_my_country_should_exist_in_the_system();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithoutResponse(apiDriver)
    {
        private CountryIdLookup AdditionalCountryIds { get; } = new();

        private Country? Country { get; set; }

        private Guid? DeletedCountryId { get; set; }

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "") =>
            Country = await ApiDriver.CreateSingleCountryAsync(countryCode: countryCode, countryName: countryName);

        public async Task Given_I_have_deleted_my_country()
        {
            Country myCountry = await Assert.That(Country).IsNotNull();
            Guid myCountryId = myCountry.Id;

            await ApiDriver.DeleteSingleCountryAsync(myCountryId);

            Country = null;
            DeletedCountryId = myCountryId;
        }

        public async Task Given_I_have_created_some_additional_countries(params string[] countryCodes)
        {
            List<Country> createdCountries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);
            AdditionalCountryIds.Populate(createdCountries);
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest_for_my_country_and_all_my_additional_countries()
        {
            Country myCountry = await Assert.That(Country).IsNotNull();

            Guid[] countryIds = AdditionalCountryIds.GetAll().Prepend(myCountry.Id).ToArray();

            _ = await ApiDriver.CreateSingleStockholmFormatContestAsync(contestYear: TestDefaults.ContestYear,
                cityName: TestDefaults.CityName,
                group1CountryIds: countryIds.Take(3).ToArray(),
                group2CountryIds: countryIds.Skip(3).ToArray());

            Country = await ApiDriver.GetSingleCountryAsync(myCountry.Id);
        }

        public async Task Given_I_want_to_delete_my_country()
        {
            Country myCountry = await Assert.That(Country).IsNotNull();

            Request = ApiDriver.RequestFactory.Countries.DeleteCountry(myCountry.Id);
        }

        public async Task Given_I_want_to_delete_my_deleted_country()
        {
            Guid myDeletedCountryId = await Assert.That(DeletedCountryId).IsNotNull();

            Request = ApiDriver.RequestFactory.Countries.DeleteCountry(myDeletedCountryId);
        }

        public async Task Then_my_country_should_not_exist_in_the_system()
        {
            Country myCountry = await Assert.That(Country).IsNotNull();

            Country[] allCountries = await ApiDriver.GetAllCountriesAsync();

            await Assert.That(allCountries)
                .DoesNotContain(country => new CountryEqualityComparer().Equals(country, myCountry));
        }

        public async Task Then_my_country_should_exist_in_the_system()
        {
            Country myCountry = await Assert.That(Country).IsNotNull();

            Country[] allCountries = await ApiDriver.GetAllCountriesAsync();

            await Assert.That(allCountries)
                .Contains(country => new CountryEqualityComparer().Equals(country, myCountry));
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_deleted_country_ID()
        {
            ProblemDetails problemDetails = await Assert.That(ResponseProblemDetails).IsNotNull();
            Guid myDeletedCountryId = await Assert.That(DeletedCountryId).IsNotNull();

            await Assert.That(problemDetails).HasExtension("countryId", myDeletedCountryId);
        }
    }
}

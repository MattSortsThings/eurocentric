using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class HandleContestDeletedTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task DomainEventHandler_should_update_all_participating_countries_when_contest_deleted(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_all_my_countries_with_contest_year(2016);

        await admin.Given_I_want_to_delete_my_Liverpool_format_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_every_country_should_now_participate_in_my_Stockholm_format_contest_only();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithoutResponse(apiDriver)
    {
        private CountryIdLookup CountryIds { get; } = new();

        private Guid? StockholmFormatContestId { get; set; }

        private Guid? LiverpoolFormatContestId { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            List<Country> createdCountries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);
            CountryIds.Populate(createdCountries);
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest_for_all_my_countries_with_contest_year(int contestYear)
        {
            Guid[] countryIds = CountryIds.GetAll();

            Contest createdContest = await ApiDriver.CreateSingleStockholmFormatContestAsync(contestYear: contestYear,
                cityName: TestDefaults.CityName,
                group1CountryIds: countryIds.Take(3).ToArray(),
                group2CountryIds: countryIds.Skip(3).ToArray());

            StockholmFormatContestId = createdContest.Id;
        }

        public async Task Given_I_have_created_a_Liverpool_format_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string group0CountryCode = "",
            int contestYear = 0)
        {
            Contest createdContest = await ApiDriver.CreateSingleLiverpoolFormatContestAsync(contestYear: contestYear,
                cityName: TestDefaults.CityName,
                group0CountryId: CountryIds.GetSingle(group0CountryCode),
                group1CountryIds: CountryIds.GetMultiple(group1CountryCodes),
                group2CountryIds: CountryIds.GetMultiple(group2CountryCodes));

            LiverpoolFormatContestId = createdContest.Id;
        }

        public async Task Given_I_want_to_delete_my_Liverpool_format_contest()
        {
            Guid myContestId = await Assert.That(LiverpoolFormatContestId).IsNotNull();

            Request = ApiDriver.RequestFactory.Contests.DeleteContest(myContestId);
        }

        public async Task Then_every_country_should_now_participate_in_my_Stockholm_format_contest_only()
        {
            Guid myContestId = await Assert.That(StockholmFormatContestId).IsNotNull();

            Country[] allCountries = await ApiDriver.GetAllCountriesAsync();

            await Assert.That(allCountries)
                .ContainsOnly(country => country.ParticipatingContestIds.Single() == myContestId);
        }
    }
}

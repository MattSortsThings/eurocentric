using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Contests.GetContests;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestsTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_all_existing_contests_in_contest_year_order(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(contestYear: 2025,
            cityName: "Basel",
            group0CountryCode: "XX",
            group1CountryCodes: ["DK", "EE", "FI"],
            group2CountryCodes: ["AT", "BE", "CZ"]);
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2022,
            cityName: "Turin",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(contestYear: 2024,
            cityName: "Malmö",
            group0CountryCode: "XX",
            group1CountryCodes: ["DK", "EE", "FI"],
            group2CountryCodes: ["AT", "BE", "CZ"]);
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2018,
            cityName: "Lisbon",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_empty_list_when_no_contests_exist(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_contests_should_be_an_empty_list();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<GetContestsResponse>(apiDriver)
    {
        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(7);

        private List<Contest> MyContests { get; } = new(4);

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            List<Country> createdCountries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);

            foreach (Country country in createdCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string cityName = "",
            int contestYear = 0)
        {
            Guid[] group1CountryIds = group1CountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();
            Guid[] group2CountryIds = group2CountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();

            Contest contest = await ApiDriver.CreateSingleStockholmFormatContestAsync(contestYear: contestYear,
                cityName: cityName,
                group1CountryIds: group1CountryIds,
                group2CountryIds: group2CountryIds);

            MyContests.Add(contest);
        }

        public async Task Given_I_have_created_a_Liverpool_format_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string group0CountryCode = "",
            string cityName = "",
            int contestYear = 0)
        {
            Guid[] group1CountryIds = group1CountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();
            Guid[] group2CountryIds = group2CountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();

            Contest contest = await ApiDriver.CreateSingleLiverpoolFormatContestAsync(contestYear: contestYear,
                cityName: cityName,
                group0CountryId: MyCountryCodesAndIds[group0CountryCode],
                group1CountryIds: group1CountryIds,
                group2CountryIds: group2CountryIds);

            MyContests.Add(contest);
        }

        public void Given_I_want_to_retrieve_all_existing_contests() =>
            Request = ApiDriver.RequestFactory.Contests.GetContests();

        public async Task Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order()
        {
            IOrderedEnumerable<Contest> expectedContests = MyContests.OrderBy(contest => contest.ContestYear);

            GetContestsResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.Contests)
                .IsEquivalentTo(expectedContests, new ContestEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_contests_should_be_an_empty_list()
        {
            GetContestsResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.Contests).IsEmpty();
        }
    }
}

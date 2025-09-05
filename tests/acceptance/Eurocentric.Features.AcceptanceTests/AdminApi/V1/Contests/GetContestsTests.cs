using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.DataSourceGenerators;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.Helpers.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.Helpers.Countries;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Contests.GetContests;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestsTests : SerialCleanAcceptanceTest
{
    [Test]
    [AdminApiV1Point0AndUp]
    public async Task Endpoint_should_retrieve_all_existing_contests_ordered_by_contest_year(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(
            contestYear: 2024,
            cityName: "Malmö",
            group1Countries: ["AT", "BE", "CZ"],
            group2Countries: ["DK", "EE", "FI"],
            globalTelevoteCountry: "XX");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(
            contestYear: 2018,
            cityName: "Lisbon",
            group1Countries: ["BE", "DK", "FI"],
            group2Countries: ["AT", "CZ", "EE"]);
        await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(
            contestYear: 2025,
            cityName: "Basel",
            group1Countries: ["AT", "BE", "CZ", "DK"],
            group2Countries: ["EE", "FI", "GB", "HR"],
            globalTelevoteCountry: "XX");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(
            contestYear: 2016,
            cityName: "Stockholm",
            group1Countries: ["BE", "DK", "GB", "HR"],
            group2Countries: ["AT", "CZ", "EE"]);

        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order();
    }

    [Test]
    [AdminApiV1Point0AndUp]
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
        private CountryIdLookup CountryIds { get; } = new();

        private List<Contest> Contests { get; } = [];

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            await foreach (Country createdCountry in ApiDriver.CreateMultipleCountriesAsync(countryCodes))
            {
                CountryIds.Add(createdCountry);
            }
        }

        public async Task Given_I_have_created_a_Liverpool_format_contest_for_my_countries(
            string globalTelevoteCountry = "",
            string[] group2Countries = null!,
            string[] group1Countries = null!,
            string cityName = "",
            int contestYear = 0)
        {
            Contest createdContest = await ApiDriver.CreateSingleLiverpoolFormatContestAsync(contestYear: contestYear,
                cityName: cityName,
                group1CountryIds: group1Countries.Select(CountryIds.GetSingle).ToArray(),
                group2CountryIds: group2Countries.Select(CountryIds.GetSingle).ToArray(),
                globalTelevoteCountryId: CountryIds.GetSingle(globalTelevoteCountry));

            Contests.Add(createdContest);
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest_for_my_countries(
            string[] group2Countries = null!,
            string[] group1Countries = null!,
            string cityName = "",
            int contestYear = 0)
        {
            Contest createdContest = await ApiDriver.CreateSingleStockholmFormatContestAsync(contestYear: contestYear,
                cityName: cityName,
                group1CountryIds: group1Countries.Select(CountryIds.GetSingle).ToArray(),
                group2CountryIds: group2Countries.Select(CountryIds.GetSingle).ToArray());

            Contests.Add(createdContest);
        }

        public void Given_I_want_to_retrieve_all_existing_contests() =>
            Request = ApiDriver.RequestFactory.Contests.GetContests();

        public async Task Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order()
        {
            GetContestsResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            IOrderedEnumerable<Contest> expectedContests = Contests.OrderBy(contest => contest.ContestYear);

            Contest[] actualContests = responseBody.Contests;

            await Assert.That(actualContests)
                .IsEquivalentTo(expectedContests, new ContestEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_contests_should_be_an_empty_list()
        {
            GetContestsResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.Contests).IsEmpty();
        }
    }
}

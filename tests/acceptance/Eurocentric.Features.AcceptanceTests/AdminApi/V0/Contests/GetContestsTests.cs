using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Extensions.Countries;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Contests.GetContests;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Contests;

public static class GetContestsTests
{
    public sealed class GetContests : SerialCleanAcceptanceTest
    {
        [Test]
        [Arguments("v0.2")]
        public async Task Should_retrieve_all_existing_contests_ordered_by_contest_year(string apiVersion)
        {
            AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(contestYear: 2023,
                cityName: "Liverpool",
                countryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI", "XX"]);
            await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
                cityName: "Stockholm",
                countryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI", "XX"]);
            await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(contestYear: 2024,
                cityName: "Malmö",
                countryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI", "XX"]);
            await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2018,
                cityName: "Lisbon",
                countryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI", "XX"]);

            admin.Given_I_want_to_retrieve_all_existing_contests();

            // When
            await admin.When_I_send_my_request();

            // Then
            await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
            await admin.Then_the_retrieved_contests_should_be_my_contests_ordered_by_contest_year();
        }

        [Test]
        [Arguments("v0.2")]
        public async Task Should_retrieve_empty_list_when_no_contests_exist(string apiVersion)
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
    }

    private sealed class AdminActor : AdminActorWithResponse<GetContestsResponse>
    {
        public AdminActor(IApiDriver apiDriver) : base(apiDriver)
        {
        }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(7);

        private List<Contest> MyContests { get; } = new(3);

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Dictionary<string, Guid> countries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);
            foreach (var (countryCode, countryId) in countries)
            {
                MyCountryCodesAndIds.Add(countryCode, countryId);
            }
        }

        public async Task Given_I_have_created_a_Liverpool_format_contest_for_my_countries(string[] countryCodes = null!,
            string cityName = "",
            int contestYear = 0)
        {
            Guid[] countryIds = countryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();

            Contest contest = await ApiDriver.CreateSingleContestAsync(contestFormat: ContestFormat.Liverpool,
                cityName: cityName,
                contestYear: contestYear,
                countryIds: countryIds);

            MyContests.Add(contest);
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest_for_my_countries(string[] countryCodes = null!,
            string cityName = "",
            int contestYear = 0)
        {
            Guid[] countryIds = countryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();

            Contest contest = await ApiDriver.CreateSingleContestAsync(contestFormat: ContestFormat.Stockholm,
                cityName: cityName,
                contestYear: contestYear,
                countryIds: countryIds);

            MyContests.Add(contest);
        }

        public void Given_I_want_to_retrieve_all_existing_contests() =>
            Request = ApiDriver.RequestFactory.Contests.GetContests();

        public async Task Then_the_retrieved_contests_should_be_my_contests_ordered_by_contest_year()
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

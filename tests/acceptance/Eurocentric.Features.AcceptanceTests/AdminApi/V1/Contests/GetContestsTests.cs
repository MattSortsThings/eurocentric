using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestsTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_all_existing_contests_in_contest_year_order(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR");
        await admin.Given_I_have_created_a_contest(
            contestFormat: "Stockholm",
            contestYear: 2022,
            cityName: "Turin",
            group1CountryCodes: ["DK", "BE", "CZ"],
            group2CountryCodes: ["AT", "EE", "FI"]);
        await admin.Given_I_have_created_a_contest(
            contestFormat: "Stockholm",
            contestYear: 2016,
            cityName: "Lisbon",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_contest(
            contestFormat: "Stockholm",
            contestYear: 2017,
            cityName: "Kyiv",
            group1CountryCodes: ["AT", "BE", "CZ", "HR"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_200_OK();
        admin.Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_empty_list_when_no_contests_exist(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_200_OK();
        admin.Then_the_retrieved_contests_should_be_an_empty_list();
    }

    private sealed class AdminActor : ActorWithResponse<GetContestsResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver, IWebAppFixtureBackDoor backDoor) : base(apiDriver)
        {
            BackDoor = backDoor;
        }

        private IWebAppFixtureBackDoor BackDoor { get; }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(8);

        private List<Contest> MyContests { get; } = new(3);

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Country[] myCountries = await ApiDriver.Countries.CreateMultipleCountriesAsync(countryCodes,
                TestContext.Current.CancellationToken);

            foreach (Country country in myCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_contest(int contestYear = 0,
            string cityName = "",
            string contestFormat = "",
            string? group0CountryCode = null,
            string[]? group1CountryCodes = null,
            string[]? group2CountryCodes = null)
        {
            Contest myContest = await ApiDriver.Contests.CreateAContestAsync(cityName: cityName,
                contestFormat: Enum.Parse<ContestFormat>(contestFormat),
                contestYear: contestYear,
                group0CountryId: group0CountryCode is null ? null : MyCountryCodesAndIds[group0CountryCode],
                group1CountryIds: group1CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray(),
                group2CountryIds: group2CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray());

            MyContests.Add(myContest);
        }

        public void Given_I_want_to_retrieve_all_existing_contests() =>
            SendMyRequest = apiDriver => apiDriver.Contests.GetContests(TestContext.Current.CancellationToken);

        public void Then_the_retrieved_contests_should_be_an_empty_list()
        {
            Assert.NotNull(ResponseObject);

            Assert.Empty(ResponseObject.Contests);
        }

        public void Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order()
        {
            Assert.NotNull(ResponseObject);

            IOrderedEnumerable<Contest> expectedContests = MyContests.OrderBy(contest => contest.ContestYear);

            Assert.Equal(expectedContests, ResponseObject.Contests, new ContestEqualityComparer());
        }
    }
}

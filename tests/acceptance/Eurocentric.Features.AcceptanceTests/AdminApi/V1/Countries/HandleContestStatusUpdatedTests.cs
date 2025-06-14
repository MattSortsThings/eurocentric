using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class HandleContestStatusUpdatedTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_replace_participating_contest_memo_in_all_participating_countries_when_contest_status_updated(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competingCountryCodes: ["AT", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        await admin.Then_the_following_countries_should_have_no_participating_contests("GB", "HR", "IT");
        await admin.Then_the_following_countries_should_have_my_InProgress_contest_as_their_only_participating_contest(
            "AT", "BE", "CZ", "DK", "EE", "FI", "XX");
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_not_replace_participating_contest_memo_in_participating_countries_when_contest_status_not_updated(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competingCountryCodes: ["XX", "XX", "XX"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        await admin.Then_the_following_countries_should_have_no_participating_contests("GB", "HR", "IT");
        await admin.Then_the_following_countries_should_have_my_Initialized_contest_as_their_only_participating_contest(
            "AT", "BE", "CZ", "DK", "EE", "FI", "XX");
    }

    private sealed class AdminActor : ActorWithResponse<CreateChildBroadcastResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(10);

        private Guid MyContestId { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Country[] myCountries = await ApiDriver.Countries.CreateMultipleCountriesAsync(countryCodes,
                TestContext.Current.CancellationToken);

            foreach (Country country in myCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Liverpool_format_contest(
            string[]? group1CountryCodes = null,
            string[]? group2CountryCodes = null,
            string group0CountryCode = "")
        {
            Contest myContest = await ApiDriver.Contests.CreateAContestAsync(contestFormat: ContestFormat.Liverpool,
                cityName: "CityName",
                contestYear: 2025,
                group0CountryId: MyCountryCodesAndIds[group0CountryCode],
                group1CountryIds: group1CountryCodes?.Select(code => MyCountryCodesAndIds[code]) ?? [],
                group2CountryIds: group2CountryCodes?.Select(code => MyCountryCodesAndIds[code]) ?? [],
                cancellationToken: TestContext.Current.CancellationToken);

            MyContestId = myContest.Id;
        }

        public void Given_I_want_to_create_a_child_broadcast_for_my_contest(string[]? competingCountryCodes = null,
            string contestStage = "")
        {
            CreateChildBroadcastRequest requestBody = new()
            {
                BroadcastDate = DateOnly.ParseExact("2025-05-01", "yyyy-MM-dd"),
                ContestStage = Enum.Parse<ContestStage>(contestStage),
                CompetingCountryIds = competingCountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray() ?? []
            };

            Guid myContestId = MyContestId;

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateChildBroadcast(myContestId, requestBody, TestContext.Current.CancellationToken);
        }

        public async Task Then_the_following_countries_should_have_my_Initialized_contest_as_their_only_participating_contest(
            params string[] countryCodes)
        {
            ContestMemo expectedMemo = new(MyContestId, ContestStatus.Initialized);

            Country[] allExistingCountries =
                await ApiDriver.Countries.GetAllCountriesAsync(TestContext.Current.CancellationToken);

            IEnumerable<Country> countriesToVerify = allExistingCountries
                .Join(countryCodes,
                    country => country.CountryCode,
                    code => code,
                    (country, _) => country);

            Assert.All(countriesToVerify, country =>
            {
                ContestMemo memo = Assert.Single(country.ParticipatingContests);
                Assert.Equal(expectedMemo, memo);
            });
        }

        public async Task Then_the_following_countries_should_have_my_InProgress_contest_as_their_only_participating_contest(
            params string[] countryCodes)
        {
            ContestMemo expectedMemo = new(MyContestId, ContestStatus.InProgress);

            Country[] allExistingCountries =
                await ApiDriver.Countries.GetAllCountriesAsync(TestContext.Current.CancellationToken);

            IEnumerable<Country> countriesToVerify = allExistingCountries
                .Join(countryCodes,
                    country => country.CountryCode,
                    code => code,
                    (country, _) => country);

            Assert.All(countriesToVerify, country =>
            {
                ContestMemo memo = Assert.Single(country.ParticipatingContests);
                Assert.Equal(expectedMemo, memo);
            });
        }

        public async Task Then_the_following_countries_should_have_no_participating_contests(params string[] countryCodes)
        {
            Country[] allExistingCountries =
                await ApiDriver.Countries.GetAllCountriesAsync(TestContext.Current.CancellationToken);

            IEnumerable<Country> countriesToVerify = allExistingCountries
                .Join(countryCodes,
                    country => country.CountryCode,
                    code => code,
                    (country, _) => country);

            Assert.All(countriesToVerify, country => Assert.Empty(country.ParticipatingContests));
        }
    }
}

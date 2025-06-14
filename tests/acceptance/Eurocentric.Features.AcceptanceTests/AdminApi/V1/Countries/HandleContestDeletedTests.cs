using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class HandleContestDeletedTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_remove_participating_contest_memo_from_all_participating_countries_when_contest_deleted(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT");
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_delete_my_contest_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_all_countries_should_have_no_participating_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_not_update_any_countries_when_contest_deletion_fails(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT");
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);
        admin.Given_I_want_to_delete_my_contest_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        await admin.Then_the_following_countries_should_have_my_InProgress_contest_as_their_only_participating_contest(
            "AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Then_the_following_countries_should_have_no_participating_contests("GB", "HR", "IT");
    }

    private sealed class AdminActor : ActorWithoutResponse
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(9);

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

        public async Task Given_I_have_created_a_Stockholm_format_contest(
            string[]? group1CountryCodes = null,
            string[]? group2CountryCodes = null,
            int contestYear = 0)
        {
            Contest myContest = await ApiDriver.Contests.CreateAContestAsync(
                cityName: "CityName",
                contestFormat: ContestFormat.Stockholm,
                contestYear: contestYear,
                group0CountryId: null,
                group1CountryIds: group1CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray(),
                group2CountryIds: group2CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray());

            MyContestId = myContest.Id;
        }

        public async Task Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(
            string[]? competingCountryCodes = null,
            string broadcastDate = "") => _ = await ApiDriver.Contests.CreateAChildBroadcastAsync(
            contestId: MyContestId,
            contestStage: ContestStage.SemiFinal1,
            broadcastDate: DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
            competingCountryIds: competingCountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray() ?? [],
            cancellationToken: TestContext.Current.CancellationToken);

        public void Given_I_want_to_delete_my_contest_by_its_ID()
        {
            Guid myContestId = MyContestId;

            SendMyRequest = apiDriver => apiDriver.Contests.DeleteContest(myContestId, TestContext.Current.CancellationToken);
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

        public async Task Then_all_countries_should_have_no_participating_contests()
        {
            Country[] countriesToVerify = await ApiDriver.Countries.GetAllCountriesAsync(TestContext.Current.CancellationToken);

            Assert.All(countriesToVerify, country => Assert.Empty(country.ParticipatingContests));
        }
    }
}

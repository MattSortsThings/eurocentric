using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class DeleteContestTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_delete_contest_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR");
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_delete_my_contest_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_delete_contest_with_child_broadcast_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR");
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            competingCountryCodes: ["AT", "DK"]);
        admin.Given_I_want_to_delete_my_contest_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Cannot delete contest",
            detail: "A contest may only be deleted if it has no child broadcasts.");
        admin.Then_the_problem_details_extensions_should_contain_my_contest_ID_with_key("contestId");
        await admin.Then_my_contest_should_be_retrievable_by_its_ID();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_delete_non_existent_contest_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR");
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_deleted_my_contest();
        admin.Given_I_want_to_delete_my_contest_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_404_NotFound();
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Contest not found",
            detail: "No contest exists with the provided contest ID.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    private sealed class AdminActor : ActorWithoutResponse
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(9);

        private Contest? MyContest { get; set; }

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
            int contestYear = 0) => MyContest =
            await ApiDriver.Contests.CreateAContestAsync(
                cityName: "CityName",
                contestFormat: ContestFormat.Stockholm,
                contestYear: contestYear,
                group0CountryId: null,
                group1CountryIds: group1CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray(),
                group2CountryIds: group2CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray());

        public async Task Given_I_have_deleted_my_contest()
        {
            Assert.NotNull(MyContest);

            await ApiDriver.Contests.DeleteAContestAsync(MyContest.Id, TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_created_a_child_broadcast_for_my_contest(string broadcastDate = "",
            string contestStage = "",
            string[]? competingCountryCodes = null)
        {
            Assert.NotNull(MyContest);

            _ = await ApiDriver.Contests.CreateAChildBroadcastAsync(contestId: MyContest.Id,
                contestStage: Enum.Parse<ContestStage>(contestStage),
                broadcastDate: DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
                competingCountryIds: competingCountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray() ?? [],
                cancellationToken: TestContext.Current.CancellationToken);

            MyContest = await ApiDriver.Contests.GetAContestAsync(MyContest.Id, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_delete_my_contest_by_its_ID()
        {
            Assert.NotNull(MyContest);

            Guid myContestId = MyContest.Id;

            SendMyRequest = apiDriver => apiDriver.Contests.DeleteContest(myContestId, TestContext.Current.CancellationToken);
        }

        public async Task Then_there_should_be_no_existing_contests()
        {
            Contest[] existingContests = await ApiDriver.Contests.GetAllContestAsync(TestContext.Current.CancellationToken);

            Assert.Empty(existingContests);
        }

        public void Then_the_problem_details_extensions_should_contain_my_contest_ID_with_key(string key)
        {
            Assert.NotNull(MyContest);

            Then_the_problem_details_extensions_should_contain(key, MyContest.Id);
        }

        public async Task Then_my_contest_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(MyContest);

            Contest myContestRetrievedAgain =
                await ApiDriver.Contests.GetAContestAsync(MyContest.Id, TestContext.Current.CancellationToken);

            Assert.Equal(MyContest, myContestRetrievedAgain, new ContestEqualityComparer());
        }
    }
}

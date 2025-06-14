using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using JetBrains.Annotations;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class HandleBroadcastDeletedTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_update_parent_contest_status_and_remove_child_broadcast_memo_when_broadcast_deleted_scenario_1(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_GrandFinal_child_broadcast_for_my_contest(
            broadcastDate: "2025-05-03",
            competingCountryCodes: ["AT", "DK"]);
        admin.Given_I_want_to_delete_my_GrandFinal_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_my_contest_status_should_now_be("Initialized");
        await admin.Then_my_contest_should_now_have_no_child_broadcasts();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_update_parent_contest_status_and_remove_child_broadcast_memo_when_broadcast_deleted_scenario_2(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE"]);
        await admin.Given_I_have_created_the_SemiFinal2_child_broadcast_for_my_contest(
            broadcastDate: "2025-05-02",
            competingCountryCodes: ["DK", "EE"]);
        await admin.Given_I_have_created_the_GrandFinal_child_broadcast_for_my_contest(
            broadcastDate: "2025-05-03",
            competingCountryCodes: ["AT", "DK"]);
        await admin.Given_I_have_awarded_all_the_points_in_my_SemiFinal1_broadcast();
        await admin.Given_I_have_awarded_all_the_points_in_my_SemiFinal2_broadcast();
        admin.Given_I_want_to_delete_my_GrandFinal_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_my_contest_status_should_now_be("InProgress");
        await admin.Then_my_contest_should_now_have_SemiFinal1_and_SemiFinal2_child_broadcasts_only();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_update_parent_contest_status_and_remove_child_broadcast_memo_when_broadcast_deleted_scenario_3(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE"]);
        await admin.Given_I_have_created_the_SemiFinal2_child_broadcast_for_my_contest(
            broadcastDate: "2025-05-02",
            competingCountryCodes: ["DK", "EE"]);
        await admin.Given_I_have_created_the_GrandFinal_child_broadcast_for_my_contest(
            broadcastDate: "2025-05-03",
            competingCountryCodes: ["AT", "DK"]);
        await admin.Given_I_have_awarded_all_the_points_in_my_SemiFinal1_broadcast();
        await admin.Given_I_have_awarded_all_the_points_in_my_SemiFinal2_broadcast();
        await admin.Given_I_have_awarded_all_the_points_in_my_GrandFinal_broadcast();
        admin.Given_I_want_to_delete_my_GrandFinal_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_my_contest_status_should_now_be("InProgress");
        await admin.Then_my_contest_should_now_have_SemiFinal1_and_SemiFinal2_child_broadcasts_only();
    }

    private sealed class AdminActor : ActorWithoutResponse
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(7);

        private Guid MyContestId { get; set; }

        private Broadcast? MySemiFinal1Broadcast { get; set; }

        private Broadcast? MySemiFinal2Broadcast { get; set; }

        private Broadcast? MyGrandFinalBroadcast { get; set; }

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
            string group0CountryCode = "",
            int contestYear = 0)
        {
            Contest myContest = await ApiDriver.Contests.CreateAContestAsync(
                cityName: "CityName",
                contestFormat: ContestFormat.Liverpool,
                contestYear: contestYear,
                group0CountryId: MyCountryCodesAndIds[group0CountryCode],
                group1CountryIds: group1CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray(),
                group2CountryIds: group2CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray());

            MyContestId = myContest.Id;
        }

        public async Task Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(
            string[]? competingCountryCodes = null,
            string broadcastDate = "")
        {
            Guid[] competingCountryIds = competingCountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray() ?? [];

            MySemiFinal1Broadcast = await ApiDriver.Contests.CreateAChildBroadcastAsync(contestId: MyContestId,
                broadcastDate: DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
                contestStage: ContestStage.SemiFinal1,
                competingCountryIds: competingCountryIds,
                cancellationToken: TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_created_the_SemiFinal2_child_broadcast_for_my_contest(
            string[]? competingCountryCodes = null,
            string broadcastDate = "")
        {
            Guid[] competingCountryIds = competingCountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray() ?? [];

            MySemiFinal2Broadcast = await ApiDriver.Contests.CreateAChildBroadcastAsync(contestId: MyContestId,
                broadcastDate: DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
                contestStage: ContestStage.SemiFinal2,
                competingCountryIds: competingCountryIds,
                cancellationToken: TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_created_the_GrandFinal_child_broadcast_for_my_contest(
            string[]? competingCountryCodes = null,
            string broadcastDate = "")
        {
            Guid[] competingCountryIds = competingCountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray() ?? [];

            MyGrandFinalBroadcast = await ApiDriver.Contests.CreateAChildBroadcastAsync(contestId: MyContestId,
                broadcastDate: DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
                contestStage: ContestStage.GrandFinal,
                competingCountryIds: competingCountryIds,
                cancellationToken: TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_awarded_all_the_points_in_my_SemiFinal1_broadcast()
        {
            Assert.NotNull(MySemiFinal1Broadcast);

            await ApiDriver.Broadcasts.AwardAllPointsInABroadcastAsync(MySemiFinal1Broadcast,
                TestContext.Current.CancellationToken);

            MySemiFinal1Broadcast = await ApiDriver.Broadcasts.GetABroadcastAsync(MySemiFinal1Broadcast.Id,
                TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_awarded_all_the_points_in_my_SemiFinal2_broadcast()
        {
            Assert.NotNull(MySemiFinal2Broadcast);

            await ApiDriver.Broadcasts.AwardAllPointsInABroadcastAsync(MySemiFinal2Broadcast,
                TestContext.Current.CancellationToken);

            MySemiFinal2Broadcast =
                await ApiDriver.Broadcasts.GetABroadcastAsync(MySemiFinal2Broadcast.Id, TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_awarded_all_the_points_in_my_GrandFinal_broadcast()
        {
            Assert.NotNull(MyGrandFinalBroadcast);

            await ApiDriver.Broadcasts.AwardAllPointsInABroadcastAsync(MyGrandFinalBroadcast,
                TestContext.Current.CancellationToken);

            MyGrandFinalBroadcast =
                await ApiDriver.Broadcasts.GetABroadcastAsync(MyGrandFinalBroadcast.Id, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_delete_my_GrandFinal_broadcast()
        {
            Assert.NotNull(MyGrandFinalBroadcast);

            Guid myBroadcastId = MyGrandFinalBroadcast.Id;

            SendMyRequest = apiDriver =>
                apiDriver.Broadcasts.DeleteBroadcast(myBroadcastId, TestContext.Current.CancellationToken);
        }

        public async Task Then_my_contest_status_should_now_be(string contestStatus)
        {
            Contest myRetrievedContest =
                await ApiDriver.Contests.GetAContestAsync(MyContestId, TestContext.Current.CancellationToken);

            Assert.Equal(Enum.Parse<ContestStatus>(contestStatus), myRetrievedContest.ContestStatus);
        }

        public async Task Then_my_contest_should_now_have_SemiFinal1_and_SemiFinal2_child_broadcasts_only()
        {
            Assert.NotNull(MySemiFinal1Broadcast);
            Assert.NotNull(MySemiFinal2Broadcast);

            Contest myRetrievedContest =
                await ApiDriver.Contests.GetAContestAsync(MyContestId, TestContext.Current.CancellationToken);

            Assert.Collection(myRetrievedContest.ChildBroadcasts, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
                Assert.Equal(MySemiFinal1Broadcast.Id, memo.BroadcastId);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal2, memo.ContestStage);
                Assert.Equal(MySemiFinal2Broadcast.Id, memo.BroadcastId);
            });
        }

        public async Task Then_my_contest_should_now_have_no_child_broadcasts()
        {
            Contest myRetrievedContest =
                await ApiDriver.Contests.GetAContestAsync(MyContestId, TestContext.Current.CancellationToken);

            Assert.Empty(myRetrievedContest.ChildBroadcasts);
        }
    }
}

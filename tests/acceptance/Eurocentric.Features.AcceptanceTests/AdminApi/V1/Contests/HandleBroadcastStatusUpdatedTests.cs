using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using JetBrains.Annotations;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class HandleBroadcastStatusUpdatedTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_update_parent_contest_status_and_replace_child_broadcast_memo_when_broadcast_updated_scenario_1(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE"]);
        admin.Given_I_want_to_award_televote_points_in_my_SemiFinal1_broadcast(
            votingCountryCode: "XX",
            competingCountryCodes: ["AT", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_my_contest_status_should_now_be("InProgress");
        await admin.Then_my_contest_should_contain_my_InProgress_SemiFinal1_broadcast();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_update_parent_contest_status_and_replace_child_broadcast_memo_when_broadcast_updated_scenario_2(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
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
        await admin.Given_I_have_awarded_all_the_points_in_my_SemiFinal2_broadcast();
        await admin.Given_I_have_awarded_all_the_points_in_my_GrandFinal_broadcast();
        admin.Given_I_want_to_award_televote_points_in_my_SemiFinal1_broadcast(
            votingCountryCode: "XX",
            competingCountryCodes: ["AT", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_my_contest_status_should_now_be("InProgress");
        await admin.Then_my_contest_should_contain_my_InProgress_SemiFinal1_and_Completed_SemiFinal2_and_GrandFinal_broadcasts();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_update_parent_contest_status_and_replace_child_broadcast_memo_when_broadcast_updated_scenario_3(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
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
        await admin.Given_I_have_awarded_all_the_points_in_my_SemiFinal2_broadcast();
        await admin.Given_I_have_awarded_all_the_points_in_my_GrandFinal_broadcast();
        await admin.Given_I_have_awarded_televote_points_in_my_SemiFinal1_broadcast(
            votingCountryCode: "AT",
            competingCountryCodes: ["BE"]);
        await admin.Given_I_have_awarded_televote_points_in_my_SemiFinal1_broadcast(
            votingCountryCode: "BE",
            competingCountryCodes: ["AT"]);
        await admin.Given_I_have_awarded_televote_points_in_my_SemiFinal1_broadcast(
            votingCountryCode: "CZ",
            competingCountryCodes: ["AT", "BE"]);
        admin.Given_I_want_to_award_televote_points_in_my_SemiFinal1_broadcast(
            votingCountryCode: "XX",
            competingCountryCodes: ["AT", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_my_contest_status_should_now_be("Completed");
        await admin.Then_my_contest_should_contain_my_Completed_SemiFinal1_and_SemiFinal2_and_GrandFinal_broadcasts();
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
            string[]? group2CountryCodes = null,
            string[]? group1CountryCodes = null,
            string group0CountryCode = "")
        {
            Contest myContest = await ApiDriver.Contests.CreateAContestAsync(
                cityName: "CityName",
                contestFormat: ContestFormat.Liverpool,
                contestYear: 2025,
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

        public async Task Given_I_have_awarded_televote_points_in_my_SemiFinal1_broadcast(string[]? competingCountryCodes = null,
            string votingCountryCode = "")
        {
            Assert.NotNull(MySemiFinal1Broadcast);

            await ApiDriver.Broadcasts.AwardASetOfTelevotePointsAsync(broadcastId: MySemiFinal1Broadcast.Id,
                votingCountryId: MyCountryCodesAndIds[votingCountryCode],
                rankedCompetingCountryIds: competingCountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray() ?? [],
                cancellationToken: TestContext.Current.CancellationToken);

            MySemiFinal1Broadcast =
                await ApiDriver.Broadcasts.GetABroadcastAsync(MySemiFinal1Broadcast.Id, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_award_televote_points_in_my_SemiFinal1_broadcast(string[]? competingCountryCodes = null,
            string votingCountryCode = "")
        {
            Assert.NotNull(MySemiFinal1Broadcast);

            Guid myBroadcastId = MySemiFinal1Broadcast.Id;

            AwardTelevotePointsRequest requestBody = new()
            {
                VotingCountryId = MyCountryCodesAndIds[votingCountryCode],
                RankedCompetingCountryIds = competingCountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray() ?? []
            };

            SendMyRequest = apiDriver =>
                apiDriver.Broadcasts.AwardTelevotePoints(myBroadcastId, requestBody, TestContext.Current.CancellationToken);
        }

        public async Task Then_my_contest_status_should_now_be(string contestStatus)
        {
            Contest myRetrievedContest =
                await ApiDriver.Contests.GetAContestAsync(MyContestId, TestContext.Current.CancellationToken);

            Assert.Equal(Enum.Parse<ContestStatus>(contestStatus), myRetrievedContest.ContestStatus);
        }

        public async Task
            Then_my_contest_should_contain_my_InProgress_SemiFinal1_and_Completed_SemiFinal2_and_GrandFinal_broadcasts()
        {
            Assert.NotNull(MySemiFinal1Broadcast);
            Assert.NotNull(MySemiFinal2Broadcast);
            Assert.NotNull(MyGrandFinalBroadcast);

            Contest myRetrievedContest =
                await ApiDriver.Contests.GetAContestAsync(MyContestId, TestContext.Current.CancellationToken);

            Assert.Collection(myRetrievedContest.ChildBroadcasts, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
                Assert.Equal(MySemiFinal1Broadcast.Id, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.InProgress, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal2, memo.ContestStage);
                Assert.Equal(MySemiFinal2Broadcast.Id, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.GrandFinal, memo.ContestStage);
                Assert.Equal(MyGrandFinalBroadcast.Id, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            });
        }

        public async Task Then_my_contest_should_contain_my_Completed_SemiFinal1_and_SemiFinal2_and_GrandFinal_broadcasts()
        {
            Assert.NotNull(MySemiFinal1Broadcast);
            Assert.NotNull(MySemiFinal2Broadcast);
            Assert.NotNull(MyGrandFinalBroadcast);

            Contest myRetrievedContest =
                await ApiDriver.Contests.GetAContestAsync(MyContestId, TestContext.Current.CancellationToken);

            Assert.Collection(myRetrievedContest.ChildBroadcasts, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
                Assert.Equal(MySemiFinal1Broadcast.Id, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal2, memo.ContestStage);
                Assert.Equal(MySemiFinal2Broadcast.Id, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.GrandFinal, memo.ContestStage);
                Assert.Equal(MyGrandFinalBroadcast.Id, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            });
        }

        public async Task Then_my_contest_should_contain_my_InProgress_SemiFinal1_broadcast()
        {
            Assert.NotNull(MySemiFinal1Broadcast);

            Contest myRetrievedContest =
                await ApiDriver.Contests.GetAContestAsync(MyContestId, TestContext.Current.CancellationToken);

            BroadcastMemo singleMemo = Assert.Single(myRetrievedContest.ChildBroadcasts);

            Assert.Equal(ContestStage.SemiFinal1, singleMemo.ContestStage);
            Assert.Equal(MySemiFinal1Broadcast.Id, singleMemo.BroadcastId);
            Assert.Equal(BroadcastStatus.InProgress, singleMemo.BroadcastStatus);
        }
    }
}

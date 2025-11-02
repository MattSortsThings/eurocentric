using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Enums;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests;

public sealed class HandleBroadcastDeletedTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_update_parent_contest_on_broadcast_deleted_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2016,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_all_three_broadcasts_for_my_contest();
        await admin.Given_I_have_awarded_all_the_points_in_all_my_broadcasts();

        admin.Given_I_want_to_delete_my_GrandFinal_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);

        await admin.Then_my_contest_should_now_match(
            queryable: false,
            childBroadcasts: """
            | ContestStage | Completed |
            |--------------|-----------|
            | SemiFinal1   | true      |
            | SemiFinal2   | true      |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_update_parent_contest_on_broadcast_deleted_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2016,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_all_three_broadcasts_for_my_contest();
        await admin.Given_I_have_deleted_my_SemiFinal1_broadcast();

        admin.Given_I_want_to_delete_my_GrandFinal_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);

        await admin.Then_my_contest_should_now_match(
            queryable: false,
            childBroadcasts: """
            | ContestStage | Completed |
            |--------------|-----------|
            | SemiFinal2   | false     |
            """
        );
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private CountryIdLookup ExistingCountryIds { get; } = new();

        private Contest? ExistingContest { get; set; }

        private List<Broadcast> ExistingBroadcasts { get; } = [];

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            ExistingCountryIds.EnsureCapacity(countryCodes.Length);

            await foreach (Country country in Kernel.CreateMultipleCountriesAsync(countryCodes))
            {
                ExistingCountryIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Stockholm_rules_contest(
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            int contestYear = 0
        )
        {
            ExistingContest = await Kernel.CreateAStockholmRulesContestAsync(
                contestYear: contestYear,
                semiFinal1CountryIds: semiFinal1Countries.Select(ExistingCountryIds.GetId).ToArray(),
                semiFinal2CountryIds: semiFinal2Countries.Select(ExistingCountryIds.GetId).ToArray()
            );
        }

        public async Task Given_I_have_created_all_three_broadcasts_for_my_contest()
        {
            Contest contest = await Assert.That(ExistingContest).IsNotNull();

            _ = await Kernel.CreateABroadcastAsync(
                contestId: contest.Id,
                broadcastDate: new DateOnly(contest.ContestYear, 5, 1),
                contestStage: ContestStage.SemiFinal1,
                competingCountryIds: contest
                    .Participants.Where(participant => participant.SemiFinalDraw == SemiFinalDraw.SemiFinal1)
                    .Select(participant => (Guid?)participant.ParticipatingCountryId)
                    .ToArray()
            );

            _ = await Kernel.CreateABroadcastAsync(
                contestId: contest.Id,
                broadcastDate: new DateOnly(contest.ContestYear, 5, 2),
                contestStage: ContestStage.SemiFinal2,
                competingCountryIds: contest
                    .Participants.Where(participant => participant.SemiFinalDraw == SemiFinalDraw.SemiFinal2)
                    .Select(participant => (Guid?)participant.ParticipatingCountryId)
                    .ToArray()
            );

            _ = await Kernel.CreateABroadcastAsync(
                contestId: contest.Id,
                broadcastDate: new DateOnly(contest.ContestYear, 5, 3),
                contestStage: ContestStage.GrandFinal,
                competingCountryIds: contest
                    .Participants.Select(participant => (Guid?)participant.ParticipatingCountryId)
                    .ToArray()
            );

            await RefreshExistingContestAndBroadcasts();
        }

        public async Task Given_I_have_awarded_all_the_points_in_all_my_broadcasts()
        {
            foreach (Broadcast broadcast in ExistingBroadcasts)
            {
                await Kernel.AwardAllBroadcastJuryPointsAsync(broadcast);
                await Kernel.AwardAllBroadcastTelevotePointsAsync(broadcast);
            }

            await RefreshExistingContestAndBroadcasts();
        }

        public async Task Given_I_have_deleted_my_SemiFinal1_broadcast()
        {
            Broadcast broadcast = ExistingBroadcasts.Single(broadcast =>
                broadcast.ContestStage == ContestStage.SemiFinal1
            );

            await Kernel.DeleteABroadcastAsync(broadcast.Id);

            await RefreshExistingContestAndBroadcasts();
        }

        public void Given_I_want_to_delete_my_GrandFinal_broadcast()
        {
            Broadcast broadcast = ExistingBroadcasts.Single(broadcast =>
                broadcast.ContestStage == ContestStage.GrandFinal
            );

            Request = Kernel.Requests.Broadcasts.DeleteBroadcast(broadcast.Id);
        }

        public async Task Then_my_contest_should_now_match(string childBroadcasts = "", bool queryable = true)
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            ChildBroadcast[] expectedChildBroadcasts = MarkdownParser.ParseTable(childBroadcasts, MapToChildBroadcast);

            Contest retrievedContest = await Kernel.GetAContestAsync(contestId);

            await Assert
                .That(retrievedContest)
                .HasProperty(contest => contest.Queryable, queryable)
                .And.Member(
                    contest => contest.ChildBroadcasts,
                    collection => collection.IsEquivalentTo(expectedChildBroadcasts)
                );
        }

        private async Task RefreshExistingContestAndBroadcasts()
        {
            ExistingBroadcasts.Clear();
            ExistingBroadcasts.AddRange(await Kernel.GetAllBroadcastsAsync());

            ExistingContest = await Kernel.GetAContestAsync(ExistingBroadcasts[0].ParentContestId);
        }

        private ChildBroadcast MapToChildBroadcast(Dictionary<string, string> row)
        {
            ContestStage contestStage = Enum.Parse<ContestStage>(row["ContestStage"]);
            Guid childBroadcastId = ExistingBroadcasts.Single(broadcast => broadcast.ContestStage == contestStage).Id;
            bool completed = bool.Parse(row["Completed"]);

            return new ChildBroadcast
            {
                ChildBroadcastId = childBroadcastId,
                ContestStage = contestStage,
                Completed = completed,
            };
        }
    }
}

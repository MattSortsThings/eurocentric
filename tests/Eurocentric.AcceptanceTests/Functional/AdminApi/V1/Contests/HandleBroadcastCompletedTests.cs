using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Features.Broadcasts;
using ContestStage = Eurocentric.Apis.Admin.V1.Enums.ContestStage;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests;

[Category("admin-api")]
public sealed class HandleBroadcastCompletedTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_update_parent_contest_on_broadcast_completed_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2016,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2016-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_have_awarded_all_the_televote_points_in_my_broadcast(contestStage: "SemiFinal1");
        await admin.Given_I_have_awarded_all_the_jury_points_in_my_broadcast(
            contestStage: "SemiFinal1",
            excludedVotingCountry: "CZ"
        );

        admin.Given_I_want_to_award_jury_points_in_my_broadcast(
            contestStage: "SemiFinal1",
            votingCountry: "CZ",
            rankedCompetingCountries: ["AT", "BE"]
        );

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
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_update_parent_contest_on_broadcast_completed_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2016,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2016-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_have_awarded_all_the_jury_points_in_my_broadcast(contestStage: "SemiFinal1");
        await admin.Given_I_have_awarded_all_the_televote_points_in_my_broadcast(
            contestStage: "SemiFinal1",
            excludedVotingCountry: "CZ"
        );

        admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            contestStage: "SemiFinal1",
            votingCountry: "CZ",
            rankedCompetingCountries: ["AT", "BE"]
        );

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
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_update_parent_contest_on_broadcast_completed_scenario_3(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2016,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2016-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2016-05-02",
            competingCountries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2016-05-03",
            competingCountries: ["AT", "FI"]
        );

        await admin.Given_I_have_awarded_all_the_jury_points_in_my_broadcast(contestStage: "SemiFinal1");
        await admin.Given_I_have_awarded_all_the_televote_points_in_my_broadcast(
            contestStage: "SemiFinal1",
            excludedVotingCountry: "CZ"
        );

        admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            contestStage: "SemiFinal1",
            votingCountry: "CZ",
            rankedCompetingCountries: ["AT", "BE"]
        );

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
            | SemiFinal2   | false     |
            | GrandFinal   | false     |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_update_parent_contest_on_broadcast_completed_scenario_4(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2016,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2016-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2016-05-02",
            competingCountries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2016-05-03",
            competingCountries: ["AT", "FI"]
        );

        await admin.Given_I_have_awarded_all_the_jury_points_in_my_broadcast(contestStage: "SemiFinal1");
        await admin.Given_I_have_awarded_all_the_televote_points_in_my_broadcast(contestStage: "SemiFinal1");
        await admin.Given_I_have_awarded_all_the_jury_points_in_my_broadcast(contestStage: "SemiFinal2");
        await admin.Given_I_have_awarded_all_the_televote_points_in_my_broadcast(
            contestStage: "SemiFinal2",
            excludedVotingCountry: "FI"
        );

        admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            contestStage: "SemiFinal2",
            votingCountry: "FI",
            rankedCompetingCountries: ["DK", "EE"]
        );

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
            | GrandFinal   | false     |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_update_parent_contest_on_broadcast_completed_scenario_5(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2016,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2016-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2016-05-02",
            competingCountries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2016-05-03",
            competingCountries: ["AT", "FI"]
        );

        await admin.Given_I_have_awarded_all_the_jury_points_in_my_broadcast(contestStage: "SemiFinal1");
        await admin.Given_I_have_awarded_all_the_televote_points_in_my_broadcast(contestStage: "SemiFinal1");
        await admin.Given_I_have_awarded_all_the_jury_points_in_my_broadcast(contestStage: "SemiFinal2");
        await admin.Given_I_have_awarded_all_the_televote_points_in_my_broadcast(contestStage: "SemiFinal2");
        await admin.Given_I_have_awarded_all_the_jury_points_in_my_broadcast(contestStage: "GrandFinal");
        await admin.Given_I_have_awarded_all_the_televote_points_in_my_broadcast(
            contestStage: "GrandFinal",
            excludedVotingCountry: "DK"
        );

        admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            contestStage: "GrandFinal",
            votingCountry: "DK",
            rankedCompetingCountries: ["AT", "FI"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);
        await admin.Then_my_contest_should_now_match(
            queryable: true,
            childBroadcasts: """
            | ContestStage | Completed |
            |--------------|-----------|
            | SemiFinal1   | true      |
            | SemiFinal2   | true      |
            | GrandFinal   | true      |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_update_parent_contest_on_failed_jury_points_award(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2016,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2016-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_have_awarded_all_the_televote_points_in_my_broadcast(contestStage: "SemiFinal1");
        await admin.Given_I_have_awarded_all_the_jury_points_in_my_broadcast(
            contestStage: "SemiFinal1",
            excludedVotingCountry: "CZ"
        );

        admin.Given_I_want_to_award_jury_points_in_my_broadcast(
            contestStage: "SemiFinal1",
            votingCountry: "CZ",
            rankedCompetingCountries: []
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_my_contest_should_now_match(
            queryable: false,
            childBroadcasts: """
            | ContestStage | Completed |
            |--------------|-----------|
            | SemiFinal1   | false     |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_update_parent_contest_on_failed_televote_points_award(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2016,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2016-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_have_awarded_all_the_jury_points_in_my_broadcast(contestStage: "SemiFinal1");
        await admin.Given_I_have_awarded_all_the_televote_points_in_my_broadcast(
            contestStage: "SemiFinal1",
            excludedVotingCountry: "CZ"
        );

        admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            contestStage: "SemiFinal1",
            votingCountry: "CZ",
            rankedCompetingCountries: []
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_my_contest_should_now_match(
            queryable: false,
            childBroadcasts: """
            | ContestStage | Completed |
            |--------------|-----------|
            | SemiFinal1   | false     |
            """
        );
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor
    {
        private CountryIdLookup ExistingCountryIds { get; } = new();

        private Contest? ExistingContest { get; set; }

        private List<Broadcast> ExistingBroadcasts { get; } = [];

        private protected override AdminKernel Kernel { get; } = kernel;

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

        public async Task Given_I_have_created_a_broadcast_for_my_contest(
            string?[] competingCountries = null!,
            string broadcastDate = "",
            string contestStage = ""
        )
        {
            Guid existingContestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            Broadcast broadcast = await Kernel.CreateABroadcastAsync(
                broadcastDate: DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                contestStage: Enum.Parse<ContestStage>(contestStage),
                contestId: existingContestId,
                competingCountryIds: ExistingCountryIds.MapToNullableGuids(competingCountries)
            );

            ExistingBroadcasts.Add(broadcast);

            ExistingContest = await Kernel.GetAContestAsync(existingContestId);
        }

        public async Task Given_I_have_awarded_all_the_jury_points_in_my_broadcast(
            string? excludedVotingCountry = null,
            string contestStage = ""
        )
        {
            Guid? excludedVotingCountryId = excludedVotingCountry is null
                ? null
                : ExistingCountryIds.GetId(excludedVotingCountry);

            Broadcast broadcast = ExistingBroadcasts.Single(broadcast =>
                broadcast.ContestStage == Enum.Parse<ContestStage>(contestStage)
            );

            await Kernel.AwardAllBroadcastJuryPointsAsync(broadcast, excludedVotingCountryId);
            await RefreshExistingContestAndBroadcasts();
        }

        public async Task Given_I_have_awarded_all_the_televote_points_in_my_broadcast(
            string? excludedVotingCountry = null,
            string contestStage = ""
        )
        {
            Guid? excludedVotingCountryId = excludedVotingCountry is null
                ? null
                : ExistingCountryIds.GetId(excludedVotingCountry);

            Broadcast broadcast = ExistingBroadcasts.Single(broadcast =>
                broadcast.ContestStage == Enum.Parse<ContestStage>(contestStage)
            );

            await Kernel.AwardAllBroadcastTelevotePointsAsync(broadcast, excludedVotingCountryId);
            await RefreshExistingContestAndBroadcasts();
        }

        public void Given_I_want_to_award_televote_points_in_my_broadcast(
            string[] rankedCompetingCountries = null!,
            string votingCountry = "",
            string contestStage = ""
        )
        {
            Broadcast broadcast = ExistingBroadcasts.Single(broadcast =>
                broadcast.ContestStage == Enum.Parse<ContestStage>(contestStage)
            );

            AwardBroadcastTelevotePointsRequest requestBody = new()
            {
                VotingCountryId = ExistingCountryIds.GetId(votingCountry),
                RankedCompetingCountryIds = ExistingCountryIds.MapToGuids(rankedCompetingCountries),
            };

            Request = Kernel.Requests.Broadcasts.AwardBroadcastTelevotePoints(broadcast.Id, requestBody);
        }

        public void Given_I_want_to_award_jury_points_in_my_broadcast(
            string[] rankedCompetingCountries = null!,
            string votingCountry = "",
            string contestStage = ""
        )
        {
            Broadcast broadcast = ExistingBroadcasts.Single(broadcast =>
                broadcast.ContestStage == Enum.Parse<ContestStage>(contestStage)
            );

            AwardBroadcastJuryPointsRequest requestBody = new()
            {
                VotingCountryId = ExistingCountryIds.GetId(votingCountry),
                RankedCompetingCountryIds = ExistingCountryIds.MapToGuids(rankedCompetingCountries),
            };

            Request = Kernel.Requests.Broadcasts.AwardBroadcastJuryPoints(broadcast.Id, requestBody);
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

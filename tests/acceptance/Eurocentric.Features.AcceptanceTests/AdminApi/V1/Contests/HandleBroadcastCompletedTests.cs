using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Broadcasts.AwardTelevotePoints;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class HandleBroadcastCompletedTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task DomainEventHandler_should_update_parent_contest_when_broadcast_completed_scenario_1_of_3(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_have_created_the_GrandFinal_child_broadcast_for_my_contest(broadcastDate: "2025-05-03",
            competingCountryCodes: ["AT", "FI"]);
        await admin.Given_I_have_awarded_all_the_jury_points_in_my_GrandFinal_broadcast();
        await admin.Given_I_have_awarded_all_the_televote_points_in_my_GrandFinal_broadcast_except_the_voting_country("XX");

        admin.Given_I_want_to_award_televote_points_in_my_GrandFinal_broadcast(
            votingCountryCode: "XX",
            rankedCompetingCountryCodes: ["AT", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_my_contest_should_now_match(completed: false,
            childBroadcasts: """
                             | ContestStage | Completed |
                             |--------------|-----------|
                             | GrandFinal   | true      |
                             """);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task DomainEventHandler_should_update_parent_contest_when_broadcast_completed_scenario_2_of_3(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE"]);

        await admin.Given_I_have_created_the_SemiFinal2_child_broadcast_for_my_contest(broadcastDate: "2025-05-02",
            competingCountryCodes: ["DK", "EE"]);

        await admin.Given_I_have_created_the_GrandFinal_child_broadcast_for_my_contest(broadcastDate: "2025-05-03",
            competingCountryCodes: ["AT", "FI"]);
        await admin.Given_I_have_awarded_all_the_jury_points_in_my_GrandFinal_broadcast();
        await admin.Given_I_have_awarded_all_the_televote_points_in_my_GrandFinal_broadcast_except_the_voting_country("XX");

        admin.Given_I_want_to_award_televote_points_in_my_GrandFinal_broadcast(
            votingCountryCode: "XX",
            rankedCompetingCountryCodes: ["AT", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_my_contest_should_now_match(completed: false,
            childBroadcasts: """
                             | ContestStage | Completed |
                             |--------------|-----------|
                             | SemiFinal1   | false     |
                             | SemiFinal2   | false     |
                             | GrandFinal   | true      |
                             """);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task DomainEventHandler_should_update_parent_contest_when_broadcast_completed_scenario_3_of_3(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE"]);
        await admin.Given_I_have_awarded_all_the_points_in_my_SemiFinal1_broadcast();

        await admin.Given_I_have_created_the_SemiFinal2_child_broadcast_for_my_contest(broadcastDate: "2025-05-02",
            competingCountryCodes: ["DK", "EE"]);
        await admin.Given_I_have_awarded_all_the_points_in_my_SemiFinal2_broadcast();

        await admin.Given_I_have_created_the_GrandFinal_child_broadcast_for_my_contest(broadcastDate: "2025-05-03",
            competingCountryCodes: ["AT", "FI"]);
        await admin.Given_I_have_awarded_all_the_jury_points_in_my_GrandFinal_broadcast();
        await admin.Given_I_have_awarded_all_the_televote_points_in_my_GrandFinal_broadcast_except_the_voting_country("XX");

        admin.Given_I_want_to_award_televote_points_in_my_GrandFinal_broadcast(
            votingCountryCode: "XX",
            rankedCompetingCountryCodes: ["AT", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_my_contest_should_now_match(completed: true,
            childBroadcasts: """
                             | ContestStage | Completed |
                             |--------------|-----------|
                             | SemiFinal1   | true      |
                             | SemiFinal2   | true      |
                             | GrandFinal   | true      |
                             """);
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithoutResponse(apiDriver)
    {
        private CountryIdLookup CountryIds { get; } = new();

        private Guid? ContestId { get; set; }

        private BroadcastLookup Broadcasts { get; } = new();

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            List<Country> createdCountries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);
            CountryIds.Populate(createdCountries);
        }

        public async Task Given_I_have_created_a_Liverpool_format_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string group0CountryCode = "",
            int contestYear = 0)
        {
            Guid[] group1CountryIds = CountryIds.GetMultiple(group1CountryCodes);
            Guid[] group2CountryIds = CountryIds.GetMultiple(group2CountryCodes);

            Contest contest = await ApiDriver.CreateSingleLiverpoolFormatContestAsync(contestYear: contestYear,
                cityName: TestDefaults.CityName,
                group0CountryId: CountryIds.GetSingle(group0CountryCode),
                group1CountryIds: group1CountryIds,
                group2CountryIds: group2CountryIds);

            ContestId = contest.Id;
        }

        public async Task Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(
            string[] competingCountryCodes = null!,
            string broadcastDate = "")
        {
            Guid myContestId = await Assert.That(ContestId).IsNotNull();

            Broadcast createdBroadcast = await ApiDriver.CreateSingleBroadcastAsync(contestId: myContestId,
                contestStage: ContestStage.SemiFinal1,
                broadcastDate: DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                competingCountryIds: CountryIds.GetMultiple(competingCountryCodes));

            Broadcasts.AddOrReplace(createdBroadcast);
        }

        public async Task Given_I_have_created_the_SemiFinal2_child_broadcast_for_my_contest(
            string[] competingCountryCodes = null!,
            string broadcastDate = "")
        {
            Guid myContestId = await Assert.That(ContestId).IsNotNull();

            Broadcast createdBroadcast = await ApiDriver.CreateSingleBroadcastAsync(contestId: myContestId,
                contestStage: ContestStage.SemiFinal2,
                broadcastDate: DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                competingCountryIds: CountryIds.GetMultiple(competingCountryCodes));

            Broadcasts.AddOrReplace(createdBroadcast);
        }

        public async Task Given_I_have_created_the_GrandFinal_child_broadcast_for_my_contest(
            string[] competingCountryCodes = null!,
            string broadcastDate = "")
        {
            Guid myContestId = await Assert.That(ContestId).IsNotNull();

            Broadcast createdBroadcast = await ApiDriver.CreateSingleBroadcastAsync(contestId: myContestId,
                contestStage: ContestStage.GrandFinal,
                broadcastDate: DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                competingCountryIds: CountryIds.GetMultiple(competingCountryCodes));

            Broadcasts.AddOrReplace(createdBroadcast);
        }

        public async Task Given_I_have_awarded_all_the_points_in_my_SemiFinal1_broadcast()
        {
            Broadcast myBroadcast = Broadcasts.GetSingle(ContestStage.SemiFinal1);

            await ApiDriver.AwardAllTelevotePointsAsync(myBroadcast);

            Broadcast updatedBroadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcast.Id);

            Broadcasts.AddOrReplace(updatedBroadcast);
        }

        public async Task Given_I_have_awarded_all_the_points_in_my_SemiFinal2_broadcast()
        {
            Broadcast myBroadcast = Broadcasts.GetSingle(ContestStage.SemiFinal2);

            await ApiDriver.AwardAllTelevotePointsAsync(myBroadcast);

            Broadcast updatedBroadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcast.Id);

            Broadcasts.AddOrReplace(updatedBroadcast);
        }

        public async Task Given_I_have_awarded_all_the_jury_points_in_my_GrandFinal_broadcast()
        {
            Broadcast myBroadcast = Broadcasts.GetSingle(ContestStage.GrandFinal);

            await ApiDriver.AwardAllJuryPointsAsync(myBroadcast);

            Broadcast updatedBroadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcast.Id);

            Broadcasts.AddOrReplace(updatedBroadcast);
        }

        public async Task Given_I_have_awarded_all_the_televote_points_in_my_GrandFinal_broadcast_except_the_voting_country(
            string countryCode)
        {
            Broadcast myBroadcast = Broadcasts.GetSingle(ContestStage.GrandFinal);

            await ApiDriver.AwardAllTelevotePointsAsync(myBroadcast, CountryIds.GetSingle(countryCode));

            Broadcast updatedBroadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcast.Id);

            Broadcasts.AddOrReplace(updatedBroadcast);
        }

        public void Given_I_want_to_award_televote_points_in_my_GrandFinal_broadcast(
            string[] rankedCompetingCountryCodes = null!,
            string votingCountryCode = "")
        {
            Broadcast myBroadcast = Broadcasts.GetSingle(ContestStage.GrandFinal);

            AwardTelevotePointsRequest requestBody = new()
            {
                VotingCountryId = CountryIds.GetSingle(votingCountryCode),
                RankedCompetingCountryIds = CountryIds.GetMultiple(rankedCompetingCountryCodes)
            };

            Request = ApiDriver.RequestFactory.Broadcasts.AwardTelevotePoints(myBroadcast.Id, requestBody);
        }

        public async Task Then_my_contest_should_now_match(string childBroadcasts = "",
            bool completed = true)
        {
            Guid myContestId = await Assert.That(ContestId).IsNotNull();
            Contest retrievedContest = await ApiDriver.GetSingleContestAsync(myContestId);

            ChildBroadcast[] expectedChildBroadcasts = MarkdownParser.ParseTable(childBroadcasts, MapToChildBroadcast)
                .OrderBy(broadcast => broadcast.BroadcastId).ToArray();

            await Assert.That(retrievedContest.Completed).IsEqualTo(completed);

            await Assert.That(retrievedContest.ChildBroadcasts.OrderBy(broadcast => broadcast.BroadcastId))
                .IsEquivalentTo(expectedChildBroadcasts);
        }

        private ChildBroadcast MapToChildBroadcast(Dictionary<string, string> row)
        {
            ContestStage contestStage = Enum.Parse<ContestStage>(row["ContestStage"]);

            return new ChildBroadcast
            {
                ContestStage = contestStage,
                BroadcastId = Broadcasts.GetSingle(contestStage).Id,
                Completed = bool.Parse(row["Completed"])
            };
        }
    }
}

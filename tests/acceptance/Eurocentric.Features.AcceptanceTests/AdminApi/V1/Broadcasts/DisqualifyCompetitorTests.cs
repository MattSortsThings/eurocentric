using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Eurocentric.Features.AdminApi.V1.Broadcasts.AwardJuryPoints;
using Eurocentric.Features.AdminApi.V1.Broadcasts.AwardTelevotePoints;
using Eurocentric.Features.AdminApi.V1.Broadcasts.DisqualifyCompetitor;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed class DisqualifyCompetitorTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_disqualify_requested_competitor_scenario_1_of_3(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);

        await admin.Given_I_want_to_disqualify_the_competitor_from_my_broadcast_representing_the_country("AT");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_my_broadcast_should_now_match(completed: false,
            competitors: """
                         | RunningOrder | CountryCode | Finish |
                         |--------------|-------------|--------|
                         | 2            | BE          | 1      |
                         | 3            | CZ          | 2      |
                         """,
            juries: """
                    | CountryCode | PointsAwarded |
                    |-------------|---------------|
                    | AT          | false         |
                    | BE          | false         |
                    | CZ          | false         |
                    """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       |-------------|---------------|
                       | AT          | false         |
                       | BE          | false         |
                       | CZ          | false         |
                       """);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_disqualify_requested_competitor_scenario_2_of_3(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);

        await admin.Given_I_want_to_disqualify_the_competitor_from_my_broadcast_representing_the_country("BE");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_my_broadcast_should_now_match(completed: false,
            competitors: """
                         | RunningOrder | CountryCode | Finish |
                         |--------------|-------------|--------|
                         | 1            | AT          | 1      |
                         | 3            | CZ          | 2      |
                         """,
            juries: """
                    | CountryCode | PointsAwarded |
                    |-------------|---------------|
                    | AT          | false         |
                    | BE          | false         |
                    | CZ          | false         |
                    """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       |-------------|---------------|
                       | AT          | false         |
                       | BE          | false         |
                       | CZ          | false         |
                       """);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_disqualify_requested_competitor_scenario_3_of_3(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);

        await admin.Given_I_want_to_disqualify_the_competitor_from_my_broadcast_representing_the_country("CZ");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_my_broadcast_should_now_match(completed: false,
            competitors: """
                         | RunningOrder | CountryCode | Finish |
                         |--------------|-------------|--------|
                         | 1            | AT          | 1      |
                         | 2            | BE          | 2      |
                         """,
            juries: """
                    | CountryCode | PointsAwarded |
                    |-------------|---------------|
                    | AT          | false         |
                    | BE          | false         |
                    | CZ          | false         |
                    """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       |-------------|---------------|
                       | AT          | false         |
                       | BE          | false         |
                       | CZ          | false         |
                       """);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_non_existent_broadcast(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);
        await admin.Given_I_have_deleted_my_broadcast();

        await admin.Given_I_want_to_disqualify_a_competitor_from_my_deleted_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_404_NotFound();
        await admin.Then_the_response_problem_details_should_match(status: 404,
            title: "Broadcast not found",
            detail: "No broadcast exists with the provided broadcast ID.");
        await admin.Then_the_response_problem_details_extensions_should_include_my_deleted_broadcast_ID();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_broadcast_with_all_points_awarded(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);
        await admin.Given_I_have_awarded_all_the_jury_points_in_my_broadcast();
        await admin.Given_I_have_awarded_all_the_televote_points_in_my_broadcast();

        await admin.Given_I_want_to_disqualify_the_competitor_from_my_broadcast_representing_the_country("AT");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Broadcast running order locked",
            detail: "Broadcast running order cannot be modified once at least one jury or televote has awarded its points.");
        await admin.Then_my_broadcast_should_be_unchanged();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_broadcast_with_any_jury_points_awarded(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);
        await admin.Given_I_have_awarded_jury_points_in_my_broadcast_for_the_country("CZ");

        await admin.Given_I_want_to_disqualify_the_competitor_from_my_broadcast_representing_the_country("AT");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Broadcast running order locked",
            detail: "Broadcast running order cannot be modified once at least one jury or televote has awarded its points.");
        await admin.Then_my_broadcast_should_be_unchanged();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_broadcast_with_any_televote_points_awarded(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);
        await admin.Given_I_have_awarded_televote_points_in_my_broadcast_for_the_country("CZ");

        await admin.Given_I_want_to_disqualify_the_competitor_from_my_broadcast_representing_the_country("AT");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Broadcast running order locked",
            detail: "Broadcast running order cannot be modified once at least one jury or televote has awarded its points.");
        await admin.Then_my_broadcast_should_be_unchanged();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_competing_country_ID_matching_no_competitor(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);

        await admin.Given_I_want_to_disqualify_the_competitor_from_my_broadcast_representing_the_country("FI");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Disqualified competing country ID mismatch",
            detail: "Disqualified competing country ID has no matching competitor in broadcast.");
        await admin.Then_my_broadcast_should_be_unchanged();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithoutResponse(apiDriver)
    {
        private CountryIdLookup CountryIds { get; } = new();

        private Guid? ContestId { get; set; }

        private Broadcast? Broadcast { get; set; }

        private Guid? DeletedBroadcastId { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            List<Country> createdCountries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);
            CountryIds.Populate(createdCountries);
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            int contestYear = 0)
        {
            Contest createdContest = await ApiDriver.CreateSingleStockholmFormatContestAsync(cityName: TestDefaults.CityName,
                contestYear: contestYear,
                group1CountryIds: CountryIds.GetMultiple(group1CountryCodes),
                group2CountryIds: CountryIds.GetMultiple(group2CountryCodes));

            ContestId = createdContest.Id;
        }

        public async Task Given_I_have_awarded_all_the_televote_points_in_my_broadcast()
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();
            await ApiDriver.AwardAllTelevotePointsAsync(myBroadcast);

            Broadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcast.Id);
        }

        public async Task Given_I_have_awarded_all_the_jury_points_in_my_broadcast()
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();
            await ApiDriver.AwardAllJuryPointsAsync(myBroadcast);

            Broadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcast.Id);
        }

        public async Task Given_I_have_awarded_jury_points_in_my_broadcast_for_the_country(string countryCode)
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();
            Guid myBroadcastId = myBroadcast.Id;

            Guid votingCountryId = CountryIds.GetSingle(countryCode);

            Guid[] rankedCompetingCountryIds = myBroadcast.Competitors
                .Select(competitor => competitor.CompetingCountryId)
                .Where(countryId => countryId != votingCountryId)
                .ToArray();

            AwardJuryPointsRequest requestBody = new()
            {
                VotingCountryId = votingCountryId, RankedCompetingCountryIds = rankedCompetingCountryIds
            };

            await ApiDriver.AwardMultipleJuryPointsAsync(myBroadcastId, [requestBody]);

            Broadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcastId);
        }

        public async Task Given_I_have_awarded_televote_points_in_my_broadcast_for_the_country(string countryCode)
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();
            Guid myBroadcastId = myBroadcast.Id;

            Guid votingCountryId = CountryIds.GetSingle(countryCode);

            Guid[] rankedCompetingCountryIds = myBroadcast.Competitors
                .Select(competitor => competitor.CompetingCountryId)
                .Where(countryId => countryId != votingCountryId)
                .ToArray();

            AwardTelevotePointsRequest requestBody = new()
            {
                VotingCountryId = votingCountryId, RankedCompetingCountryIds = rankedCompetingCountryIds
            };

            await ApiDriver.AwardMultipleTelevotePointsAsync(myBroadcastId, [requestBody]);

            Broadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcastId);
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

            Broadcast = createdBroadcast;
        }

        public async Task Given_I_have_deleted_my_broadcast()
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();
            Guid myBroadcastId = myBroadcast.Id;

            await ApiDriver.DeleteSingleBroadcastAsync(myBroadcastId);

            Broadcast = null;
            DeletedBroadcastId = myBroadcastId;
        }

        public async Task Given_I_want_to_disqualify_the_competitor_from_my_broadcast_representing_the_country(
            string countryCode)
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();

            DisqualifyCompetitorRequest requestBody = new() { CompetingCountryId = CountryIds.GetSingle(countryCode) };

            Request = ApiDriver.RequestFactory.Broadcasts.DisqualifyCompetitor(myBroadcast.Id, requestBody);
        }

        public async Task Given_I_want_to_disqualify_a_competitor_from_my_deleted_broadcast()
        {
            Guid myDeletedBroadcastId = await Assert.That(DeletedBroadcastId).IsNotNull();

            DisqualifyCompetitorRequest requestBody = new() { CompetingCountryId = CountryIds.GetAll().First() };

            Request = ApiDriver.RequestFactory.Broadcasts.DisqualifyCompetitor(myDeletedBroadcastId, requestBody);
        }

        public async Task Then_my_broadcast_should_now_match(string? televotes = null,
            string? juries = null,
            string competitors = "",
            bool completed = true)
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();
            Broadcast myRetrievedBroadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcast.Id);

            Voter[] expectedTelevotes = MarkdownParser.ParseTable(televotes, MapRowToVoter)
                .OrderBy(voter => voter.VotingCountryId)
                .ToArray();

            Voter[] expectedJuries = MarkdownParser.ParseTable(juries, MapRowToVoter)
                .OrderBy(voter => voter.VotingCountryId)
                .ToArray();

            Competitor[] expectedCompetitors = MarkdownParser.ParseTable(competitors, MapRowToCompetitor)
                .OrderBy(competitor => competitor.CompetingCountryId)
                .ToArray();

            await Assert.That(myRetrievedBroadcast.Completed).IsEqualTo(completed);

            await Assert.That(myRetrievedBroadcast.Juries.OrderBy(voter => voter.VotingCountryId))
                .IsEquivalentTo(expectedJuries, CollectionOrdering.Matching);

            await Assert.That(myRetrievedBroadcast.Televotes.OrderBy(voter => voter.VotingCountryId))
                .IsEquivalentTo(expectedTelevotes, CollectionOrdering.Matching);

            await Assert.That(myRetrievedBroadcast.Competitors.OrderBy(competitor => competitor.CompetingCountryId))
                .IsEquivalentTo(expectedCompetitors, new CompetitorEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_my_broadcast_should_be_unchanged()
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();
            Broadcast myRetrievedBroadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcast.Id);

            await Assert.That(myRetrievedBroadcast).IsEqualTo(myBroadcast, new BroadcastEqualityComparer());
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_deleted_broadcast_ID()
        {
            ProblemDetails problemDetails = await Assert.That(ResponseProblemDetails).IsNotNull();
            Guid myDeletedBroadcastId = await Assert.That(DeletedBroadcastId).IsNotNull();

            await Assert.That(problemDetails).HasExtension("broadcastId", myDeletedBroadcastId);
        }

        private Voter MapRowToVoter(Dictionary<string, string> row) => new()
        {
            VotingCountryId = CountryIds.GetSingle(row["CountryCode"]), PointsAwarded = bool.Parse(row["PointsAwarded"])
        };

        private Competitor MapRowToCompetitor(Dictionary<string, string> row) => new()
        {
            RunningOrderPosition = int.Parse(row["RunningOrder"]),
            CompetingCountryId = CountryIds.GetSingle(row["CountryCode"]),
            TelevoteAwards = [],
            JuryAwards = [],
            FinishingPosition = int.Parse(row["Finish"])
        };
    }
}

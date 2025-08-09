using System.Text.RegularExpressions;
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
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed partial class AwardJuryPointsTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_award_points_for_jury_in_broadcast_scenario_1_of_4(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);

        await admin.Given_I_want_to_award_jury_points_in_my_broadcast(
            votingCountryCode: "AT",
            rankedCompetingCountryCodes: ["BE", "CZ"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_my_broadcast_should_now_match(
            completed: false,
            competitors: """
                         | RunningOrder | CountryCode | TelevoteAwards | JuryAwards | Finish |
                         |--------------|-------------|----------------|------------|--------|
                         | 1            | AT          | {}             | {}         | 3      |
                         | 2            | BE          | {}             | {AT:12}    | 1      |
                         | 3            | CZ          | {}             | {AT:10}    | 2      |
                         """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       |-------------|---------------|
                       | AT          | false         |
                       | BE          | false         |
                       | CZ          | false         |
                       """,
            juries: """
                    | CountryCode | PointsAwarded |
                    |-------------|---------------|
                    | AT          | true          |
                    | BE          | false         |
                    | CZ          | false         |
                    """);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_award_points_for_jury_in_broadcast_scenario_2_of_4(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);
        await admin.Given_I_have_awarded_jury_points_in_my_broadcast(
            """
            | VotingCountryCode | RankedCompetingCountryCodes |
            |-------------------|-----------------------------|
            | AT                | [BE,CZ]                     |
            """);

        await admin.Given_I_want_to_award_jury_points_in_my_broadcast(
            votingCountryCode: "BE",
            rankedCompetingCountryCodes: ["CZ", "AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_my_broadcast_should_now_match(
            completed: false,
            competitors: """
                         | RunningOrder | CountryCode | TelevoteAwards | JuryAwards    | Finish |
                         |--------------|-------------|----------------|---------------|--------|
                         | 1            | AT          | {}             | {BE:10}       | 3      |
                         | 2            | BE          | {}             | {AT:12}       | 2      |
                         | 3            | CZ          | {}             | {AT:10,BE:12} | 1      |
                         """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       |-------------|---------------|
                       | AT          | false         |
                       | BE          | false         |
                       | CZ          | false         |
                       """,
            juries: """
                    | CountryCode | PointsAwarded |
                    |-------------|---------------|
                    | AT          | true          |
                    | BE          | true          |
                    | CZ          | false         |
                    """);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_award_points_for_jury_in_broadcast_scenario_3_of_4(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);
        await admin.Given_I_have_awarded_jury_points_in_my_broadcast(
            """
            | VotingCountryCode | RankedCompetingCountryCodes |
            |-------------------|-----------------------------|
            | BE                | [CZ,AT]                     |
            | AT                | [BE,CZ]                     |
            """);

        await admin.Given_I_want_to_award_jury_points_in_my_broadcast(
            votingCountryCode: "CZ",
            rankedCompetingCountryCodes: ["AT", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_my_broadcast_should_now_match(
            completed: false,
            competitors: """
                         | RunningOrder | CountryCode | TelevoteAwards | JuryAwards    | Finish |
                         |--------------|-------------|----------------|---------------|--------|
                         | 1            | AT          | {}             | {BE:10,CZ:12} | 1      |
                         | 2            | BE          | {}             | {AT:12,CZ:10} | 2      |
                         | 3            | CZ          | {}             | {AT:10,BE:12} | 3      |
                         """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       |-------------|---------------|
                       | AT          | false         |
                       | BE          | false         |
                       | CZ          | false         |
                       """,
            juries: """
                    | CountryCode | PointsAwarded |
                    |-------------|---------------|
                    | AT          | true          |
                    | BE          | true          |
                    | CZ          | true          |
                    """);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_award_points_for_jury_in_broadcast_scenario_4_of_4(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);
        await admin.Given_I_have_awarded_televote_points_in_my_broadcast(
            """
            | VotingCountryCode | RankedCompetingCountryCodes |
            |-------------------|-----------------------------|
            | CZ                | [AT,BE]                     |
            | BE                | [CZ,AT]                     |
            | AT                | [BE,CZ]                     |
            """);
        await admin.Given_I_have_awarded_jury_points_in_my_broadcast(
            """
            | VotingCountryCode | RankedCompetingCountryCodes |
            |-------------------|-----------------------------|
            | BE                | [CZ,AT]                     |
            | AT                | [BE,CZ]                     |
            """);

        await admin.Given_I_want_to_award_jury_points_in_my_broadcast(
            votingCountryCode: "CZ",
            rankedCompetingCountryCodes: ["AT", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_my_broadcast_should_now_match(
            completed: true,
            competitors: """
                         | RunningOrder | CountryCode | TelevoteAwards | JuryAwards    | Finish |
                         |--------------|-------------|----------------|---------------|--------|
                         | 1            | AT          | {BE:10,CZ:12}  | {BE:10,CZ:12} | 1      |
                         | 2            | BE          | {AT:12,CZ:10}  | {AT:12,CZ:10} | 2      |
                         | 3            | CZ          | {AT:10,BE:12}  | {AT:10,BE:12} | 3      |
                         """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       |-------------|---------------|
                       | AT          | true          |
                       | BE          | true          |
                       | CZ          | true          |
                       """,
            juries: """
                    | CountryCode | PointsAwarded |
                    |-------------|---------------|
                    | AT          | true          |
                    | BE          | true          |
                    | CZ          | true          |
                    """);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_non_existent_broadcast_requested(string apiVersion)
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

        await admin.Given_I_want_to_award_jury_points_in_my_deleted_broadcast();

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
    public async Task Endpoint_should_fail_on_voting_country_ID_matching_no_jury(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);

        await admin.Given_I_want_to_award_jury_points_in_my_broadcast(
            votingCountryCode: "FI",
            rankedCompetingCountryCodes: ["AT", "BE", "CZ"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Jury voting country ID mismatch",
            detail: "Voting country ID must match a jury in the broadcast that has not yet awarded its points.");
        await admin.Then_my_broadcast_should_be_unchanged();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_voting_country_ID_matching_jury_with_points_awarded(string apiVersion)
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

        await admin.Given_I_want_to_award_jury_points_in_my_broadcast(
            votingCountryCode: "AT",
            rankedCompetingCountryCodes: ["BE", "CZ"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Jury voting country ID mismatch",
            detail: "Voting country ID must match a jury in the broadcast that has not yet awarded its points.");
        await admin.Then_my_broadcast_should_be_unchanged();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_ranked_competing_country_ID_matching_no_competitor(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);

        await admin.Given_I_want_to_award_jury_points_in_my_broadcast(
            votingCountryCode: "AT",
            rankedCompetingCountryCodes: ["BE", "CZ", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Ranked competing country IDs mismatch",
            detail: "Ranked competing country IDs must comprise every competing country ID in the broadcast, " +
                    "excluding the voting country ID, exactly once.");
        await admin.Then_my_broadcast_should_be_unchanged();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_ranked_competing_country_IDs_omitting_competitor(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);

        await admin.Given_I_want_to_award_jury_points_in_my_broadcast(
            votingCountryCode: "AT",
            rankedCompetingCountryCodes: ["BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Ranked competing country IDs mismatch",
            detail: "Ranked competing country IDs must comprise every competing country ID in the broadcast, " +
                    "excluding the voting country ID, exactly once.");
        await admin.Then_my_broadcast_should_be_unchanged();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_ranked_competing_country_IDs_including_voting_country_ID(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);

        await admin.Given_I_want_to_award_jury_points_in_my_broadcast(
            votingCountryCode: "AT",
            rankedCompetingCountryCodes: ["BE", "CZ", "AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Ranked competing country IDs mismatch",
            detail: "Ranked competing country IDs must comprise every competing country ID in the broadcast, " +
                    "excluding the voting country ID, exactly once.");
        await admin.Then_my_broadcast_should_be_unchanged();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_duplicate_ranked_competing_country_IDs(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);

        await admin.Given_I_want_to_award_jury_points_in_my_broadcast(
            votingCountryCode: "AT",
            rankedCompetingCountryCodes: ["BE", "CZ", "CZ"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Ranked competing country IDs mismatch",
            detail: "Ranked competing country IDs must comprise every competing country ID in the broadcast, " +
                    "excluding the voting country ID, exactly once.");
        await admin.Then_my_broadcast_should_be_unchanged();
    }

    private sealed partial class AdminActor(IApiDriver apiDriver) : AdminActorWithoutResponse(apiDriver)
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

        public async Task Given_I_have_awarded_jury_points_in_my_broadcast(string requests)
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();
            Guid myBroadcastId = myBroadcast.Id;

            AwardJuryPointsRequest[] requestBodies =
                MarkdownParser.ParseTable(requests, MapRowToAwardJuryPointsRequest).ToArray();

            await ApiDriver.AwardMultipleJuryPointsAsync(myBroadcastId, requestBodies);

            Broadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcastId);
        }

        public async Task Given_I_have_awarded_televote_points_in_my_broadcast(string requests)
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();
            Guid myBroadcastId = myBroadcast.Id;

            AwardTelevotePointsRequest[] requestBodies =
                MarkdownParser.ParseTable(requests, MapRowToAwardTelevotePointsRequest).ToArray();

            await ApiDriver.AwardMultipleTelevotePointsAsync(myBroadcastId, requestBodies);

            Broadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcastId);
        }

        public async Task Given_I_have_awarded_all_the_jury_points_in_my_broadcast()
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();
            Guid myBroadcastId = myBroadcast.Id;

            IEnumerable<AwardJuryPointsRequest> requestBodies = GenerateAllAwardJuryPointsRequests(myBroadcast);

            await ApiDriver.AwardMultipleJuryPointsAsync(myBroadcastId, requestBodies);

            Broadcast = await ApiDriver.GetSingleBroadcastAsync(myBroadcastId);
        }

        public async Task Given_I_want_to_award_jury_points_in_my_broadcast(string[] rankedCompetingCountryCodes = null!,
            string votingCountryCode = "")
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();

            AwardJuryPointsRequest requestBody = new()
            {
                VotingCountryId = CountryIds.GetSingle(votingCountryCode),
                RankedCompetingCountryIds = CountryIds.GetMultiple(rankedCompetingCountryCodes)
            };

            Request = ApiDriver.RequestFactory.Broadcasts.AwardJuryPoints(myBroadcast.Id, requestBody);
        }

        public async Task Given_I_want_to_award_jury_points_in_my_deleted_broadcast()
        {
            Guid myDeletedBroadcastId = await Assert.That(DeletedBroadcastId).IsNotNull();
            Guid[] countryIds = CountryIds.GetAll();

            AwardJuryPointsRequest requestBody = new()
            {
                VotingCountryId = countryIds[0], RankedCompetingCountryIds = countryIds.Skip(0).ToArray()
            };

            Request = ApiDriver.RequestFactory.Broadcasts.AwardJuryPoints(myDeletedBroadcastId, requestBody);
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

        private AwardJuryPointsRequest MapRowToAwardJuryPointsRequest(Dictionary<string, string> row)
        {
            string[] rankedCompetingCountryCodes = row["RankedCompetingCountryCodes"].TrimStart('[')
                .TrimEnd(']')
                .Split(',');

            return new AwardJuryPointsRequest
            {
                VotingCountryId = CountryIds.GetSingle(row["VotingCountryCode"]),
                RankedCompetingCountryIds = CountryIds.GetMultiple(rankedCompetingCountryCodes)
            };
        }

        private AwardTelevotePointsRequest MapRowToAwardTelevotePointsRequest(Dictionary<string, string> row)
        {
            string[] rankedCompetingCountryCodes = row["RankedCompetingCountryCodes"].TrimStart('[')
                .TrimEnd(']')
                .Split(',');

            return new AwardTelevotePointsRequest
            {
                VotingCountryId = CountryIds.GetSingle(row["VotingCountryCode"]),
                RankedCompetingCountryIds = CountryIds.GetMultiple(rankedCompetingCountryCodes)
            };
        }

        private Voter MapRowToVoter(Dictionary<string, string> row) => new()
        {
            VotingCountryId = CountryIds.GetSingle(row["CountryCode"]), PointsAwarded = bool.Parse(row["PointsAwarded"])
        };

        private Competitor MapRowToCompetitor(Dictionary<string, string> row) => new()
        {
            RunningOrderPosition = int.Parse(row["RunningOrder"]),
            CompetingCountryId = CountryIds.GetSingle(row["CountryCode"]),
            TelevoteAwards = MapCellToAwards(row["TelevoteAwards"]),
            JuryAwards = MapCellToAwards(row["JuryAwards"]),
            FinishingPosition = int.Parse(row["Finish"])
        };

        private Award[] MapCellToAwards(string cell)
        {
            MatchCollection matches = AwardRegex().Matches(cell);

            return matches.Select(match => new Award
            {
                VotingCountryId = CountryIds.GetSingle(match.Groups["VotingCountryCode"].Value),
                PointsValue = int.Parse(match.Groups["PointsValue"].Value)
            }).ToArray();
        }

        [GeneratedRegex("(?<VotingCountryCode>[A-Z]{2}):(?<PointsValue>[0-9]+)", RegexOptions.Compiled)]
        private static partial Regex AwardRegex();

        private static IEnumerable<AwardJuryPointsRequest> GenerateAllAwardJuryPointsRequests(Broadcast broadcast)
        {
            Guid[] allCompetingCountryIds = broadcast.Competitors.Select(competitor => competitor.CompetingCountryId).ToArray();

            foreach (Voter televote in broadcast.Juries.Where(voter => !voter.PointsAwarded))
            {
                Guid votingCountryId = televote.VotingCountryId;

                yield return new AwardJuryPointsRequest
                {
                    VotingCountryId = votingCountryId,
                    RankedCompetingCountryIds = allCompetingCountryIds.Where(id => id != votingCountryId).ToArray()
                };
            }
        }
    }
}

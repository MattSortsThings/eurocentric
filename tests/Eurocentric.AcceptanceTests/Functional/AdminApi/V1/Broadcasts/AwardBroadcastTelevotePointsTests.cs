using System.Text.RegularExpressions;
using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Broadcasts;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Broadcasts;

public sealed partial class AwardBroadcastTelevotePointsTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_award_televote_points_in_jury_and_televote_broadcast_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2025,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "AT",
            rankedCompetingCountries: ["BE", "CZ"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);
        await admin.Then_my_broadcast_should_now_match(
            completed: false,
            competitors: """
            | RunningOrderSpot | CompetingCountry | TelevoteAwards | JuryAwards | FinishingPosition |
            |------------------|------------------|----------------|------------|-------------------|
            | 1                | AT               | {}             | {}         | 3                 |
            | 2                | BE               | {AT:12}        | {}         | 1                 |
            | 3                | CZ               | {AT:10}        | {}         | 2                 |
            """,
            televotes: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | true          |
            | BE            | false         |
            | CZ            | false         |
            """,
            juries: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | false         |
            | BE            | false         |
            | CZ            | false         |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_award_televote_points_in_jury_and_televote_broadcast_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2025,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );
        await admin.Given_I_have_awarded_televote_points_in_my_broadcast(
            """
            | VotingCountry | RankedCompetingCountries |
            |---------------|--------------------------|
            | AT            | [BE,CZ]                  |
            """
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "BE",
            rankedCompetingCountries: ["CZ", "AT"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);
        await admin.Then_my_broadcast_should_now_match(
            completed: false,
            competitors: """
            | RunningOrderSpot | CompetingCountry | TelevoteAwards | JuryAwards | FinishingPosition |
            |------------------|------------------|----------------|------------|-------------------|
            | 1                | AT               | {BE:10}        | {}         | 3                 |
            | 2                | BE               | {AT:12}        | {}         | 2                 |
            | 3                | CZ               | {AT:10,BE:12}  | {}         | 1                 |
            """,
            televotes: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | true          |
            | BE            | true          |
            | CZ            | false         |
            """,
            juries: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | false         |
            | BE            | false         |
            | CZ            | false         |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_award_televote_points_in_jury_and_televote_broadcast_scenario_3(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2025,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );
        await admin.Given_I_have_awarded_televote_points_in_my_broadcast(
            """
            | VotingCountry | RankedCompetingCountries |
            |---------------|--------------------------|
            | BE            | [CZ,AT]                  |
            | AT            | [BE,CZ]                  |
            """
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "CZ",
            rankedCompetingCountries: ["AT", "BE"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);
        await admin.Then_my_broadcast_should_now_match(
            completed: false,
            competitors: """
            | RunningOrderSpot | CompetingCountry | TelevoteAwards | JuryAwards | FinishingPosition |
            |------------------|------------------|----------------|------------|-------------------|
            | 1                | AT               | {BE:10,CZ:12}  | {}         | 1                 |
            | 2                | BE               | {AT:12,CZ:10}  | {}         | 2                 |
            | 3                | CZ               | {AT:10,BE:12}  | {}         | 3                 |
            """,
            televotes: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | true          |
            | BE            | true          |
            | CZ            | true          |
            """,
            juries: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | false         |
            | BE            | false         |
            | CZ            | false         |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_award_televote_points_in_televote_only_broadcast_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "AT",
            rankedCompetingCountries: ["BE", "CZ"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);
        await admin.Then_my_broadcast_should_now_match(
            completed: false,
            competitors: """
            | RunningOrderSpot | CompetingCountry | TelevoteAwards | JuryAwards | FinishingPosition |
            |------------------|------------------|----------------|------------|-------------------|
            | 1                | AT               | {}             | {}         | 3                 |
            | 2                | BE               | {AT:12}        | {}         | 1                 |
            | 3                | CZ               | {AT:10}        | {}         | 2                 |
            """,
            televotes: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | true          |
            | BE            | false         |
            | CZ            | false         |
            | XX            | false         |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_award_televote_points_in_televote_only_broadcast_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );
        await admin.Given_I_have_awarded_televote_points_in_my_broadcast(
            """
            | VotingCountry | RankedCompetingCountries |
            |---------------|--------------------------|
            | AT            | [BE,CZ]                  |
            """
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "BE",
            rankedCompetingCountries: ["CZ", "AT"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);
        await admin.Then_my_broadcast_should_now_match(
            completed: false,
            competitors: """
            | RunningOrderSpot | CompetingCountry | TelevoteAwards | JuryAwards | FinishingPosition |
            |------------------|------------------|----------------|------------|-------------------|
            | 1                | AT               | {BE:10}        | {}         | 3                 |
            | 2                | BE               | {AT:12}        | {}         | 2                 |
            | 3                | CZ               | {AT:10,BE:12}  | {}         | 1                 |
            """,
            televotes: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | true          |
            | BE            | true          |
            | CZ            | false         |
            | XX            | false         |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_award_televote_points_in_televote_only_broadcast_scenario_3(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );
        await admin.Given_I_have_awarded_televote_points_in_my_broadcast(
            """
            | VotingCountry | RankedCompetingCountries |
            |---------------|--------------------------|
            | BE            | [CZ,AT]                  |
            | AT            | [BE,CZ]                  |
            """
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "CZ",
            rankedCompetingCountries: ["AT", "BE"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);
        await admin.Then_my_broadcast_should_now_match(
            completed: false,
            competitors: """
            | RunningOrderSpot | CompetingCountry | TelevoteAwards | JuryAwards | FinishingPosition |
            |------------------|------------------|----------------|------------|-------------------|
            | 1                | AT               | {BE:10,CZ:12}  | {}         | 1                 |
            | 2                | BE               | {AT:12,CZ:10}  | {}         | 2                 |
            | 3                | CZ               | {AT:10,BE:12}  | {}         | 3                 |
            """,
            televotes: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | true          |
            | BE            | true          |
            | CZ            | true          |
            | XX            | false         |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_award_televote_points_in_televote_only_broadcast_scenario_4(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );
        await admin.Given_I_have_awarded_televote_points_in_my_broadcast(
            """
            | VotingCountry | RankedCompetingCountries |
            |---------------|--------------------------|
            | CZ            | [AT,BE]                  |
            | BE            | [CZ,AT]                  |
            | AT            | [BE,CZ]                  |
            """
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "XX",
            rankedCompetingCountries: ["CZ", "AT", "BE"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);
        await admin.Then_my_broadcast_should_now_match(
            completed: true,
            competitors: """
            | RunningOrderSpot | CompetingCountry | TelevoteAwards      | JuryAwards | FinishingPosition |
            |------------------|------------------|---------------------|------------|-------------------|
            | 1                | AT               | {BE:10,CZ:12,XX:10} | {}         | 2                 |
            | 2                | BE               | {AT:12,CZ:10,XX:8}  | {}         | 3                 |
            | 3                | CZ               | {AT:10,BE:12,XX:12} | {}         | 1                 |
            """,
            televotes: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | true          |
            | BE            | true          |
            | CZ            | true          |
            | XX            | true          |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_broadcast_not_found(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "XX",
            rankedCompetingCountries: ["CZ", "AT", "BE"]
        );
        await admin.Given_I_have_deleted_my_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(404);
        await admin.Then_the_response_problem_details_should_match(
            status: 404,
            title: "Broadcast not found",
            detail: "The requested broadcast does not exist."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_the_deleted_broadcast_ID();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_televote_voting_country_conflict_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB");
        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2025,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "GB",
            rankedCompetingCountries: ["AT", "BE", "CZ"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Televote voting country conflict",
            detail: "The requested broadcast has no televote that may award points and has the provided country ID."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country("GB");
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_televote_voting_country_conflict_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2025,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "FI",
            rankedCompetingCountries: ["AT", "BE", "CZ"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Televote voting country conflict",
            detail: "The requested broadcast has no televote that may award points and has the provided country ID."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country("FI");
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_televote_voting_country_conflict_scenario_3(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2025,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );
        await admin.Given_I_have_awarded_televote_points_in_my_broadcast(
            """
            | VotingCountry | RankedCompetingCountries |
            |---------------|--------------------------|
            | CZ            | [AT,BE]                  |
            """
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "CZ",
            rankedCompetingCountries: ["BE", "AT"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Televote voting country conflict",
            detail: "The requested broadcast has no televote that may award points and has the provided country ID."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country("CZ");
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "XX",
            rankedCompetingCountries: []
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Ranked competing countries conflict",
            detail: "Ranked competing countries must comprise every competing country in the broadcast, "
                + "excluding the voting country, without duplication."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "XX",
            rankedCompetingCountries: ["AT"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Ranked competing countries conflict",
            detail: "Ranked competing countries must comprise every competing country in the broadcast, "
                + "excluding the voting country, without duplication."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_3(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "XX",
            rankedCompetingCountries: ["AT", "BE"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Ranked competing countries conflict",
            detail: "Ranked competing countries must comprise every competing country in the broadcast, "
                + "excluding the voting country, without duplication."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_4(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "XX",
            rankedCompetingCountries: ["AT", "BE", "CZ", "AT"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Ranked competing countries conflict",
            detail: "Ranked competing countries must comprise every competing country in the broadcast, "
                + "excluding the voting country, without duplication."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_5(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "XX",
            rankedCompetingCountries: ["AT", "BE", "CZ", "FI"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Ranked competing countries conflict",
            detail: "Ranked competing countries must comprise every competing country in the broadcast, "
                + "excluding the voting country, without duplication."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_6(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "AT",
            rankedCompetingCountries: []
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Ranked competing countries conflict",
            detail: "Ranked competing countries must comprise every competing country in the broadcast, "
                + "excluding the voting country, without duplication."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_7(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "AT",
            rankedCompetingCountries: ["BE"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Ranked competing countries conflict",
            detail: "Ranked competing countries must comprise every competing country in the broadcast, "
                + "excluding the voting country, without duplication."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_8(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "AT",
            rankedCompetingCountries: ["BE", "CZ", "AT"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Ranked competing countries conflict",
            detail: "Ranked competing countries must comprise every competing country in the broadcast, "
                + "excluding the voting country, without duplication."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_9(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "AT",
            rankedCompetingCountries: ["BE", "CZ", "CZ"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Ranked competing countries conflict",
            detail: "Ranked competing countries must comprise every competing country in the broadcast, "
                + "excluding the voting country, without duplication."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_10(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "AT",
            rankedCompetingCountries: ["BE", "CZ", "XX"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Ranked competing countries conflict",
            detail: "Ranked competing countries must comprise every competing country in the broadcast, "
                + "excluding the voting country, without duplication."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_11(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE", "CZ"]
        );

        await admin.Given_I_want_to_award_televote_points_in_my_broadcast(
            votingCountry: "AT",
            rankedCompetingCountries: ["BE", "CZ", "FI"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Ranked competing countries conflict",
            detail: "Ranked competing countries must comprise every competing country in the broadcast, "
                + "excluding the voting country, without duplication."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        await admin.Then_my_broadcast_should_not_be_updated();
    }

    private sealed partial class Admin(AdminKernel kernel) : AdminActor
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private CountryIdLookup ExistingCountryIds { get; } = new();

        private Guid? ExistingContestId { get; set; }

        private Broadcast? ExistingBroadcast { get; set; }

        private Guid? DeletedBroadcastId { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            ExistingCountryIds.EnsureCapacity(countryCodes.Length);

            await foreach (Country country in Kernel.CreateMultipleCountriesAsync(countryCodes))
            {
                ExistingCountryIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Liverpool_rules_contest(
            string globalTelevoteCountry = "",
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            int contestYear = 0
        )
        {
            Contest contest = await Kernel.CreateALiverpoolRulesContestAsync(
                contestYear: contestYear,
                globalTelevoteCountryId: ExistingCountryIds.GetId(globalTelevoteCountry),
                semiFinal1CountryIds: ExistingCountryIds.MapToGuids(semiFinal1Countries),
                semiFinal2CountryIds: ExistingCountryIds.MapToGuids(semiFinal2Countries)
            );

            ExistingContestId = contest.Id;
        }

        public async Task Given_I_have_created_a_Stockholm_rules_contest(
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            int contestYear = 0
        )
        {
            Contest contest = await Kernel.CreateAStockholmRulesContestAsync(
                contestYear: contestYear,
                semiFinal1CountryIds: ExistingCountryIds.MapToGuids(semiFinal1Countries),
                semiFinal2CountryIds: ExistingCountryIds.MapToGuids(semiFinal2Countries)
            );

            ExistingContestId = contest.Id;
        }

        public async Task Given_I_have_created_the_SemiFinal1_broadcast_for_my_contest(
            string?[] competingCountries = null!,
            string broadcastDate = ""
        )
        {
            Guid contestId = await Assert.That(ExistingContestId).IsNotNull();

            ExistingBroadcast = await Kernel.CreateABroadcastAsync(
                contestId: contestId,
                broadcastDate: DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                contestStage: ContestStage.SemiFinal1,
                competingCountryIds: ExistingCountryIds.MapToNullableGuids(competingCountries)
            );
        }

        public async Task Given_I_have_awarded_televote_points_in_my_broadcast(string requests)
        {
            Guid broadcastId = await Assert.That(ExistingBroadcast?.Id).IsNotNull();
            AwardBroadcastTelevotePointsRequest[] requestBodies = MarkdownParser.ParseTable(requests, MapToRequestBody);

            await Kernel.AwardBroadcastTelevotePointsAsync(broadcastId, requestBodies);

            ExistingBroadcast = await Kernel.GetABroadcastAsync(broadcastId);
        }

        public async Task Given_I_have_deleted_my_broadcast()
        {
            Guid broadcastId = await Assert.That(ExistingBroadcast?.Id).IsNotNull();

            await Kernel.DeleteABroadcastAsync(broadcastId);

            ExistingBroadcast = null;
            DeletedBroadcastId = broadcastId;
        }

        public async Task Given_I_want_to_award_televote_points_in_my_broadcast(
            string[] rankedCompetingCountries = null!,
            string votingCountry = ""
        )
        {
            Guid broadcastId = await Assert.That(ExistingBroadcast?.Id).IsNotNull();

            AwardBroadcastTelevotePointsRequest requestBody = new()
            {
                VotingCountryId = ExistingCountryIds.GetId(votingCountry),
                RankedCompetingCountryIds = ExistingCountryIds.MapToGuids(rankedCompetingCountries),
            };

            Request = Kernel.Requests.Broadcasts.AwardBroadcastTelevotePoints(broadcastId, requestBody);
        }

        public async Task Then_my_broadcast_should_now_match(
            string? televotes = null,
            string? juries = null,
            string competitors = "",
            bool completed = true
        )
        {
            Guid broadcastId = await Assert.That(ExistingBroadcast?.Id).IsNotNull();

            Competitor[] expectedCompetitors = MarkdownParser.ParseTable(competitors, MapToCompetitor);
            Jury[] expectedJuries = MarkdownParser.ParseTable(juries, MapToJury);
            Televote[] expectedTelevotes = MarkdownParser.ParseTable(televotes, MapToTelevote);

            Broadcast retrievedBroadcast = await Kernel.GetABroadcastAsync(broadcastId);

            await Assert
                .That(retrievedBroadcast)
                .HasProperty(broadcast => broadcast.Completed, completed)
                .And.Member(
                    broadcast => broadcast.Competitors,
                    collection => collection.IsEquivalentTo(expectedCompetitors, new CompetitorEqualityComparer())
                )
                .And.Member(broadcast => broadcast.Juries, collection => collection.IsEquivalentTo(expectedJuries))
                .And.Member(
                    broadcast => broadcast.Televotes,
                    collection => collection.IsEquivalentTo(expectedTelevotes)
                );
        }

        public async Task Then_my_broadcast_should_not_be_updated()
        {
            Broadcast existingBroadcast = await Assert.That(ExistingBroadcast).IsNotNull();
            Broadcast retrievedBroadcast = await Kernel.GetABroadcastAsync(existingBroadcast.Id);

            await Assert.That(retrievedBroadcast).IsEqualTo(existingBroadcast, new BroadcastEqualityComparer());
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_broadcast_ID()
        {
            Guid broadcastId = await Assert.That(ExistingBroadcast?.Id).IsNotNull();

            await Assert.That(FailureResponse?.Data).HasExtension("broadcastId", broadcastId);
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_deleted_broadcast_ID()
        {
            Guid deletedBroadcastId = await Assert.That(DeletedBroadcastId).IsNotNull();

            await Assert.That(FailureResponse?.Data).HasExtension("broadcastId", deletedBroadcastId);
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country(
            string country
        )
        {
            Guid countryId = ExistingCountryIds.GetId(country);

            await Assert.That(FailureResponse?.Data).HasExtension("countryId", countryId);
        }

        private AwardBroadcastTelevotePointsRequest MapToRequestBody(Dictionary<string, string> row)
        {
            return new AwardBroadcastTelevotePointsRequest
            {
                VotingCountryId = ExistingCountryIds.GetId(row["VotingCountry"]),
                RankedCompetingCountryIds = MapCellToCountryIds(row["RankedCompetingCountries"]),
            };
        }

        private Competitor MapToCompetitor(Dictionary<string, string> row)
        {
            return new Competitor
            {
                RunningOrderSpot = int.Parse(row["RunningOrderSpot"]),
                CompetingCountryId = ExistingCountryIds.GetId(row["CompetingCountry"]),
                FinishingPosition = int.Parse(row["FinishingPosition"]),
                JuryAwards = MapCellToJuryAwards(row["JuryAwards"]),
                TelevoteAwards = MapCellToTelevoteAwards(row["TelevoteAwards"]),
            };
        }

        private Jury MapToJury(Dictionary<string, string> row)
        {
            return new Jury
            {
                VotingCountryId = ExistingCountryIds.GetId(row["VotingCountry"]),
                PointsAwarded = bool.Parse(row["PointsAwarded"]),
            };
        }

        private Televote MapToTelevote(Dictionary<string, string> row)
        {
            return new Televote
            {
                VotingCountryId = ExistingCountryIds.GetId(row["VotingCountry"]),
                PointsAwarded = bool.Parse(row["PointsAwarded"]),
            };
        }

        private JuryAward[] MapCellToJuryAwards(string cell)
        {
            MatchCollection matches = AwardRegex().Matches(cell);

            return matches
                .Select(match => new JuryAward
                {
                    VotingCountryId = ExistingCountryIds.GetId(match.Groups["VotingCountry"].Value),
                    PointsValue = int.Parse(match.Groups["PointsValue"].Value),
                })
                .ToArray();
        }

        private TelevoteAward[] MapCellToTelevoteAwards(string cell)
        {
            MatchCollection matches = AwardRegex().Matches(cell);

            return matches
                .Select(match => new TelevoteAward
                {
                    VotingCountryId = ExistingCountryIds.GetId(match.Groups["VotingCountry"].Value),
                    PointsValue = int.Parse(match.Groups["PointsValue"].Value),
                })
                .ToArray();
        }

        private Guid[] MapCellToCountryIds(string cell) =>
            ExistingCountryIds.MapToGuids(cell.TrimStart('[').TrimEnd(']').Split(','));

        [GeneratedRegex(@"(?<VotingCountry>[A-Z]{2}):(?<PointsValue>[0-9]+)", RegexOptions.Compiled)]
        private static partial Regex AwardRegex();
    }
}

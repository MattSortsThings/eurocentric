using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests;

public sealed partial class CreateContestBroadcastTests
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_create_and_return_Liverpool_rules_SemiFinal1_broadcast(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            contestStage: "SemiFinal1",
            competingCountries: ["CZ", "AT", "DK"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_response_headers_should_include_the_created_broadcast_location_for_API_version(apiVersion);
        await admin.Then_the_created_broadcast_should_be_the_only_existing_broadcast();
        await admin.Then_the_created_broadcast_should_reference_my_existing_contest_as_its_parent_contest();
        await admin.Then_the_created_broadcast_should_match(
            broadcastDate: "2025-05-01",
            contestStage: "SemiFinal1",
            completed: false,
            competitors: """
            | RunningOrderSpot | CompetingCountry | FinishingPosition |
            |------------------|------------------|-------------------|
            | 1                | CZ               | 1                 |
            | 2                | AT               | 2                 |
            | 3                | DK               | 3                 |
            """,
            televotes: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | false         |
            | BE            | false         |
            | CZ            | false         |
            | DK            | false         |
            | XX            | false         |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_create_and_return_Liverpool_rules_SemiFinal2_broadcast(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            broadcastDate: "2025-05-02",
            contestStage: "SemiFinal2",
            competingCountries: ["EE", "HR", "FI"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_response_headers_should_include_the_created_broadcast_location_for_API_version(apiVersion);
        await admin.Then_the_created_broadcast_should_be_the_only_existing_broadcast();
        await admin.Then_the_created_broadcast_should_reference_my_existing_contest_as_its_parent_contest();
        await admin.Then_the_created_broadcast_should_match(
            broadcastDate: "2025-05-02",
            contestStage: "SemiFinal2",
            completed: false,
            competitors: """
            | RunningOrderSpot | CompetingCountry | FinishingPosition |
            |------------------|------------------|-------------------|
            | 1                | EE               | 1                 |
            | 2                | HR               | 2                 |
            | 3                | FI               | 3                 |
            """,
            televotes: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | EE            | false         |
            | FI            | false         |
            | GB            | false         |
            | HR            | false         |
            | XX            | false         |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_create_and_return_Liverpool_rules_GrandFinal_broadcast(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            broadcastDate: "2025-05-03",
            contestStage: "GrandFinal",
            competingCountries: ["AT", "HR", "CZ", "EE", "GB"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_response_headers_should_include_the_created_broadcast_location_for_API_version(apiVersion);
        await admin.Then_the_created_broadcast_should_be_the_only_existing_broadcast();
        await admin.Then_the_created_broadcast_should_reference_my_existing_contest_as_its_parent_contest();
        await admin.Then_the_created_broadcast_should_match(
            broadcastDate: "2025-05-03",
            contestStage: "GrandFinal",
            completed: false,
            competitors: """
            | RunningOrderSpot | CompetingCountry | FinishingPosition |
            |------------------|------------------|-------------------|
            | 1                | AT               | 1                 |
            | 2                | HR               | 2                 |
            | 3                | CZ               | 3                 |
            | 4                | EE               | 4                 |
            | 5                | GB               | 5                 |
            """,
            juries: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | false         |
            | BE            | false         |
            | CZ            | false         |
            | DK            | false         |
            | EE            | false         |
            | FI            | false         |
            | GB            | false         |
            | HR            | false         |
            """,
            televotes: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | false         |
            | BE            | false         |
            | CZ            | false         |
            | DK            | false         |
            | EE            | false         |
            | FI            | false         |
            | GB            | false         |
            | HR            | false         |
            | XX            | false         |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_create_and_return_Liverpool_rules_broadcast_with_vacant_running_order_spot(
        string apiVersion
    )
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            broadcastDate: "2025-05-03",
            contestStage: "GrandFinal",
            competingCountries: ["AT", "HR", "CZ", null, "EE", "GB"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_response_headers_should_include_the_created_broadcast_location_for_API_version(apiVersion);
        await admin.Then_the_created_broadcast_should_be_the_only_existing_broadcast();
        await admin.Then_the_created_broadcast_should_reference_my_existing_contest_as_its_parent_contest();
        await admin.Then_the_created_broadcast_should_match(
            broadcastDate: "2025-05-03",
            contestStage: "GrandFinal",
            completed: false,
            competitors: """
            | RunningOrderSpot | CompetingCountry | FinishingPosition |
            |------------------|------------------|-------------------|
            | 1                | AT               | 1                 |
            | 2                | HR               | 2                 |
            | 3                | CZ               | 3                 |
            | 5                | EE               | 4                 |
            | 6                | GB               | 5                 |
            """,
            juries: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | false         |
            | BE            | false         |
            | CZ            | false         |
            | DK            | false         |
            | EE            | false         |
            | FI            | false         |
            | GB            | false         |
            | HR            | false         |
            """,
            televotes: """
            | VotingCountry | PointsAwarded |
            |---------------|---------------|
            | AT            | false         |
            | BE            | false         |
            | CZ            | false         |
            | DK            | false         |
            | EE            | false         |
            | FI            | false         |
            | GB            | false         |
            | HR            | false         |
            | XX            | false         |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_contest_not_found(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            contestStage: "SemiFinal1",
            competingCountries: ["AT", "BE"]
        );
        await admin.Given_I_have_deleted_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(404);
        await admin.Then_the_response_problem_details_should_match(
            status: 404,
            title: "Contest not found",
            detail: "The requested contest does not exist."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_the_deleted_contest_ID();
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_broadcast_date_conflict(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            broadcastDate: "2025-01-01",
            contestStage: "SemiFinal1",
            competingCountries: ["AT", "BE"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            broadcastDate: "2025-01-01",
            contestStage: "SemiFinal2",
            competingCountries: ["EE", "FI"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Broadcast date conflict",
            detail: "A broadcast already exists with the provided broadcast date."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(
            key: "broadcastDate",
            value: "2025-01-01"
        );
        await admin.Then_my_existing_broadcast_should_be_the_only_existing_broadcast();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_parent_contest_child_broadcasts_conflict(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-01-01",
            competingCountries: ["AT", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-31",
            competingCountries: ["EE", "DK"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Parent contest child broadcasts conflict",
            detail: "The requested contest already has a child broadcast with the provided contest stage."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(
            key: "contestStage",
            value: "GrandFinal"
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_contest_ID();
        await admin.Then_my_existing_broadcast_should_be_the_only_existing_broadcast();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_parent_contest_year_conflict(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            broadcastDate: "2016-01-01",
            contestStage: "GrandFinal",
            competingCountries: ["EE", "DK"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Parent contest year conflict",
            detail: "The requested contest's year does not match the provided broadcast date."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(
            key: "broadcastDate",
            value: "2016-01-01"
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_contest_ID();
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_parent_contest_participants_conflict_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            competingCountries: ["AT", "IT"],
            broadcastDate: "2025-01-01"
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Parent contest participants conflict",
            detail: "The requested contest has no participant with the provided country ID "
                + "eligible to compete in the provided contest stage."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(
            key: "contestStage",
            value: "SemiFinal1"
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_contest_ID();
        await admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country("IT");
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_parent_contest_participants_conflict_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            competingCountries: ["EE", "IT"],
            broadcastDate: "2025-01-01"
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Parent contest participants conflict",
            detail: "The requested contest has no participant with the provided country ID "
                + "eligible to compete in the provided contest stage."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(
            key: "contestStage",
            value: "SemiFinal2"
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_contest_ID();
        await admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country("IT");
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_parent_contest_participants_conflict_scenario_3(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            competingCountries: ["AT", "IT"],
            broadcastDate: "2025-01-01"
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Parent contest participants conflict",
            detail: "The requested contest has no participant with the provided country ID "
                + "eligible to compete in the provided contest stage."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(
            key: "contestStage",
            value: "GrandFinal"
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_contest_ID();
        await admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country("IT");
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_parent_contest_participants_conflict_scenario_4(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            competingCountries: ["AT", "EE"],
            broadcastDate: "2025-01-01"
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Parent contest participants conflict",
            detail: "The requested contest has no participant with the provided country ID "
                + "eligible to compete in the provided contest stage."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(
            key: "contestStage",
            value: "SemiFinal1"
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_contest_ID();
        await admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country("EE");
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_parent_contest_participants_conflict_scenario_5(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            competingCountries: ["EE", "AT"],
            broadcastDate: "2025-01-01"
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Parent contest participants conflict",
            detail: "The requested contest has no participant with the provided country ID "
                + "eligible to compete in the provided contest stage."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(
            key: "contestStage",
            value: "SemiFinal2"
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_contest_ID();
        await admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country("AT");
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_parent_contest_participants_conflict_scenario_6(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            competingCountries: ["AT", "XX"],
            broadcastDate: "2025-01-01"
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Parent contest participants conflict",
            detail: "The requested contest has no participant with the provided country ID "
                + "eligible to compete in the provided contest stage."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(
            key: "contestStage",
            value: "SemiFinal1"
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_contest_ID();
        await admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country("XX");
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_parent_contest_participants_conflict_scenario_7(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            competingCountries: ["EE", "XX"],
            broadcastDate: "2025-01-01"
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Parent contest participants conflict",
            detail: "The requested contest has no participant with the provided country ID "
                + "eligible to compete in the provided contest stage."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(
            key: "contestStage",
            value: "SemiFinal2"
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_contest_ID();
        await admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country("XX");
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_parent_contest_participants_conflict_scenario_8(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            competingCountries: ["AT", "XX"],
            broadcastDate: "2025-01-01"
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Parent contest participants conflict",
            detail: "The requested contest has no participant with the provided country ID "
                + "eligible to compete in the provided contest stage."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(
            key: "contestStage",
            value: "GrandFinal"
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_contest_ID();
        await admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country("XX");
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_broadcast_date_value(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            broadcastDate: "1066-10-14",
            contestStage: "GrandFinal",
            competingCountries: ["AT", "EE"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal broadcast date value",
            detail: "Broadcast date value must be a date with a year between 2016 and 2050."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(
            key: "broadcastDate",
            value: "1066-10-14"
        );
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_competing_countries(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            ["AT", "EE", "AT"],
            broadcastDate: "2025-01-01",
            contestStage: "GrandFinal"
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal competing countries",
            detail: "Each competitor in a broadcast must reference a different country."
        );
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_competitors_count_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest([], "GrandFinal", "2025-01-01");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal competitors count",
            detail: "A broadcast must have at least 2 competitors."
        );
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_competitors_count_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest([null, null], "GrandFinal", "2025-01-01");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal competitors count",
            detail: "A broadcast must have at least 2 competitors."
        );
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_competitors_count_scenario_3(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(["AT"], "GrandFinal", "2025-01-01");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal competitors count",
            detail: "A broadcast must have at least 2 competitors."
        );
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_competitors_count_scenario_4(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(["AT", null], "GrandFinal", "2025-01-01");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal competitors count",
            detail: "A broadcast must have at least 2 competitors."
        );
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_competitors_count_scenario_5(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );

        await admin.Given_I_want_to_create_a_broadcast_for_my_contest(
            [null, "AT", null, null],
            "GrandFinal",
            "2025-01-01"
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal competitors count",
            detail: "A broadcast must have at least 2 competitors."
        );
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    private sealed partial class Admin
    {
        public async Task Given_I_have_created_a_Liverpool_rules_contest(
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            string globalTelevoteCountry = "",
            int contestYear = 0
        )
        {
            ExistingContest = await Kernel.CreateALiverpoolRulesContestAsync(
                contestYear: contestYear,
                globalTelevoteCountryId: ExistingCountryIds.GetId(globalTelevoteCountry),
                semiFinal1CountryIds: semiFinal1Countries.Select(ExistingCountryIds.GetId).ToArray(),
                semiFinal2CountryIds: semiFinal2Countries.Select(ExistingCountryIds.GetId).ToArray()
            );
        }
    }
}

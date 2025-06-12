using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed partial class CreateChildBroadcastTests
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_Liverpool_format_SemiFinal1_broadcast_scenario_1(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_broadcast_parent_contest_ID_should_be_my_contest_ID();
        admin.Then_the_created_broadcast_should_match(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-01",
            broadcastStatus: "Initialized",
            competitors: """
                         | FinishingPosition | CountryCode | RunningOrderPosition | JuryAwards | TelevoteAwards |
                         |                 1 | AT          |                    1 | []         | []             |
                         |                 2 | BE          |                    2 | []         | []             |
                         """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       | AT          | false         |
                       | BE          | false         |
                       | CZ          | false         |
                       | XX          | false         |
                       """);
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_now_have_additional_memoized_created_broadcast_and_InProgress_contest_status();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_Liverpool_format_SemiFinal1_broadcast_scenario_2(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_SemiFinal2_child_broadcast_for_my_contest_with_broadcast_date("2025-05-02");
        await admin.Given_I_have_created_a_GrandFinal_child_broadcast_for_my_contest_with_broadcast_date("2025-05-03");
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "CZ", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_broadcast_parent_contest_ID_should_be_my_contest_ID();
        admin.Then_the_created_broadcast_should_match(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-01",
            broadcastStatus: "Initialized",
            competitors: """
                         | FinishingPosition | CountryCode | RunningOrderPosition | JuryAwards | TelevoteAwards |
                         |                 1 | AT          |                    1 | []         | []             |
                         |                 2 | CZ          |                    2 | []         | []             |
                         |                 3 | BE          |                    3 | []         | []             |
                         """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       | AT          | false         |
                       | BE          | false         |
                       | CZ          | false         |
                       | XX          | false         |
                       """);
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_now_have_additional_memoized_created_broadcast_and_InProgress_contest_status();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_Liverpool_format_SemiFinal2_broadcast_scenario_1(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2025-05-02",
            competingCountryCodes: ["DK", "EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_broadcast_parent_contest_ID_should_be_my_contest_ID();
        admin.Then_the_created_broadcast_should_match(
            contestStage: "SemiFinal2",
            broadcastDate: "2025-05-02",
            broadcastStatus: "Initialized",
            competitors: """
                         | FinishingPosition | CountryCode | RunningOrderPosition | JuryAwards | TelevoteAwards |
                         |                 1 | DK          |                    1 | []         | []             |
                         |                 2 | EE          |                    2 | []         | []             |
                         """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       | DK          | false         |
                       | EE          | false         |
                       | FI          | false         |
                       | XX          | false         |
                       """);
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_now_have_additional_memoized_created_broadcast_and_InProgress_contest_status();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_Liverpool_format_SemiFinal2_broadcast_scenario_2(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_SemiFinal1_child_broadcast_for_my_contest_with_broadcast_date("2025-05-01");
        await admin.Given_I_have_created_a_GrandFinal_child_broadcast_for_my_contest_with_broadcast_date("2025-05-03");
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2025-05-02",
            competingCountryCodes: ["FI", "DK", "EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_broadcast_parent_contest_ID_should_be_my_contest_ID();
        admin.Then_the_created_broadcast_should_match(
            contestStage: "SemiFinal2",
            broadcastDate: "2025-05-02",
            broadcastStatus: "Initialized",
            competitors: """
                         | FinishingPosition | CountryCode | RunningOrderPosition | JuryAwards | TelevoteAwards |
                         |                 1 | FI          |                    1 | []         | []             |
                         |                 2 | DK          |                    2 | []         | []             |
                         |                 3 | EE          |                    3 | []         | []             |
                         """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       | DK          | false         |
                       | EE          | false         |
                       | FI          | false         |
                       | XX          | false         |
                       """);
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_now_have_additional_memoized_created_broadcast_and_InProgress_contest_status();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_Liverpool_format_GrandFinal_broadcast_scenario_1(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            competingCountryCodes: ["AT", "DK"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_broadcast_parent_contest_ID_should_be_my_contest_ID();
        admin.Then_the_created_broadcast_should_match(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            broadcastStatus: "Initialized",
            competitors: """
                         | FinishingPosition | CountryCode | RunningOrderPosition | JuryAwards | TelevoteAwards |
                         |                 1 | AT          |                    1 | []         | []             |
                         |                 2 | DK          |                    2 | []         | []             |
                         """,
            juries: """
                    | CountryCode | PointsAwarded |
                    | AT          | false         |
                    | BE          | false         |
                    | CZ          | false         |
                    | DK          | false         |
                    | EE          | false         |
                    | FI          | false         |
                    """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       | AT          | false         |
                       | BE          | false         |
                       | CZ          | false         |
                       | DK          | false         |
                       | EE          | false         |
                       | FI          | false         |
                       | XX          | false         |
                       """);
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_now_have_additional_memoized_created_broadcast_and_InProgress_contest_status();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_Liverpool_format_GrandFinal_broadcast_scenario_2(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_SemiFinal1_child_broadcast_for_my_contest_with_broadcast_date("2025-05-01");
        await admin.Given_I_have_created_a_SemiFinal2_child_broadcast_for_my_contest_with_broadcast_date("2025-05-02");
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            competingCountryCodes: ["DK", "CZ", "BE", "AT", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_broadcast_parent_contest_ID_should_be_my_contest_ID();
        admin.Then_the_created_broadcast_should_match(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            broadcastStatus: "Initialized",
            competitors: """
                         | FinishingPosition | CountryCode | RunningOrderPosition | JuryAwards | TelevoteAwards |
                         |                 1 | DK          |                    1 | []         | []             |
                         |                 2 | CZ          |                    2 | []         | []             |
                         |                 3 | BE          |                    3 | []         | []             |
                         |                 4 | AT          |                    4 | []         | []             |
                         |                 5 | EE          |                    5 | []         | []             |
                         |                 6 | FI          |                    6 | []         | []             |
                         """,
            juries: """
                    | CountryCode | PointsAwarded |
                    | AT          | false         |
                    | BE          | false         |
                    | CZ          | false         |
                    | DK          | false         |
                    | EE          | false         |
                    | FI          | false         |
                    """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       | AT          | false         |
                       | BE          | false         |
                       | CZ          | false         |
                       | DK          | false         |
                       | EE          | false         |
                       | FI          | false         |
                       | XX          | false         |
                       """);
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_now_have_additional_memoized_created_broadcast_and_InProgress_contest_status();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_with_illegal_broadcast_date_value(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "1999-01-01",
            competingCountryCodes: ["AT", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal broadcast date value",
            detail: "Broadcast date value must be between 2016-01-01 and 2050-12-31.");
        admin.Then_the_problem_details_extensions_should_contain("broadcastDate", "1999-01-01");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_with_illegal_broadcast_date_value(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "1999-01-01",
            competingCountryCodes: ["DK", "EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal broadcast date value",
            detail: "Broadcast date value must be between 2016-01-01 and 2050-12-31.");
        admin.Then_the_problem_details_extensions_should_contain("broadcastDate", "1999-01-01");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_GrandFinal_broadcast_with_illegal_broadcast_date_value(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "1999-01-01",
            competingCountryCodes: ["AT", "DK"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal broadcast date value",
            detail: "Broadcast date value must be between 2016-01-01 and 2050-12-31.");
        admin.Then_the_problem_details_extensions_should_contain("broadcastDate", "1999-01-01");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_with_broadcast_date_not_in_contest_year(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2026-01-01",
            competingCountryCodes: ["AT", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Broadcast date out of range",
            detail: "A broadcast's date must be in the same year as its parent contest.");
        admin.Then_the_problem_details_extensions_should_contain("broadcastDate", "2026-01-01");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_with_broadcast_date_not_in_contest_year(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2026-01-01",
            competingCountryCodes: ["DK", "EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Broadcast date out of range",
            detail: "A broadcast's date must be in the same year as its parent contest.");
        admin.Then_the_problem_details_extensions_should_contain("broadcastDate", "2026-01-01");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_GrandFinal_broadcast_with_broadcast_date_not_in_contest_year(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2026-01-01",
            competingCountryCodes: ["AT", "DK"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Broadcast date out of range",
            detail: "A broadcast's date must be in the same year as its parent contest.");
        admin.Then_the_problem_details_extensions_should_contain("broadcastDate", "2026-01-01");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_with_fewer_than_2_competitors(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Insufficient competitors",
            detail: "A broadcast must have at least 2 competitors.");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_with_fewer_than_2_competitors(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2025-05-02",
            competingCountryCodes: ["DK"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Insufficient competitors",
            detail: "A broadcast must have at least 2 competitors.");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_GrandFinal_broadcast_with_fewer_than_2_competitors(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            competingCountryCodes: ["AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Insufficient competitors",
            detail: "A broadcast must have at least 2 competitors.");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_with_competitors_from_same_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate competing countries",
            detail: "Every competitor in a broadcast must reference a different competing country.");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_with_competitors_from_same_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2025-05-02",
            competingCountryCodes: ["DK", "EE", "DK"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate competing countries",
            detail: "Every competitor in a broadcast must reference a different competing country.");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_GrandFinal_broadcast_with_competitors_from_same_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            competingCountryCodes: ["AT", "DK", "AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate competing countries",
            detail: "Every competitor in a broadcast must reference a different competing country.");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_with_orphan_competitor(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "GB"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Orphan competitor",
            detail: "Parent contest has no participant with the provided competing country ID.");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "competingCountryId", countryCode: "GB");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_with_orphan_competitor(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2025-05-02",
            competingCountryCodes: ["DK", "EE", "GB"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Orphan competitor",
            detail: "Parent contest has no participant with the provided competing country ID.");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "competingCountryId", countryCode: "GB");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_GrandFinal_broadcast_with_orphan_competitor(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            competingCountryCodes: ["AT", "DK", "GB"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Orphan competitor",
            detail: "Parent contest has no participant with the provided competing country ID.");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "competingCountryId", countryCode: "GB");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_when_SemiFinal1_already_exists(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_SemiFinal1_child_broadcast_for_my_contest_with_broadcast_date("2025-04-30");
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Child broadcast contest stage conflict",
            detail: "The contest already has a child broadcast with the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal1");
        await admin.Then_my_existing_contest_child_broadcasts_should_be_the_only_existing_broadcasts();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_when_SemiFinal2_already_exists(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_SemiFinal2_child_broadcast_for_my_contest_with_broadcast_date("2025-04-30");
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2025-05-02",
            competingCountryCodes: ["DK", "EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Child broadcast contest stage conflict",
            detail: "The contest already has a child broadcast with the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal2");
        await admin.Then_my_existing_contest_child_broadcasts_should_be_the_only_existing_broadcasts();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_GrandFinal_broadcast_when_GrandFinal_already_exists(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_GrandFinal_child_broadcast_for_my_contest_with_broadcast_date("2025-04-30");
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            competingCountryCodes: ["AT", "DK"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Child broadcast contest stage conflict",
            detail: "The contest already has a child broadcast with the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "GrandFinal");
        await admin.Then_my_existing_contest_child_broadcasts_should_be_the_only_existing_broadcasts();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_with_competitor_from_participant_group_0(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "XX"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Ineligible competing country",
            detail: "The contest has a participant with the provided competing country ID, " +
                    "but they are not eligible to compete in the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal1");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "competingCountryId", countryCode: "XX");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_with_competitor_from_participant_group_2(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Ineligible competing country",
            detail: "The contest has a participant with the provided competing country ID, " +
                    "but they are not eligible to compete in the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal1");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "competingCountryId", countryCode: "FI");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_with_competitor_from_participant_group_0(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2025-05-02",
            competingCountryCodes: ["DK", "EE", "XX"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Ineligible competing country",
            detail: "The contest has a participant with the provided competing country ID, " +
                    "but they are not eligible to compete in the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal2");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "competingCountryId", countryCode: "XX");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_with_competitor_from_participant_group_1(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2025-05-02",
            competingCountryCodes: ["DK", "EE", "CZ"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Ineligible competing country",
            detail: "The contest has a participant with the provided competing country ID, " +
                    "but they are not eligible to compete in the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal2");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "competingCountryId", countryCode: "CZ");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_GrandFinal_broadcast_with_competitor_from_participant_group_0(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            competingCountryCodes: ["DK", "EE", "XX"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Ineligible competing country",
            detail: "The contest has a participant with the provided competing country ID, " +
                    "but they are not eligible to compete in the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "GrandFinal");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "competingCountryId", countryCode: "XX");
        await admin.Then_no_broadcasts_should_exist();
        await admin.Then_my_contest_should_be_unchanged();
    }
}

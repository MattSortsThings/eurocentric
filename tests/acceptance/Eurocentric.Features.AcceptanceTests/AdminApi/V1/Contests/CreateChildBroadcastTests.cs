using System.Net;
using System.Text.Json;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Infrastructure.EfCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class CreateChildBroadcastTests : AcceptanceTestBase
{
    public CreateChildBroadcastTests(WebAppFixture webAppFixture) : base(webAppFixture)
    {
    }

    private protected override int ApiMajorVersion => 1;

    private protected override int ApiMinorVersion => 0;

    [Fact]
    public async Task Should_be_able_to_create_Stockholm_format_SemiFinal1_broadcast_scenario_1_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage();
        admin.Then_the_created_broadcast_should_have_the_ordered_competitors("AT", "BE");
        admin.Then_the_created_broadcast_should_have_the_juries("AT", "BE", "CZ", "DE");
        admin.Then_the_created_broadcast_should_have_the_televotes("AT", "BE", "CZ", "DE");
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast();
    }

    [Fact]
    public async Task Should_be_able_to_create_Stockholm_format_SemiFinal1_broadcast_scenario_2_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["CZ", "AT", "DE", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage();
        admin.Then_the_created_broadcast_should_have_the_ordered_competitors("CZ", "AT", "DE", "BE");
        admin.Then_the_created_broadcast_should_have_the_juries("AT", "BE", "CZ", "DE");
        admin.Then_the_created_broadcast_should_have_the_televotes("AT", "BE", "CZ", "DE");
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast();
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_SemiFinal1_broadcast_when_contest_already_has_SemiFinal1()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT", "BE"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest_with_the_same_requirements();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Broadcast contest stage conflict",
            detail: "Contest already has a child broadcast with the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal1");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_SemiFinal1_broadcast_with_non_participant_competitor()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT", "BE", "IT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("IT");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal1");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_SemiFinal1_broadcast_with_competitor_from_group_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT", "BE", "HR"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("HR");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal1");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_SemiFinal1_broadcast_with_fewer_than_two_competitors()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal broadcast size",
            detail: "Broadcast must have at least 2 competitors.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_SemiFinal1_broadcast_with_duplicate_competing_country_IDs()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT", "BE", "CZ", "AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate competing country IDs",
            detail: "Every competitor in a broadcast must have a different competing country ID.");
    }

    [Fact]
    public async Task Should_be_able_to_create_Stockholm_format_SemiFinal2_broadcast_scenario_1_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage();
        admin.Then_the_created_broadcast_should_have_the_ordered_competitors("EE", "FI");
        admin.Then_the_created_broadcast_should_have_the_juries("EE", "FI", "GB", "HR");
        admin.Then_the_created_broadcast_should_have_the_televotes("EE", "FI", "GB", "HR");
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast();
    }

    [Fact]
    public async Task Should_be_able_to_create_Stockholm_format_SemiFinal2_broadcast_scenario_2_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["HR", "EE", "GB", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage();
        admin.Then_the_created_broadcast_should_have_the_ordered_competitors("HR", "EE", "GB", "FI");
        admin.Then_the_created_broadcast_should_have_the_juries("EE", "FI", "GB", "HR");
        admin.Then_the_created_broadcast_should_have_the_televotes("EE", "FI", "GB", "HR");
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast();
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_SemiFinal2_broadcast_when_contest_already_has_SemiFinal2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE", "FI"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest_with_the_same_requirements();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Broadcast contest stage conflict",
            detail: "Contest already has a child broadcast with the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal2");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_SemiFinal2_broadcast_with_non_participant_competitor()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE", "FI", "IT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("IT");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal2");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_SemiFinal2_broadcast_with_competitor_from_group_1()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE", "FI", "AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("AT");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal2");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_SemiFinal2_broadcast_with_fewer_than_two_competitors()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal broadcast size",
            detail: "Broadcast must have at least 2 competitors.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_SemiFinal2_broadcast_with_duplicate_competing_country_IDs()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE", "FI", "GB", "EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate competing country IDs",
            detail: "Every competitor in a broadcast must have a different competing country ID.");
    }

    [Fact]
    public async Task Should_be_able_to_create_Stockholm_format_GrandFinal_broadcast_scenario_1_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["AT", "EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage();
        admin.Then_the_created_broadcast_should_have_the_ordered_competitors("AT", "EE");
        admin.Then_the_created_broadcast_should_have_the_juries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR");
        admin.Then_the_created_broadcast_should_have_the_televotes("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR");
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast();
    }

    [Fact]
    public async Task Should_be_able_to_create_Stockholm_format_GrandFinal_broadcast_scenario_2_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["GB", "AT", "DE", "EE", "CZ", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage();
        admin.Then_the_created_broadcast_should_have_the_ordered_competitors("GB", "AT", "DE", "EE", "CZ", "FI");
        admin.Then_the_created_broadcast_should_have_the_juries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR");
        admin.Then_the_created_broadcast_should_have_the_televotes("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR");
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast();
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_GrandFinal_broadcast_when_contest_already_has_GrandFinal()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["AT", "EE"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest_with_the_same_requirements();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Broadcast contest stage conflict",
            detail: "Contest already has a child broadcast with the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "GrandFinal");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_GrandFinal_broadcast_with_non_participant_competitor()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["AT", "IT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("IT");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "GrandFinal");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_GrandFinal_broadcast_with_fewer_than_two_competitors()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal broadcast size",
            detail: "Broadcast must have at least 2 competitors.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Stockholm_format_GrandFinal_broadcast_with_duplicate_competing_country_IDs()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["AT", "EE", "CZ", "DE", "FI", "AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate competing country IDs",
            detail: "Every competitor in a broadcast must have a different competing country ID.");
    }

    [Fact]
    public async Task Should_be_able_to_create_Liverpool_format_SemiFinal1_broadcast_scenario_1_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage();
        admin.Then_the_created_broadcast_should_have_the_ordered_competitors("AT", "BE");
        admin.Then_the_created_broadcast_should_have_no_juries();
        admin.Then_the_created_broadcast_should_have_the_televotes("XX", "AT", "BE", "CZ", "DE");
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast();
    }

    [Fact]
    public async Task Should_be_able_to_create_Liverpool_format_SemiFinal1_broadcast_scenario_2_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT", "BE", "DE", "CZ"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage();
        admin.Then_the_created_broadcast_should_have_the_ordered_competitors("AT", "BE", "DE", "CZ");
        admin.Then_the_created_broadcast_should_have_no_juries();
        admin.Then_the_created_broadcast_should_have_the_televotes("XX", "AT", "BE", "CZ", "DE");
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast();
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_when_contest_already_has_SemiFinal1()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT", "BE"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest_with_the_same_requirements();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Broadcast contest stage conflict",
            detail: "Contest already has a child broadcast with the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal1");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_with_non_participant_competitor()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT", "BE", "IT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("IT");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal1");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_with_competitor_from_group_0()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT", "BE", "XX"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("XX");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal1");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_with_competitor_from_group_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT", "BE", "HR"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("HR");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal1");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_with_fewer_than_two_competitors()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal broadcast size",
            detail: "Broadcast must have at least 2 competitors.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal1_broadcast_with_duplicate_competing_country_IDs()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            competitors: ["AT", "BE", "BE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate competing country IDs",
            detail: "Every competitor in a broadcast must have a different competing country ID.");
    }

    [Fact]
    public async Task Should_be_able_to_create_Liverpool_format_SemiFinal2_broadcast_scenario_1_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage();
        admin.Then_the_created_broadcast_should_have_the_ordered_competitors("EE", "FI");
        admin.Then_the_created_broadcast_should_have_no_juries();
        admin.Then_the_created_broadcast_should_have_the_televotes("XX", "EE", "FI", "GB", "HR");
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast();
    }

    [Fact]
    public async Task Should_be_able_to_create_Liverpool_format_SemiFinal2_broadcast_scenario_2_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["HR", "GB", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage();
        admin.Then_the_created_broadcast_should_have_the_ordered_competitors("HR", "GB", "EE", "FI");
        admin.Then_the_created_broadcast_should_have_no_juries();
        admin.Then_the_created_broadcast_should_have_the_televotes("XX", "EE", "FI", "GB", "HR");
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast();
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_when_contest_already_has_SemiFinal2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE", "FI"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest_with_the_same_requirements();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Broadcast contest stage conflict",
            detail: "Contest already has a child broadcast with the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal2");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_with_non_participant_competitor()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE", "FI", "IT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("IT");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal2");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_with_competitor_from_group_0()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE", "FI", "XX"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("XX");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal2");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_with_competitor_from_group_1()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE", "FI", "DE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("DE");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "SemiFinal2");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_with_fewer_than_two_competitors()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal broadcast size",
            detail: "Broadcast must have at least 2 competitors.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_SemiFinal2_broadcast_with_duplicate_competing_country_IDs()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            competitors: ["EE", "FI", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate competing country IDs",
            detail: "Every competitor in a broadcast must have a different competing country ID.");
    }

    [Fact]
    public async Task Should_be_able_to_create_Liverpool_format_GrandFinal_broadcast_scenario_1_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["AT", "EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage();
        admin.Then_the_created_broadcast_should_have_the_ordered_competitors("AT", "EE");
        admin.Then_the_created_broadcast_should_have_the_juries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR");
        admin.Then_the_created_broadcast_should_have_the_televotes("XX", "AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR");
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast();
    }

    [Fact]
    public async Task Should_be_able_to_create_Liverpool_format_GrandFinal_broadcast_scenario_2_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["HR", "DE", "BE", "GB", "AT", "EE", "CZ"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage();
        admin.Then_the_created_broadcast_should_have_the_ordered_competitors("HR", "DE", "BE", "GB", "AT", "EE", "CZ");
        admin.Then_the_created_broadcast_should_have_the_juries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR");
        admin.Then_the_created_broadcast_should_have_the_televotes("XX", "AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR");
        await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
        await admin.Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast();
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_GrandFinal_broadcast_when_contest_already_has_GrandFinal()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["AT", "EE"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest_with_the_same_requirements();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Broadcast contest stage conflict",
            detail: "Contest already has a child broadcast with the provided contest stage.");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "GrandFinal");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_GrandFinal_broadcast_with_non_participant_competitor()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["AT", "EE", "IT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("IT");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "GrandFinal");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_GrandFinal_broadcast_with_competitor_from_group_0()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["AT", "EE", "XX"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Illegal competing country IDs",
            detail: "Every competitor in a broadcast must share a country ID with a contest participant " +
                    "eligible to compete in the requested contest stage.");
        admin.Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for("XX");
        admin.Then_the_problem_details_extensions_should_contain("contestStage", "GrandFinal");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_GrandFinal_broadcast_with_fewer_than_two_competitors()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal broadcast size",
            detail: "Broadcast must have at least 2 competitors.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_Liverpool_format_GrandFinal_broadcast_with_duplicate_competing_country_IDs()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["AT", "EE", "AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate competing country IDs",
            detail: "Every competitor in a broadcast must have a different competing country ID.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_child_broadcast_for_a_non_existent_contest()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "DE"],
            group2Participants: ["EE", "FI", "GB", "HR"]);
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            competitors: ["AT", "EE"]);
        await admin.Given_I_have_deleted_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.NotFound);
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Contest not found",
            detail: "No contest exists with the provided contest ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_contest_ID();
    }

    private sealed class AdminActor : ActorWithResponse<CreateChildBroadcastResponse>
    {
        private readonly WebAppFixtureBackdoor _backdoor;
        private readonly AdminApiV1Driver _driver;

        private AdminActor(WebAppFixtureBackdoor backdoor, AdminApiV1Driver driver)
        {
            _backdoor = backdoor;
            _driver = driver;
        }

        private Dictionary<string, Guid> MyCountries { get; } = new(10);

        private Contest? MyContest { get; set; }

        private CreateChildBroadcastRequest? MyRequirements { get; set; }

        public async Task Given_I_have_created_the_countries(params string[] countryCodes)
        {
            Country[] countries = await
                _driver.CreateMultipleCountriesAsync(countryCodes, TestContext.Current.CancellationToken);

            foreach (Country country in countries)
            {
                MyCountries.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_contest(
            string? group0Participant = null,
            string[]? group1Participants = null,
            string[]? group2Participants = null,
            string? contestFormat = null)
        {
            CreateContestRequest request = new()
            {
                ContestYear = 2022,
                CityName = "Turin",
                ContestFormat = Enum.Parse<ContestFormat>(contestFormat ?? "Stockholm"),
                Group0CountryId = group0Participant is null ? null : MyCountries[group0Participant],
                Group1Participants = group1Participants?.Select(p => MyCountries[p]).ToContestParticipantData() ?? [],
                Group2Participants = group2Participants?.Select(p => MyCountries[p]).ToContestParticipantData() ?? []
            };

            ResponseOrProblem<CreateContestResponse> responseOrProblem =
                await _driver.CreateContestAsync(request, TestContext.Current.CancellationToken);

            MyContest = responseOrProblem.AsT0.Data!.Contest;
        }

        public void Given_I_want_to_create_a_child_broadcast_for_my_contest(string[]? competitors = null,
            string? contestStage = null)
        {
            MyRequirements = new CreateChildBroadcastRequest
            {
                ContestStage = Enum.Parse<ContestStage>(contestStage ?? "SemiFinal1"),
                CompetingCountryIds = competitors?.Select(c => MyCountries[c]).ToArray() ?? []
            };

            SendMyRequest = () =>
                _driver.CreateChildBroadcastAsync(MyContest!.Id, MyRequirements!, TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_created_a_child_broadcast_for_my_contest_with_the_same_requirements() =>
            _ = await _driver.CreateChildBroadcastAsync(MyContest!.Id, MyRequirements!, TestContext.Current.CancellationToken);

        public async Task Given_I_have_deleted_my_contest() => await _backdoor.DeleteContestAsync(MyContest!.Id);

        public static AdminActor WithDriverAndBackdoor(AdminApiV1Driver driver, WebAppFixtureBackdoor backdoor) =>
            new(backdoor, driver);

        public void Then_the_created_broadcast_should_be_initialized_with_my_contest_ID_and_my_required_contest_stage()
        {
            Assert.NotNull(MyContest);
            Assert.NotNull(MyRequirements);
            Assert.NotNull(Response);

            Broadcast createdBroadcast = Response.Broadcast;

            Assert.Equal(MyContest.Id, createdBroadcast.ContestId);
            Assert.Equal(MyRequirements.ContestStage, createdBroadcast.ContestStage);
            Assert.Equal(BroadcastStatus.Initialized, createdBroadcast.Status);
        }

        public void Then_the_created_broadcast_should_have_the_ordered_competitors(params string[] competitors)
        {
            Assert.NotNull(Response);

            IEnumerable<Guid> expectedCompetingCountryIds = competitors.Select(c => MyCountries[c]);
            IEnumerable<Guid> actualCompetingCountryIds = Response.Broadcast.Competitors.Select(c => c.CompetingCountryId);

            Assert.Equal(expectedCompetingCountryIds, actualCompetingCountryIds);
        }

        public void Then_the_created_broadcast_should_have_the_juries(params string[] juries)
        {
            Assert.NotNull(Response);

            IEnumerable<Guid> expectedVotingCountryIds = juries.Select(c => MyCountries[c]);
            IEnumerable<Guid> actualVotingCountryIds = Response.Broadcast.Juries.Select(v => v.VotingCountryId);

            Assert.Equivalent(expectedVotingCountryIds, actualVotingCountryIds);
        }

        public void Then_the_created_broadcast_should_have_no_juries()
        {
            Assert.NotNull(Response);

            Assert.Empty(Response.Broadcast.Juries);
        }

        public void Then_the_created_broadcast_should_have_the_televotes(params string[] televotes)
        {
            Assert.NotNull(Response);

            IEnumerable<Guid> expectedVotingCountryIds = televotes.Select(c => MyCountries[c]);
            IEnumerable<Guid> actualVotingCountryIds = Response.Broadcast.Televotes.Select(v => v.VotingCountryId);

            Assert.Equivalent(expectedVotingCountryIds, actualVotingCountryIds);
        }

        public async Task Then_the_created_broadcast_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(Response);

            Broadcast createdBroadcast = Response.Broadcast;
            Broadcast retrievedBroadcast = await GetBroadcastAsync(createdBroadcast.Id);

            Assert.Equal(createdBroadcast, retrievedBroadcast, new BroadcastEqualityComparer());
        }

        public async Task Then_my_contest_should_have_been_updated_with_an_initialized_memo_for_the_created_broadcast()
        {
            Assert.NotNull(MyContest);
            Assert.NotNull(Response);

            Contest myUpdatedContest = await GetContestAsync(MyContest.Id);

            BroadcastMemo expectedMemo = new(Response.Broadcast.Id, Response.Broadcast.ContestStage,
                BroadcastStatus.Initialized);

            Assert.Contains(expectedMemo, myUpdatedContest.BroadcastMemos);
        }

        public void Then_the_problem_details_extensions_should_contain_my_contest_ID()
        {
            Assert.NotNull(ProblemDetails);

            Assert.Contains(ProblemDetails.Extensions,
                kvp => kvp is { Key: "contestId", Value: JsonElement j } && j.GetGuid() == MyContest!.Id);
        }

        public void Then_the_problem_details_extensions_should_contain_the_illegal_competing_country_ID_for(string countryCode)
        {
            Guid expectedCountryId = MyCountries[countryCode];

            Assert.NotNull(ProblemDetails);

            Assert.Contains(ProblemDetails.Extensions, kvp => kvp is { Key: "illegalCompetingCountryIds", Value: JsonElement j }
                                                              && j.EnumerateArray().Single().GetGuid() == expectedCountryId);
        }

        private async Task<Broadcast> GetBroadcastAsync(Guid broadcastId)
        {
            ResponseOrProblem<GetBroadcastResponse> responseOrProblem =
                await _driver.GetBroadcastAsync(broadcastId, TestContext.Current.CancellationToken);

            return responseOrProblem.AsT0.Data!.Broadcast;
        }

        private async Task<Contest> GetContestAsync(Guid contestId)
        {
            ResponseOrProblem<GetContestResponse> responseOrProblem =
                await _driver.GetContestAsync(contestId, TestContext.Current.CancellationToken);

            return responseOrProblem.AsT0.Data!.Contest;
        }
    }

    private sealed class WebAppFixtureBackdoor(WebAppFixture fixture)
    {
        public async Task DeleteContestAsync(Guid contestId)
        {
            ContestId targetId = ContestId.FromValue(contestId);

            Func<IServiceProvider, Task> persist = async sp =>
            {
                await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                dbContext.Contests.Remove(dbContext.Contests.First(c => c.Id == targetId));
                await dbContext.SaveChangesAsync();
            };

            await fixture.ExecuteScopedAsync(persist);
        }
    }
}

using System.Net;
using System.Text.Json;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests.TestUtils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Infrastructure.EfCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class CreateContestTests : AcceptanceTestBase
{
    public CreateContestTests(WebAppFixture webAppFixture) : base(webAppFixture) { }

    private protected override int ApiMajorVersion => 1;

    private protected override int ApiMinorVersion => 0;

    [Fact]
    public async Task Should_be_able_to_create_a_Liverpool_format_contest_scenario_1_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            contestYear: 2025,
            cityName: "Basel",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_contest_should_have_been_correctly_created_from_my_requirements();
        await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
    }

    [Fact]
    public async Task Should_be_able_to_create_a_Liverpool_format_contest_scenario_2_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            contestYear: 2023,
            cityName: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["CZ", "ES", "AT", "HR"],
            group2Participants: ["DE", "GB", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_contest_should_have_been_correctly_created_from_my_requirements();
        await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
    }

    [Fact]
    public async Task Should_be_able_to_create_a_Stockholm_format_contest_scenario_1_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Stockholm",
            contestYear: 2022,
            cityName: "Turin",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_contest_should_have_been_correctly_created_from_my_requirements();
        await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
    }

    [Fact]
    public async Task Should_be_able_to_create_a_Stockholm_format_contest_scenario_2_of_2()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Stockholm",
            contestYear: 2016,
            cityName: "Stockholm",
            group1Participants: ["DE", "GB", "FI"],
            group2Participants: ["CZ", "ES", "AT", "HR"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_contest_should_have_been_correctly_created_from_my_requirements();
        await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_an_illegal_contest_year_value()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        const int illegalContestYear = 0;

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(illegalContestYear,
            contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal contest year value",
            detail: "Contest year value must be an integer between 2016 and 2050.");
        admin.Then_the_problem_details_extensions_should_contain("contestYear", illegalContestYear);
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_an_illegal_city_name_value()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        const string illegalCityName = "";

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(cityName: illegalCityName,
            contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal city name value",
            detail: "City name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("cityName", illegalCityName);
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_an_illegal_group_1_participant_act_name_value()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        const string illegalActName = "";

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);
        admin.Given_the_first_group_1_participant_has_the_act_name(illegalActName);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal act name value",
            detail: "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("actName", illegalActName);
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_an_illegal_group_1_participant_song_title_value()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        const string illegalSongTitle = "";

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);
        admin.Given_the_first_group_1_participant_has_the_song_title(illegalSongTitle);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal song title value",
            detail: "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("songTitle", illegalSongTitle);
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_an_illegal_group_2_participant_act_name_value()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        const string illegalActName = "";

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);
        admin.Given_the_first_group_2_participant_has_the_act_name(illegalActName);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal act name value",
            detail: "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("actName", illegalActName);
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_an_illegal_group_2_participant_song_title_value()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        const string illegalSongTitle = "";

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Stockholm",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);
        admin.Given_the_first_group_2_participant_has_the_song_title(illegalSongTitle);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal song title value",
            detail: "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("songTitle", illegalSongTitle);
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_a_group_0_participant_referencing_a_non_existent_country()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);
        await admin.Given_I_have_deleted_the_country("XX");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Orphan participating country IDs",
            detail: "Every participant in a contest must reference an existing country by its ID.");
        admin.Then_the_problem_details_extensions_should_contain_the_orphan_country_ID_for("XX");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_a_group_1_participant_referencing_a_non_existent_country()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);
        await admin.Given_I_have_deleted_the_country("BE");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Orphan participating country IDs",
            detail: "Every participant in a contest must reference an existing country by its ID.");
        admin.Then_the_problem_details_extensions_should_contain_the_orphan_country_ID_for("BE");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_a_group_2_participant_referencing_a_non_existent_country()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);
        await admin.Given_I_have_deleted_the_country("FI");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Orphan participating country IDs",
            detail: "Every participant in a contest must reference an existing country by its ID.");
        admin.Then_the_problem_details_extensions_should_contain_the_orphan_country_ID_for("FI");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_a_non_unique_contest_year()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        const int duplicateContestYear = 2023;

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        await admin.Given_I_have_created_a_contest_with_the_contest_year(duplicateContestYear);
        admin.Given_I_want_to_create_a_contest(2023,
            contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Contest year conflict",
            detail: "Contest already exists with the provided contest year.");
        admin.Then_the_problem_details_extensions_should_contain("contestYear", duplicateContestYear);
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_two_participants_referencing_same_country_scenario_1_of_5()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "XX"],
            group2Participants: ["DE", "ES", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating country IDs",
            detail: "Every participant in a contest must have a different participating country ID.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_two_participants_referencing_same_country_scenario_2_of_5()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI", "XX"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating country IDs",
            detail: "Every participant in a contest must have a different participating country ID.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_two_participants_referencing_same_country_scenario_3_of_5()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI", "AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating country IDs",
            detail: "Every participant in a contest must have a different participating country ID.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_two_participants_referencing_same_country_scenario_4_of_5()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ", "AT"],
            group2Participants: ["DE", "ES", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating country IDs",
            detail: "Every participant in a contest must have a different participating country ID.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_contest_with_two_participants_referencing_same_country_scenario_5_of_5()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI", "DE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating country IDs",
            detail: "Every participant in a contest must have a different participating country ID.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_Liverpool_format_contest_with_no_group_0_participant()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal Liverpool format group sizes",
            detail: "A Liverpool format contest must have 1 Group 0 participant, at least 3 Group 1 participants, " +
                    "and at least 3 Group 2 participants.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_Liverpool_format_contest_with_fewer_than_3_group_1_participants()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE"],
            group2Participants: ["DE", "ES", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal Liverpool format group sizes",
            detail: "A Liverpool format contest must have 1 Group 0 participant, at least 3 Group 1 participants, " +
                    "and at least 3 Group 2 participants.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_Liverpool_format_contest_with_fewer_than_3_group_2_participants()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Liverpool",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal Liverpool format group sizes",
            detail: "A Liverpool format contest must have 1 Group 0 participant, at least 3 Group 1 participants, " +
                    "and at least 3 Group 2 participants.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_Stockholm_format_contest_with_a_group_0_participant()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Stockholm",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal Stockholm format group sizes",
            detail: "A Stockholm format contest must have no Group 0 participants, at least 3 Group 1 participants, " +
                    "and at least 3 Group 2 participants.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_Stockholm_format_contest_with_fewer_than_3_group_1_participants()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Stockholm",
            group0Participant: "XX",
            group1Participants: ["AT", "BE"],
            group2Participants: ["DE", "ES", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal Stockholm format group sizes",
            detail: "A Stockholm format contest must have no Group 0 participants, at least 3 Group 1 participants, " +
                    "and at least 3 Group 2 participants.");
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_Stockholm_format_contest_with_fewer_than_3_group_2_participants()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "BE", "CZ", "DE", "ES", "FI", "GB", "HR", "XX");
        admin.Given_I_want_to_create_a_contest(contestFormat: "Stockholm",
            group0Participant: "XX",
            group1Participants: ["AT", "BE", "CZ"],
            group2Participants: ["DE", "ES"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal Stockholm format group sizes",
            detail: "A Stockholm format contest must have no Group 0 participants, at least 3 Group 1 participants, " +
                    "and at least 3 Group 2 participants.");
    }

    private static IEnumerable<Participant> GetExpectedParticipants(CreateContestRequest request)
    {
        if (request.Group0CountryId is { } id)
        {
            yield return new Participant { ParticipatingCountryId = id, Group = 0 };
        }

        foreach (ContestParticipantDatum p in request.Group1Participants.OrderBy(datum => datum.CountryId))
        {
            yield return new Participant
            {
                ParticipatingCountryId = p.CountryId, Group = 1, ActName = p.ActName, SongTitle = p.SongTitle
            };
        }

        foreach (ContestParticipantDatum p in request.Group2Participants.OrderBy(datum => datum.CountryId))
        {
            yield return new Participant
            {
                ParticipatingCountryId = p.CountryId, Group = 2, ActName = p.ActName, SongTitle = p.SongTitle
            };
        }
    }

    private sealed class AdminActor : ActorWithResponse<CreateContestResponse>
    {
        private readonly WebAppFixtureBackdoor _backdoor;
        private readonly AdminApiV1Driver _driver;

        private AdminActor(AdminApiV1Driver driver, WebAppFixtureBackdoor backdoor)
        {
            _driver = driver;
            _backdoor = backdoor;
        }

        private Dictionary<string, Guid> MyCountries { get; } = new(7);

        private CreateContestRequest? MyRequirements { get; set; }

        private protected override Func<Task<ResponseOrProblem<CreateContestResponse>>> SendMyRequest { get; set; } = null!;

        public async Task Given_I_have_created_the_countries(params string[] countryCodes)
        {
            Country[] countries = await
                _driver.CreateMultipleCountriesAsync(countryCodes, TestContext.Current.CancellationToken);

            foreach (Country country in countries)
            {
                MyCountries.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_contest_with_the_contest_year(int contestYear)
        {
            Guid[] countries = MyCountries.Values.ToArray();

            CreateContestRequest request = new()
            {
                ContestYear = contestYear,
                CityName = "CityName",
                ContestFormat = ContestFormat.Stockholm,
                Group1Participants = countries.Take(3).ToContestParticipantData(),
                Group2Participants = countries.Skip(3).ToContestParticipantData()
            };

            _ = await _driver.CreateContestAsync(request, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_contest(int? contestYear = null,
            string? cityName = null,
            string? contestFormat = null,
            string? group0Participant = null,
            string[]? group1Participants = null,
            string[]? group2Participants = null)
        {
            MyRequirements = new CreateContestRequest
            {
                ContestYear = contestYear ?? 2025,
                CityName = cityName ?? "City Name",
                ContestFormat = Enum.Parse<ContestFormat>(contestFormat ?? "Stockholm"),
                Group0CountryId = group0Participant is not null ? MyCountries[group0Participant] : null,
                Group1Participants = group1Participants?.Select(p => MyCountries[p]).ToContestParticipantData() ?? [],
                Group2Participants = group2Participants?.Select(p => MyCountries[p]).ToContestParticipantData() ?? []
            };

            SendMyRequest = () => _driver.CreateContestAsync(MyRequirements, TestContext.Current.CancellationToken);
        }

        public static AdminActor WithDriverAndBackdoor(AdminApiV1Driver driver, WebAppFixtureBackdoor backdoor) =>
            new(driver, backdoor);

        public void Then_the_contest_should_have_been_correctly_created_from_my_requirements()
        {
            Assert.NotNull(MyRequirements);
            Assert.NotNull(Response);

            Contest createdContest = Response.Contest;

            Assert.Equal(MyRequirements.ContestYear, createdContest.Year);
            Assert.Equal(MyRequirements.CityName, createdContest.CityName);
            Assert.Equal(MyRequirements.ContestFormat, createdContest.Format);
            Assert.Equal(ContestStatus.Initialized, createdContest.Status);
            Assert.Empty(createdContest.BroadcastMemos);
            Assert.Equal(GetExpectedParticipants(MyRequirements), createdContest.Participants);
        }

        public async Task Then_the_created_contest_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(Response);

            Contest createdContest = Response.Contest;
            Contest retrievedContest = await GetContestAsync(createdContest.Id);

            Assert.Equal(createdContest, retrievedContest, EqualityComparers.Contest);
        }

        private async Task<Contest> GetContestAsync(Guid contestId, CancellationToken cancellationToken = default)
        {
            ResponseOrProblem<GetContestResponse> responseOrProblem =
                await _driver.GetContestAsync(contestId, cancellationToken);

            return responseOrProblem.AsT0.Data!.Contest;
        }

        public void Given_the_first_group_1_participant_has_the_act_name(string actName)
        {
            Assert.NotNull(MyRequirements);

            MyRequirements.Group1Participants[0] = MyRequirements.Group1Participants[0] with { ActName = actName };
        }

        public void Given_the_first_group_2_participant_has_the_act_name(string actName)
        {
            Assert.NotNull(MyRequirements);

            MyRequirements.Group2Participants[0] = MyRequirements.Group2Participants[0] with { ActName = actName };
        }

        public void Given_the_first_group_1_participant_has_the_song_title(string songTitle)
        {
            Assert.NotNull(MyRequirements);

            MyRequirements.Group1Participants[0] = MyRequirements.Group1Participants[0] with { SongTitle = songTitle };
        }

        public void Given_the_first_group_2_participant_has_the_song_title(string songTitle)
        {
            Assert.NotNull(MyRequirements);

            MyRequirements.Group2Participants[0] = MyRequirements.Group2Participants[0] with { SongTitle = songTitle };
        }

        public async Task Given_I_have_deleted_the_country(string countryCode) =>
            await _backdoor.DeleteCountryAsync(MyCountries[countryCode]);

        public void Then_the_problem_details_extensions_should_contain_the_orphan_country_ID_for(string countryCode)
        {
            Guid expectedOrphanId = MyCountries[countryCode];

            Assert.NotNull(ProblemDetails);

            Assert.Contains(ProblemDetails.Extensions, kvp => kvp is { Key: "orphanCountryIds", Value: JsonElement j }
                                                              && j.EnumerateArray().Single().GetGuid() == expectedOrphanId);
        }
    }

    private sealed class WebAppFixtureBackdoor(WebAppFixture fixture)
    {
        public async Task DeleteCountryAsync(Guid countryId)
        {
            CountryId targetId = CountryId.FromValue(countryId);

            Func<IServiceProvider, Task> persist = async sp =>
            {
                await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                dbContext.Countries.Remove(dbContext.Countries.First(c => c.Id == targetId));
                await dbContext.SaveChangesAsync();
            };

            await fixture.ExecuteScopedAsync(persist);
        }
    }
}

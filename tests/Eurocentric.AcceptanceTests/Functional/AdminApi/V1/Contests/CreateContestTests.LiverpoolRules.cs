using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests;

public sealed partial class CreateContestTests
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_create_and_return_Liverpool_rules_contest_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_contest(
            contestRules: "Liverpool",
            contestYear: 2023,
            cityName: "Liverpool",
            globalTelevoteCountry: "XX",
            participants: """
            | ParticipatingCountry | SemiFinalDraw | ActName       | SongTitle              |
            |----------------------|---------------|---------------|------------------------|
            | AT                   | SemiFinal1    | Teya & Salena | Who The Hell Is Edgar? |
            | BE                   | SemiFinal1    | Gustaph       | Because Of You         |
            | CZ                   | SemiFinal1    | Vesna         | My Sister's Crown      |
            | DK                   | SemiFinal2    | Reiley        | Breaking My Heart      |
            | EE                   | SemiFinal2    | Alika         | Bridges                |
            | FI                   | SemiFinal2    | Käärijä       | Cha Cha Cha            |
            """
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_response_headers_should_include_the_created_contest_location_for_API_version(apiVersion);
        await admin.Then_the_created_contest_should_match(
            contestRules: "Liverpool",
            contestYear: 2023,
            cityName: "Liverpool",
            queryable: false,
            globalTelevoteCountry: "XX",
            participants: """
            | ParticipatingCountry | SemiFinalDraw | ActName       | SongTitle              |
            |----------------------|---------------|---------------|------------------------|
            | AT                   | SemiFinal1    | Teya & Salena | Who The Hell Is Edgar? |
            | BE                   | SemiFinal1    | Gustaph       | Because Of You         |
            | CZ                   | SemiFinal1    | Vesna         | My Sister's Crown      |
            | DK                   | SemiFinal2    | Reiley        | Breaking My Heart      |
            | EE                   | SemiFinal2    | Alika         | Bridges                |
            | FI                   | SemiFinal2    | Käärijä       | Cha Cha Cha            |
            """
        );
        await admin.Then_the_created_contest_should_have_no_child_broadcasts();
        await admin.Then_the_created_contest_should_be_the_only_existing_contest();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_create_and_return_Liverpool_rules_contest_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");

        admin.Given_I_want_to_create_a_contest(
            contestRules: "Liverpool",
            contestYear: 2025,
            cityName: "Basel",
            globalTelevoteCountry: "XX",
            participants: """
            | ParticipatingCountry | SemiFinalDraw | ActName         | SongTitle                    |
            |----------------------|---------------|-----------------|------------------------------|
            | AT                   | SemiFinal2    | JJ              | Wasted Love                  |
            | BE                   | SemiFinal1    | Red Sebastian   | Strobe Lights                |
            | CZ                   | SemiFinal2    | Adonxs          | Kiss Kiss Goodbye            |
            | EE                   | SemiFinal1    | Tommy Cash      | Espresso Macchiato           |
            | FI                   | SemiFinal2    | Erika Vikman    | ICH KOMME                    |
            | GB                   | SemiFinal2    | Remember Monday | What The Hell Just Happened? |
            | HR                   | SemiFinal1    | Marko Bošnjak   | Poison Cake                  |
            | IT                   | SemiFinal1    | Lucio Corsi     | Volevo Essere Un Duro        |
            """
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_the_response_headers_should_include_the_created_contest_location_for_API_version(apiVersion);
        await admin.Then_the_created_contest_should_match(
            contestRules: "Liverpool",
            contestYear: 2025,
            cityName: "Basel",
            queryable: false,
            globalTelevoteCountry: "XX",
            participants: """
            | ParticipatingCountry | SemiFinalDraw | ActName         | SongTitle                    |
            |----------------------|---------------|-----------------|------------------------------|
            | AT                   | SemiFinal2    | JJ              | Wasted Love                  |
            | BE                   | SemiFinal1    | Red Sebastian   | Strobe Lights                |
            | CZ                   | SemiFinal2    | Adonxs          | Kiss Kiss Goodbye            |
            | EE                   | SemiFinal1    | Tommy Cash      | Espresso Macchiato           |
            | FI                   | SemiFinal2    | Erika Vikman    | ICH KOMME                    |
            | GB                   | SemiFinal2    | Remember Monday | What The Hell Just Happened? |
            | HR                   | SemiFinal1    | Marko Bošnjak   | Poison Cake                  |
            | IT                   | SemiFinal1    | Lucio Corsi     | Volevo Essere Un Duro        |
            """
        );
        await admin.Then_the_created_contest_should_have_no_child_broadcasts();
        await admin.Then_the_created_contest_should_be_the_only_existing_contest();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_contest_year_conflict(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_contest_with_contest_year(2025);

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_contest_year(2025);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Contest year conflict",
            detail: "A contest already exists with the provided contest year."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(key: "contestYear", value: 2025);
        await admin.Then_my_existing_contest_should_be_the_only_existing_contest();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_orphan_contest_country_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_deleted_my_country("AT");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Orphan contest country",
            detail: "Every participant and global televote in a contest must reference an existing country."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_the_deleted_country_ID();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_orphan_contest_country_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_deleted_my_country("FI");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Orphan contest country",
            detail: "Every participant and global televote in a contest must reference an existing country."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_the_deleted_country_ID();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_orphan_contest_country_scenario_3(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_deleted_my_country("XX");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Orphan contest country",
            detail: "Every participant and global televote in a contest must reference an existing country."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_the_deleted_country_ID();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_contest_year_value(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_contest_year(0);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal contest year value",
            detail: "Contest year value must be an integer between 2016 and 2050."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(key: "contestYear", value: 0);
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_city_name_value(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_city_name(" ");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal city name value",
            detail: "City name value must be a non-empty, non-whitespace string of no more than 200 characters."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(key: "cityName", value: " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_act_name_value_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_act_names(
            semiFinal1ActNames: [" ", "ActName", "ActName"],
            semiFinal2ActNames: ["ActName", "ActName", "ActName"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal act name value",
            detail: "Act name value must be a non-empty, non-whitespace string of no more than 200 characters."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(key: "actName", value: " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_act_name_value_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_act_names(
            semiFinal1ActNames: ["ActName", "ActName", "ActName"],
            semiFinal2ActNames: [" ", "ActName", "ActName"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal act name value",
            detail: "Act name value must be a non-empty, non-whitespace string of no more than 200 characters."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(key: "actName", value: " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_song_title_value_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_song_titles(
            semiFinal1SongTitles: [" ", "SongTitle", "SongTitle"],
            semiFinal2SongTitles: ["SongTitle", "SongTitle", "SongTitle"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal song title value",
            detail: "Song title value must be a non-empty, non-whitespace string of no more than 200 characters."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(key: "songTitle", value: " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_song_title_value_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_song_titles(
            semiFinal1SongTitles: ["SongTitle", "SongTitle", "SongTitle"],
            semiFinal2SongTitles: [" ", "SongTitle", "SongTitle"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal song title value",
            detail: "Song title value must be a non-empty, non-whitespace string of no more than 200 characters."
        );
        await admin.Then_the_response_problem_details_extensions_should_include(key: "songTitle", value: " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_contest_countries_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "AT"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal contest countries",
            detail: "Each participant and global televote in a contest must reference a different country."
        );
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_contest_countries_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI", "DK"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal contest countries",
            detail: "Each participant and global televote in a contest must reference a different country."
        );
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_contest_countries_scenario_3(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "FI"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal contest countries",
            detail: "Each participant and global televote in a contest must reference a different country."
        );
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_contest_countries_scenario_4(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "XX"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal contest countries",
            detail: "Each participant and global televote in a contest must reference a different country."
        );
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_contest_countries_scenario_5(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI", "XX"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal contest countries",
            detail: "Each participant and global televote in a contest must reference a different country."
        );
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_global_televote(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal global televote",
            detail: "A Liverpool rules contest must have a global televote. "
                + "A Stockholm rules contest must not have a global televote."
        );
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_participant_counts_scenario_1(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: [],
            semiFinal2Countries: []
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal participant counts",
            detail: "A contest must have at least 3 participants for each semi-final draw."
        );
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_participant_counts_scenario_2(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE"],
            semiFinal2Countries: ["CZ", "DK", "EE", "FI"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal participant counts",
            detail: "A contest must have at least 3 participants for each semi-final draw."
        );
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_Liverpool_rules_illegal_participant_counts_scenario_3(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal participant counts",
            detail: "A contest must have at least 3 participants for each semi-final draw."
        );
        await admin.Then_there_should_be_no_existing_contests();
    }

    private sealed partial class Admin
    {
        public void Given_I_want_to_create_a_Liverpool_rules_contest_with_contest_year(int contestYear)
        {
            CreateContestRequest requestBody = DefaultLiverpoolRulesCreateCountryRequest() with
            {
                ContestYear = contestYear,
            };

            Request = Kernel.Requests.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_rules_contest_with_city_name(string cityName)
        {
            CreateContestRequest requestBody = DefaultLiverpoolRulesCreateCountryRequest() with { CityName = cityName };

            Request = Kernel.Requests.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_rules_contest_with_act_names(
            string[] semiFinal2ActNames = null!,
            string[] semiFinal1ActNames = null!
        )
        {
            CreateContestRequest defaultRequestBody = DefaultLiverpoolRulesCreateCountryRequest();

            IEnumerable<CreateParticipantRequest> amendedSemiFinal1Participants = defaultRequestBody
                .Participants.Where(request => request.SemiFinalDraw == SemiFinalDraw.SemiFinal1)
                .Zip(semiFinal1ActNames, (request, actName) => request with { ActName = actName });

            IEnumerable<CreateParticipantRequest> amendedSemiFinal2Participants = defaultRequestBody
                .Participants.Where(request => request.SemiFinalDraw == SemiFinalDraw.SemiFinal2)
                .Zip(semiFinal2ActNames, (request, actName) => request with { ActName = actName });

            CreateContestRequest amendedRequestBody = defaultRequestBody with
            {
                Participants = amendedSemiFinal1Participants.Concat(amendedSemiFinal2Participants).ToArray(),
            };

            Request = Kernel.Requests.Contests.CreateContest(amendedRequestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_rules_contest_with_song_titles(
            string[] semiFinal2SongTitles = null!,
            string[] semiFinal1SongTitles = null!
        )
        {
            CreateContestRequest defaultRequestBody = DefaultLiverpoolRulesCreateCountryRequest();

            IEnumerable<CreateParticipantRequest> amendedSemiFinal1Participants = defaultRequestBody
                .Participants.Where(request => request.SemiFinalDraw == SemiFinalDraw.SemiFinal1)
                .Zip(semiFinal1SongTitles, (request, songTitle) => request with { SongTitle = songTitle });

            IEnumerable<CreateParticipantRequest> amendedSemiFinal2Participants = defaultRequestBody
                .Participants.Where(request => request.SemiFinalDraw == SemiFinalDraw.SemiFinal2)
                .Zip(semiFinal2SongTitles, (request, songTitle) => request with { SongTitle = songTitle });

            CreateContestRequest amendedRequestBody = defaultRequestBody with
            {
                Participants = amendedSemiFinal1Participants.Concat(amendedSemiFinal2Participants).ToArray(),
            };

            Request = Kernel.Requests.Contests.CreateContest(amendedRequestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_rules_contest_with_countries(
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            string? globalTelevoteCountry = null
        )
        {
            IEnumerable<CreateParticipantRequest> semiFinal1Participants = semiFinal1Countries
                .Select(ExistingCountryIds.GetId)
                .Select(id => new CreateParticipantRequest
                {
                    ParticipatingCountryId = id,
                    SemiFinalDraw = SemiFinalDraw.SemiFinal1,
                    ActName = TestDefaults.ActName,
                    SongTitle = TestDefaults.SongTitle,
                });

            IEnumerable<CreateParticipantRequest> semiFinal2Participants = semiFinal2Countries
                .Select(ExistingCountryIds.GetId)
                .Select(id => new CreateParticipantRequest
                {
                    ParticipatingCountryId = id,
                    SemiFinalDraw = SemiFinalDraw.SemiFinal2,
                    ActName = TestDefaults.ActName,
                    SongTitle = TestDefaults.SongTitle,
                });

            CreateContestRequest requestBody = new()
            {
                ContestYear = TestDefaults.ContestYear,
                CityName = TestDefaults.CityName,
                ContestRules = ContestRules.Liverpool,
                GlobalTelevoteVotingCountryId = globalTelevoteCountry is null
                    ? null
                    : ExistingCountryIds.GetId(globalTelevoteCountry),
                Participants = semiFinal1Participants.Concat(semiFinal2Participants).ToArray(),
            };

            Request = Kernel.Requests.Contests.CreateContest(requestBody);
        }

        private CreateContestRequest DefaultLiverpoolRulesCreateCountryRequest()
        {
            Guid[] allCountryIds = ExistingCountryIds.GetAllIds();

            IEnumerable<CreateParticipantRequest> semiFinal1Participants = allCountryIds
                .Take(3)
                .Select(id => new CreateParticipantRequest
                {
                    ParticipatingCountryId = id,
                    SemiFinalDraw = SemiFinalDraw.SemiFinal1,
                    ActName = TestDefaults.ActName,
                    SongTitle = TestDefaults.SongTitle,
                });

            IEnumerable<CreateParticipantRequest> semiFinal2Participants = allCountryIds
                .Skip(3)
                .Take(3)
                .Select(id => new CreateParticipantRequest
                {
                    ParticipatingCountryId = id,
                    SemiFinalDraw = SemiFinalDraw.SemiFinal2,
                    ActName = TestDefaults.ActName,
                    SongTitle = TestDefaults.SongTitle,
                });

            CreateContestRequest requestBody = new()
            {
                ContestYear = TestDefaults.ContestYear,
                CityName = TestDefaults.CityName,
                ContestRules = ContestRules.Liverpool,
                GlobalTelevoteVotingCountryId = allCountryIds[6],
                Participants = semiFinal1Participants.Concat(semiFinal2Participants).ToArray(),
            };

            return requestBody;
        }
    }
}

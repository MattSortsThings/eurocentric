using CsvHelper.Configuration.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests;
using JetBrains.Annotations;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class CreateContestTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_Stockholm_format_contest_scenario_1(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_contest(
            contestFormat: "Stockholm",
            contestYear: 2016,
            cityName: "Stockholm",
            group1Participants: """
                                | CountryCode | ActName            | SongTitle           |
                                | AT          | ZOË                | Loin d'ici          |
                                | BE          | Laura Tesoro       | What's The Pressure |
                                | CZ          | Gabriela Gunčíková | I Stand             |
                                """,
            group2Participants: """
                                | CountryCode | ActName        | SongTitle        |
                                | DK          | Lighthouse X   | Soldiers Of Love |
                                | EE          | Jüri Pootsmann | Play             |
                                | FI          | Sandhja        | Sing It Away     |
                                """);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_contest_should_match(
            contestFormat: "Stockholm",
            contestYear: 2016,
            cityName: "Stockholm",
            childBroadcasts: 0,
            participants: """
                          | Group | CountryCode | ActName            | SongTitle           |
                          |     1 | AT          | ZOË                | Loin d'ici          |
                          |     1 | BE          | Laura Tesoro       | What's The Pressure |
                          |     1 | CZ          | Gabriela Gunčíková | I Stand             |
                          |     2 | DK          | Lighthouse X       | Soldiers Of Love    |
                          |     2 | EE          | Jüri Pootsmann     | Play                |
                          |     2 | FI          | Sandhja            | Sing It Away        |
                          """);
        await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_Stockholm_format_contest_scenario_2(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_contest(
            contestFormat: "Stockholm",
            contestYear: 2022,
            cityName: "Turin",
            group1Participants: """
                                | CountryCode | ActName               | SongTitle  |
                                | AT          | LUM!X feat. Pia Maria | Halo       |
                                | CZ          | We Are Domi           | Lights Off |
                                | EE          | Stefan                | Hope       |
                                | GB          | Sam Ryder             | SPACE MAN  |
                                | IT          | Mahmoud & BLANCO      | Brividi    |
                                """,
            group2Participants: """
                                | CountryCode | ActName         | SongTitle       |
                                | BE          | Jérémie Makiese | Miss You        |
                                | DK          | REDDI           | The Show        |
                                | FI          | The Rasmus      | Jezebel         |
                                | HR          | Mia Dimšić      | Guilty Pleasure |
                                """);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_contest_should_match(
            contestFormat: "Stockholm",
            contestYear: 2022,
            cityName: "Turin",
            childBroadcasts: 0,
            participants: """
                          | Group | CountryCode | ActName               | SongTitle       |
                          |     1 | AT          | LUM!X feat. Pia Maria | Halo            |
                          |     1 | CZ           | We Are Domi           | Lights Off      |
                          |     1 | EE          | Stefan                | Hope            |
                          |     1 | GB          | Sam Ryder             | SPACE MAN       |
                          |     1 | IT          | Mahmoud & BLANCO      | Brividi         |
                          |     2 | BE          | Jérémie Makiese       | Miss You        |
                          |     2 | DK          | REDDI                 | The Show        |
                          |     2 | FI          | The Rasmus            | Jezebel         |
                          |     2 | HR          | Mia Dimšić            | Guilty Pleasure |
                          """);
        await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_Liverpool_format_contest_scenario_1(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_contest(
            contestFormat: "Liverpool",
            contestYear: 2023,
            cityName: "Liverpool",
            group0CountryCode: "XX",
            group1Participants: """
                                | CountryCode | ActName       | SongTitle              |
                                | AT          | Teya & Salena | Who The Hell Is Edgar? |
                                | BE          | Gustaph       | Because Of You         |
                                | CZ          | Vesna         | My Sister's Crown      |
                                """,
            group2Participants: """
                                | CountryCode | ActName | SongTitle         |
                                | DK          | Reiley  | Breaking My Heart |
                                | EE          | Alika   | Bridges           |
                                | FI          | Käärijä | Cha Cha Cha       |
                                """);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_contest_should_match(
            contestFormat: "Liverpool",
            contestYear: 2023,
            cityName: "Liverpool",
            childBroadcasts: 0,
            participants: """
                          | Group | CountryCode | ActName       | SongTitle              |
                          |     0 | XX          | -             | -                      |
                          |     1 | AT          | Teya & Salena | Who The Hell Is Edgar? |
                          |     1 | BE          | Gustaph       | Because Of You         |
                          |     1 | CZ          | Vesna         | My Sister's Crown      |
                          |     2 | DK          | Reiley        | Breaking My Heart      |
                          |     2 | EE          | Alika         | Bridges                |
                          |     2 | FI          | Käärijä       | Cha Cha Cha            |
                          """);
        await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_create_Liverpool_format_contest_scenario_2(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_contest(
            contestFormat: "Liverpool",
            contestYear: 2025,
            cityName: "Basel",
            group0CountryCode: "XX",
            group1Participants: """
                                | CountryCode | ActName         | SongTitle                    |
                                | AT          | JJ              | Wasted Love                  |
                                | CZ          | ADONXS          | Kiss Kiss Goodbye            |
                                | EE          | Tommy Cash      | Espresso Macchiato           |
                                | GB          | Remember Monday | What The Hell Just Happened? |
                                | IT          | Lucio Corsi     | Volevo essere un duro        |
                                """,
            group2Participants: """
                                | CountryCode | ActName       | SongTitle     |
                                | BE          | Red Sebastian | Strobe Lights |
                                | DK          | Sissal        | Hallucination |
                                | FI          | Erika Vikman  | ICH KOMME     |
                                | HR          | Marko Bošnjak | Poison Cake   |
                                """);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_201_Created();
        admin.Then_the_created_contest_should_match(
            contestFormat: "Liverpool",
            contestYear: 2025,
            cityName: "Basel",
            childBroadcasts: 0,
            participants: """
                          | Group | CountryCode | ActName         | SongTitle                    |
                          |     0 | XX          | -               | -                            |
                          |     1 | AT          | JJ              | Wasted Love                  |
                          |     1 | CZ          | ADONXS          | Kiss Kiss Goodbye            |
                          |     1 | EE          | Tommy Cash      | Espresso Macchiato           |
                          |     1 | GB          | Remember Monday | What The Hell Just Happened? |
                          |     1 | IT          | Lucio Corsi     | Volevo essere un duro        |
                          |     2 | BE          | Red Sebastian   | Strobe Lights                |
                          |     2 | DK          | Sissal          | Hallucination                |
                          |     2 | FI          | Erika Vikman    | ICH KOMME                    |
                          |     2 | HR          | Marko Bošnjak   | Poison Cake                  |
                          """);
        await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_non_unique_contest_year(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest_with_contest_year(2023);
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_contest_year(2023);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Contest year conflict",
            detail: "A contest already exists with the provided contest year.");
        admin.Then_the_problem_details_extensions_should_contain("contestYear", 2023);
        await admin.Then_my_existing_contest_should_be_the_only_existing_contest();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_illegal_contest_year_value(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_contest_year(0);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_contest_year(0);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal contest year value",
            detail: "Contest year value must be an integer between 2016 and 2050.");
        admin.Then_the_problem_details_extensions_should_contain("contestYear", 0);
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_illegal_city_name_value(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_city_name(" ");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal city name value",
            detail: "City name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("cityName", " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_illegal_group_1_participant_act_name_value(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participant_act_names(
            group1ActNames: ["Act", "Act", " "],
            group2ActNames: ["Act", "Act", "Act"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal act name value",
            detail: "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("actName", " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_illegal_group_2_participant_act_name_value(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participant_act_names(
            group1ActNames: ["Act", "Act", "Act"],
            group2ActNames: ["Act", "Act", " "]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal act name value",
            detail: "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("actName", " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_illegal_group_1_participant_song_title_value(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participant_song_titles(
            group1SongTitles: ["Song", "Song", " "],
            group2SongTitles: ["Song", "Song", "Song"]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal song title value",
            detail: "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("songTitle", " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_illegal_group_2_participant_song_title_value(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participant_song_titles(
            group1SongTitles: ["Song", "Song", "Song"],
            group2SongTitles: ["Song", "Song", " "]
        );

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal song title value",
            detail: "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("songTitle", " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_group_0_participant(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal Stockholm format group sizes",
            detail: "A Stockholm format contest must have no participants in group 0, " +
                    "at least 3 in group 1, and at least 3 in group 2.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_fewer_than_3_group_1_participants(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            ["AT", "BE"],
            ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal Stockholm format group sizes",
            detail: "A Stockholm format contest must have no participants in group 0, " +
                    "at least 3 in group 1, and at least 3 in group 2.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_fewer_than_3_group_2_participants(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            ["AT", "BE", "CZ"],
            ["DK", "EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal Stockholm format group sizes",
            detail: "A Stockholm format contest must have no participants in group 0, " +
                    "at least 3 in group 1, and at least 3 in group 2.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_group_1_participants_from_same_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            ["AT", "BE", "CZ", "AT"],
            ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating countries",
            detail: "Every participant in a contest must reference a different participating country.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_group_2_participants_from_same_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            ["AT", "BE", "CZ"],
            ["DK", "EE", "FI", "DK"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating countries",
            detail: "Every participant in a contest must reference a different participating country.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_group_1_and_2_participants_from_same_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            ["AT", "BE", "CZ", "FI"],
            ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating countries",
            detail: "Every participant in a contest must reference a different participating country.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_group_1_participant_from_non_existent_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_deleted_my_country_with_country_code("IT");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            ["AT", "BE", "CZ", "IT"],
            ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Orphan participant",
            detail: "No country exists with the provided participating country ID.");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "participatingCountryId",
            countryCode: "IT");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Stockholm_format_contest_with_group_2_participant_from_non_existent_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_deleted_my_country_with_country_code("IT");
        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            ["AT", "BE", "CZ"],
            ["DK", "EE", "FI", "IT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Orphan participant",
            detail: "No country exists with the provided participating country ID.");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "participatingCountryId",
            countryCode: "IT");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_non_unique_contest_year(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_created_a_contest_with_contest_year(2023);
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_contest_year(2023);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Contest year conflict",
            detail: "A contest already exists with the provided contest year.");
        admin.Then_the_problem_details_extensions_should_contain("contestYear", 2023);
        await admin.Then_my_existing_contest_should_be_the_only_existing_contest();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_illegal_contest_year_value(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_contest_year(0);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal contest year value",
            detail: "Contest year value must be an integer between 2016 and 2050.");
        admin.Then_the_problem_details_extensions_should_contain("contestYear", 0);
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_illegal_city_name_value(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_city_name(" ");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal city name value",
            detail: "City name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("cityName", " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_illegal_group_1_participant_act_name_value(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participant_act_names(
            group1ActNames: ["Act", "Act", " "],
            group2ActNames: ["Act", "Act", "Act"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal act name value",
            detail: "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("actName", " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_illegal_group_2_participant_act_name_value(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participant_act_names(
            group1ActNames: ["Act", "Act", "Act"],
            group2ActNames: ["Act", "Act", " "]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal act name value",
            detail: "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("actName", " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_illegal_group_1_participant_song_title_value(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participant_song_titles(
            group1SongTitles: ["Song", "Song", " "],
            group2SongTitles: ["Song", "Song", "Song"]);


        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal song title value",
            detail: "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("songTitle", " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_illegal_group_2_participant_song_title_value(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participant_song_titles(
            group1SongTitles: ["Song", "Song", "Song"],
            group2SongTitles: ["Song", "Song", " "]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal song title value",
            detail: "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("songTitle", " ");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_no_group_0_participant(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            ["AT", "BE", "CZ"],
            ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal Liverpool format group sizes",
            detail: "A Liverpool format contest must have a single participant in group 0, " +
                    "at least 3 in group 1, and at least 3 in group 2.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_fewer_than_3_group_1_participants(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            ["AT", "BE"],
            ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal Liverpool format group sizes",
            detail: "A Liverpool format contest must have a single participant in group 0, " +
                    "at least 3 in group 1, and at least 3 in group 2.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_fewer_than_3_group_2_participants(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            ["AT", "BE", "CZ"],
            ["DK", "EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal Liverpool format group sizes",
            detail: "A Liverpool format contest must have a single participant in group 0, " +
                    "at least 3 in group 1, and at least 3 in group 2.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_group_1_participants_from_same_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ", "AT"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating countries",
            detail: "Every participant in a contest must reference a different participating country.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_group_2_participants_from_same_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI", "DK"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating countries",
            detail: "Every participant in a contest must reference a different participating country.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_group_0_and_1_participants_from_same_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ", "XX"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating countries",
            detail: "Every participant in a contest must reference a different participating country.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_group_0_and_2_participants_from_same_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI", "XX"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating countries",
            detail: "Every participant in a contest must reference a different participating country.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_group_1_and_2_participants_from_same_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ", "FI"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_422_UnprocessableEntity();
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Duplicate participating countries",
            detail: "Every participant in a contest must reference a different participating country.");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_group_0_participant_from_non_existent_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_deleted_my_country_with_country_code("IT");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "IT",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Orphan participant",
            detail: "No country exists with the provided participating country ID.");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "participatingCountryId",
            countryCode: "IT");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_group_1_participant_from_non_existent_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_deleted_my_country_with_country_code("IT");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ", "IT"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Orphan participant",
            detail: "No country exists with the provided participating country ID.");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "participatingCountryId",
            countryCode: "IT");
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_Liverpool_format_contest_with_group_2_participant_from_non_existent_country(
        string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "IT", "XX");
        await admin.Given_I_have_deleted_my_country_with_country_code("IT");
        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI", "IT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Orphan participant",
            detail: "No country exists with the provided participating country ID.");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "participatingCountryId",
            countryCode: "IT");
        await admin.Then_there_should_be_no_existing_contests();
    }

    private sealed class AdminActor : ActorWithResponse<CreateContestResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(10);

        private Contest? MyExistingContest { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Country[] myCountries = await ApiDriver.Countries.CreateMultipleCountriesAsync(countryCodes,
                TestContext.Current.CancellationToken);

            foreach (Country country in myCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_contest_with_contest_year(int contestYear)
        {
            Guid[] myCountryIds = MyCountryCodesAndIds.Values.ToArray();

            MyExistingContest = await ApiDriver.Contests.CreateAContestAsync(contestFormat: ContestFormat.Stockholm,
                cityName: "CityName",
                contestYear: contestYear,
                group1CountryIds: myCountryIds.Take(3),
                group2CountryIds: myCountryIds.Skip(3).Take(3),
                cancellationToken: TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_deleted_my_country_with_country_code(string countryCode)
        {
            Guid myCountryId = MyCountryCodesAndIds[countryCode];

            await ApiDriver.Countries.DeleteCountry(myCountryId, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_contest(int contestYear = 0,
            string cityName = "",
            string contestFormat = "",
            string? group0CountryCode = null,
            string group1Participants = "",
            string group2Participants = "")
        {
            CreateContestRequest requestBody = new()
            {
                ContestYear = contestYear,
                CityName = cityName,
                ContestFormat = Enum.Parse<ContestFormat>(contestFormat),
                Group0CountryId = group0CountryCode is null ? null : MyCountryCodesAndIds[group0CountryCode],
                Group1Participants = CreateParticipantSpecifications(group1Participants.ParseItems<GivenParticipant>()),
                Group2Participants = CreateParticipantSpecifications(group2Participants.ParseItems<GivenParticipant>())
            };

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_Stockholm_format_contest_with_contest_year(int contestYear)
        {
            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest() with { ContestYear = contestYear };

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_Stockholm_format_contest_with_city_name(string cityName)
        {
            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest() with { CityName = cityName };

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_Stockholm_format_contest_with_participant_act_names(string[]? group2ActNames = null,
            string[]? group1ActNames = null)
        {
            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest();

            requestBody = requestBody with
            {
                Group1Participants = CopyWithActNames(requestBody.Group1Participants, group1ActNames),
                Group2Participants = CopyWithActNames(requestBody.Group2Participants, group2ActNames)
            };

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_Stockholm_format_contest_with_participant_song_titles(
            string[]? group2SongTitles = null,
            string[]? group1SongTitles = null)
        {
            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest();

            requestBody = requestBody with
            {
                Group1Participants = CopyWithSongTitles(requestBody.Group1Participants, group1SongTitles),
                Group2Participants = CopyWithSongTitles(requestBody.Group2Participants, group2SongTitles)
            };

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            string[]? group1CountryCodes = null,
            string[]? group2CountryCodes = null,
            string? group0CountryCode = null)
        {
            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest() with
            {
                Group0CountryId = group0CountryCode is null
                    ? null
                    : MyCountryCodesAndIds[group0CountryCode],
                Group1Participants = group1CountryCodes is null
                    ? []
                    : group1CountryCodes.Select(code => MyCountryCodesAndIds[code]).ToParticipantSpecifications(),
                Group2Participants = group2CountryCodes is null
                    ? []
                    : group2CountryCodes.Select(code => MyCountryCodesAndIds[code]).ToParticipantSpecifications()
            };

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_contest_year(int contestYear)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest() with { ContestYear = contestYear };

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_city_name(string cityName)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest() with { CityName = cityName };

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            string[]? group1CountryCodes = null,
            string[]? group2CountryCodes = null,
            string? group0CountryCode = null)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest() with
            {
                Group0CountryId = group0CountryCode is null
                    ? null
                    : MyCountryCodesAndIds[group0CountryCode],
                Group1Participants = group1CountryCodes is null
                    ? []
                    : group1CountryCodes.Select(code => MyCountryCodesAndIds[code]).ToParticipantSpecifications(),
                Group2Participants = group2CountryCodes is null
                    ? []
                    : group2CountryCodes.Select(code => MyCountryCodesAndIds[code]).ToParticipantSpecifications()
            };

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_participant_act_names(string[]? group2ActNames = null,
            string[]? group1ActNames = null)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest();

            requestBody = requestBody with
            {
                Group1Participants = CopyWithActNames(requestBody.Group1Participants, group1ActNames),
                Group2Participants = CopyWithActNames(requestBody.Group2Participants, group2ActNames)
            };

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_participant_song_titles(
            string[]? group2SongTitles = null,
            string[]? group1SongTitles = null)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest();

            requestBody = requestBody with
            {
                Group1Participants = CopyWithSongTitles(requestBody.Group1Participants, group1SongTitles),
                Group2Participants = CopyWithSongTitles(requestBody.Group2Participants, group2SongTitles)
            };

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateContest(requestBody, TestContext.Current.CancellationToken);
        }

        public async Task Then_the_created_contest_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(ResponseObject);

            Contest createdContest = ResponseObject.Contest;
            Contest retrievedContest =
                await ApiDriver.Contests.GetAContestAsync(createdContest.Id, TestContext.Current.CancellationToken);

            Assert.Equal(createdContest, retrievedContest, new ContestEqualityComparer());
        }

        public void Then_the_created_contest_should_match(int contestYear = 0,
            string cityName = "",
            string contestFormat = "",
            int childBroadcasts = 0,
            string participants = "")
        {
            Assert.NotNull(ResponseObject);

            Contest createdContest = ResponseObject.Contest;
            ContestFormat expectedContestFormat = Enum.Parse<ContestFormat>(contestFormat);
            IOrderedEnumerable<Participant> expectedParticipants =
                CreateParticipants(participants.ParseItems<ExpectedParticipant>())
                    .OrderBy(participant => participant.ParticipantGroup)
                    .ThenBy(participant => participant.ParticipatingCountryId);

            Assert.Equal(contestYear, createdContest.ContestYear);
            Assert.Equal(cityName, createdContest.CityName);
            Assert.Equal(expectedContestFormat, createdContest.ContestFormat);
            Assert.Equal(childBroadcasts, createdContest.ChildBroadcasts.Length);
            Assert.Equal(expectedParticipants, createdContest.Participants);
        }

        public void Then_the_problem_details_extensions_should_contain_the_country_ID(string countryCode = "", string key = "")
        {
            Assert.NotNull(ResponseProblemDetails);

            Guid expectedId = MyCountryCodesAndIds[countryCode];

            Then_the_problem_details_extensions_should_contain(key, expectedId);
        }

        public async Task Then_my_existing_contest_should_be_the_only_existing_contest()
        {
            Assert.NotNull(MyExistingContest);

            Contest[] existingContests = await ApiDriver.Contests.GetAllContestAsync(TestContext.Current.CancellationToken);

            Assert.Single(existingContests);
            Assert.Contains(MyExistingContest, existingContests, new ContestEqualityComparer());
        }

        public async Task Then_there_should_be_no_existing_contests()
        {
            Contest[] existingContests = await ApiDriver.Contests.GetAllContestAsync(TestContext.Current.CancellationToken);

            Assert.Empty(existingContests);
        }

        private CreateContestRequest CreateDefaultStockholmFormatContest()
        {
            Guid[] myCountryIds = MyCountryCodesAndIds.Values.ToArray();

            return new CreateContestRequest
            {
                ContestYear = 2025,
                CityName = "CityName",
                ContestFormat = ContestFormat.Stockholm,
                Group0CountryId = null,
                Group1Participants = myCountryIds.Take(3).ToParticipantSpecifications(),
                Group2Participants = myCountryIds.Skip(3).Take(3).ToParticipantSpecifications()
            };
        }

        private CreateContestRequest CreateDefaultLiverpoolFormatContest()
        {
            Guid[] myCountryIds = MyCountryCodesAndIds.Values.ToArray();

            return new CreateContestRequest
            {
                ContestYear = 2025,
                CityName = "CityName",
                ContestFormat = ContestFormat.Liverpool,
                Group0CountryId = myCountryIds[6],
                Group1Participants = myCountryIds.Take(3).ToParticipantSpecifications(),
                Group2Participants = myCountryIds.Skip(3).Take(3).ToParticipantSpecifications()
            };
        }

        private ParticipantSpecification[] CreateParticipantSpecifications(IEnumerable<GivenParticipant> givenParticipants) =>
            givenParticipants.Select(participant => new ParticipantSpecification
            {
                CountryId = MyCountryCodesAndIds[participant.CountryCode],
                ActName = participant.ActName,
                SongTitle = participant.SongTitle
            }).ToArray();

        private Participant[] CreateParticipants(IEnumerable<ExpectedParticipant> participants) => participants
            .Select(participant => new Participant
            {
                ParticipantGroup = participant.Group,
                ParticipatingCountryId = MyCountryCodesAndIds[participant.CountryCode],
                ActName = participant.ActName,
                SongTitle = participant.SongTitle
            }).ToArray();

        private static ParticipantSpecification[] CopyWithActNames(IEnumerable<ParticipantSpecification> participants,
            string[]? actNames) => actNames is null
            ? []
            : participants.Zip(actNames, (participant, actName) => participant with { ActName = actName }).ToArray();

        private static ParticipantSpecification[] CopyWithSongTitles(IEnumerable<ParticipantSpecification> participants,
            string[]? songTitles)
        {
            if (songTitles is null)
            {
                return [];
            }

            return participants.Zip(songTitles, (participant, songTitle) => participant with { SongTitle = songTitle })
                .ToArray();
        }

        [UsedImplicitly]
        private sealed record GivenParticipant
        {
            public required string CountryCode { get; [UsedImplicitly] init; }

            public required string ActName { get; [UsedImplicitly] init; }

            public required string SongTitle { get; [UsedImplicitly] init; }
        }

        private sealed record ExpectedParticipant
        {
            public required int Group { get; [UsedImplicitly] init; }

            public required string CountryCode { get; [UsedImplicitly] init; }

            [NullValues("-")]
            public required string ActName { get; [UsedImplicitly] init; }

            [NullValues("-")]
            public required string SongTitle { get; [UsedImplicitly] init; }
        }
    }
}

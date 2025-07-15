using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public static partial class CreateContestTests
{
    public sealed partial class Endpoint
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_create_and_return_Liverpool_format_contest_scenario_1(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_contest(
                contestFormat: "Liverpool",
                contestYear: 2023,
                cityName: "Liverpool",
                group0ParticipatingCountryCode: "XX",
                group1Participants: """
                                    | CountryCode | ActName       | SongTitle              |
                                    |:------------|:--------------|:-----------------------|
                                    | AT          | Teya & Salena | Who The Hell Is Edgar? |
                                    | BE          | Gustaph       | Because Of You         |
                                    | CZ          | Vesna         | My Sister's Crown      |
                                    """,
                group2Participants: """
                                    | CountryCode | ActName | SongTitle         |
                                    |:------------|:--------|:------------------|
                                    | DK          | Reiley  | Breaking My Heart |
                                    | EE          | Alika   | Bridges           |
                                    | FI          | Käärijä | Cha Cha Cha       |
                                    """);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(201);
            admin.Then_the_created_contest_should_match(
                contestFormat: "Liverpool",
                contestYear: 2023,
                cityName: "Liverpool",
                completed: false,
                childBroadcasts: 0,
                participants: """
                              | CountryCode | ActName       | SongTitle              | Group |
                              |:------------|:--------------|:-----------------------|------:|
                              | XX          |               |                        |     0 |
                              | AT          | Teya & Salena | Who The Hell Is Edgar? |     1 |
                              | BE          | Gustaph       | Because Of You         |     1 |
                              | CZ          | Vesna         | My Sister's Crown      |     1 |
                              | DK          | Reiley        | Breaking My Heart      |     2 |
                              | EE          | Alika         | Bridges                |     2 |
                              | FI          | Käärijä       | Cha Cha Cha            |     2 |
                              """);
            await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_create_and_return_Liverpool_format_contest_scenario_2(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "IT", "NO", "XX");
            admin.Given_I_want_to_create_a_contest(
                contestFormat: "Liverpool",
                contestYear: 2025,
                cityName: "Basel",
                group0ParticipatingCountryCode: "XX",
                group1Participants: """
                                    | CountryCode | ActName         | SongTitle                    |
                                    |:------------|:----------------|:-----------------------------|
                                    | BE          | Red Sebastian   | Strobe Lights                |
                                    | DK          | Sissal          | Hallucination                |
                                    | EE          | Tommy Cash      | Espresso Macchiato           |
                                    | GB          | Remember Monday | What The Hell Just Happened? |
                                    """,
                group2Participants: """
                                    | CountryCode | ActName     | SongTitle             |
                                    |:------------|:------------|:----------------------|
                                    | AT          | JJ          | Wasted Love           |
                                    | CZ          | Adonxs      | Kiss Kiss Goodbye     |
                                    | IT          | Lucio Corsi | Volevo Essere Un Duro |
                                    """);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(201);
            admin.Then_the_created_contest_should_match(
                contestFormat: "Liverpool",
                contestYear: 2025,
                cityName: "Basel",
                completed: false,
                childBroadcasts: 0,
                participants: """
                              | CountryCode | ActName         | SongTitle                    | Group |
                              |:------------|:----------------|:-----------------------------|------:|
                              | XX          |                 |                              |     0 |
                              | BE          | Red Sebastian   | Strobe Lights                |     1 |
                              | DK          | Sissal          | Hallucination                |     1 |
                              | EE          | Tommy Cash      | Espresso Macchiato           |     1 |
                              | GB          | Remember Monday | What The Hell Just Happened? |     1 |
                              | AT          | JJ              | Wasted Love                  |     2 |
                              | CZ          | Adonxs          | Kiss Kiss Goodbye            |     2 |
                              | IT          | Lucio Corsi     | Volevo Essere Un Duro        |     2 |
                              """);
            await admin.Then_the_created_contest_should_be_retrievable_by_its_ID();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_orphan_group_0_participating_country_ID(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            await admin.Given_I_have_deleted_the_country("XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(404);
            admin.Then_the_response_problem_details_should_match(status: 404,
                title: "Orphan participating country ID",
                detail: "No country exists with the provided country ID.");
            admin.Then_the_problem_details_extensions_should_contain_the_country_ID_for_the_country("XX");
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_orphan_group_1_participating_country_ID(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            await admin.Given_I_have_deleted_the_country("CZ");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(404);
            admin.Then_the_response_problem_details_should_match(status: 404,
                title: "Orphan participating country ID",
                detail: "No country exists with the provided country ID.");
            admin.Then_the_problem_details_extensions_should_contain_the_country_ID_for_the_country("CZ");
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_orphan_group_2_participating_country_ID(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            await admin.Given_I_have_deleted_the_country("FI");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(404);
            admin.Then_the_response_problem_details_should_match(status: 404,
                title: "Orphan participating country ID",
                detail: "No country exists with the provided country ID.");
            admin.Then_the_problem_details_extensions_should_contain_the_country_ID_for_the_country("FI");
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_non_unique_contest_year(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            await admin.Given_I_have_created_a_Liverpool_format_contest(
                contestYear: 2025,
                cityName: "Basel",
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_contest_year(2025);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(409);
            admin.Then_the_response_problem_details_should_match(status: 409,
                title: "Contest year conflict",
                detail: "A contest already exists with the provided contest year.");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "contestYear", value: 2025);
            await admin.Then_my_given_contest_should_be_the_only_existing_contest();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_illegal_contest_year_value(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_contest_year(3000);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal contest year value",
                detail: "Contest year value must be an integer between 2016 and 2050.");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "contestYear", value: 3000);
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_illegal_city_name_value(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_city_name(" ");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal city name value",
                detail: "City name value must be a non-empty, non-whitespace string of no more than 200 characters.");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "cityName", value: " ");
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_illegal_group_1_participant_act_name_value(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_1_participant(
                actName: " ",
                songTitle: "ArbitrarySongTitle");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal act name value",
                detail: "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "actName", value: " ");
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_illegal_group_2_participant_act_name_value(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_2_participant(
                actName: " ",
                songTitle: "ArbitrarySongTitle");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal act name value",
                detail: "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "actName", value: " ");
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_illegal_group_1_participant_song_title_value(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_1_participant(
                actName: "ArbitraryActName",
                songTitle: " ");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal song title value",
                detail: "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "songTitle", value: " ");
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_illegal_group_2_participant_song_title_value(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_2_participant(
                actName: "ArbitraryActName",
                songTitle: " ");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal song title value",
                detail: "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "songTitle", value: " ");
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_duplicate_group_0_and_group_1_participating_country_IDs(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ", "XX"],
                group2CountryCodes: ["DK", "EE", "FI"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Duplicate participating country IDs",
                detail: "Each participant in a contest must have a participating country ID referencing a different country.");
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_duplicate_group_0_and_group_2_participating_country_IDs(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI", "XX"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Duplicate participating country IDs",
                detail: "Each participant in a contest must have a participating country ID referencing a different country.");
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_duplicate_group_1_and_group_2_participating_country_IDs(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ", "FI"],
                group2CountryCodes: ["DK", "EE", "FI"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Duplicate participating country IDs",
                detail: "Each participant in a contest must have a participating country ID referencing a different country.");
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_duplicate_group_1_participating_country_IDs(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ", "AT"],
                group2CountryCodes: ["DK", "EE", "FI"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Duplicate participating country IDs",
                detail: "Each participant in a contest must have a participating country ID referencing a different country.");
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_duplicate_group_2_participating_country_IDs(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI", "DK"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Duplicate participating country IDs",
                detail: "Each participant in a contest must have a participating country ID referencing a different country.");
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_no_group_0_participant(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal Liverpool format participant groups",
                detail: "A Liverpool format contest must have a single group 0 participant, " +
                        "at least three group 1 participants, and at least three group 2 participants.");
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_fewer_than_three_group_1_participants(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
                group0CountryCode: "XX",
                group1CountryCodes: ["BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal Liverpool format participant groups",
                detail: "A Liverpool format contest must have a single group 0 participant, " +
                        "at least three group 1 participants, and at least three group 2 participants.");
            await admin.Then_no_contests_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Liverpool_format_contest_with_fewer_than_three_group_2_participants(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["EE", "FI"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal Liverpool format participant groups",
                detail: "A Liverpool format contest must have a single group 0 participant, " +
                        "at least three group 1 participants, and at least three group 2 participants.");
            await admin.Then_no_contests_should_exist();
        }
    }

    private sealed partial class Admin
    {
        public void Given_I_want_to_create_a_Liverpool_format_contest_with_contest_year(int contestYear)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContestRequestBody() with
            {
                ContestYear = contestYear
            };

            Request = RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_city_name(string cityName)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContestRequestBody() with { CityName = cityName };

            Request = RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_1_participant(string songTitle = "SongTitle",
            string actName = "ActName")
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContestRequestBody();

            requestBody.Group1Participants[0] =
                requestBody.Group1Participants[0] with { ActName = actName, SongTitle = songTitle };

            Request = RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_2_participant(string songTitle = "SongTitle",
            string actName = "ActName")
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContestRequestBody();

            requestBody.Group2Participants[0] =
                requestBody.Group2Participants[0] with { ActName = actName, SongTitle = songTitle };

            Request = RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string? group0CountryCode = null)
        {
            CreateContestRequest requestBody = new()
            {
                ContestYear = DefaultContestYear,
                CityName = DefaultCityName,
                ContestFormat = ContestFormat.Liverpool,
                Group0ParticipatingCountryId = group0CountryCode is null ? null : GivenCountries.GetId(group0CountryCode),
                Group1Participants = group1CountryCodes.Select(GivenCountries.GetId).ToContestParticipantSpecifications(),
                Group2Participants = group2CountryCodes.Select(GivenCountries.GetId).ToContestParticipantSpecifications()
            };

            Request = RequestFactory.Contests.CreateContest(requestBody);
        }

        private CreateContestRequest CreateDefaultLiverpoolFormatContestRequestBody()
        {
            Guid[] countryIds = GivenCountries.GetAllCountries().Select(country => country.Id).ToArray();

            return new CreateContestRequest
            {
                ContestYear = 2025,
                CityName = "CityName",
                ContestFormat = ContestFormat.Liverpool,
                Group0ParticipatingCountryId = countryIds[6],
                Group1Participants = countryIds.Take(3)
                    .ToContestParticipantSpecifications(),
                Group2Participants = countryIds.Skip(3)
                    .Take(3)
                    .ToContestParticipantSpecifications()
            };
        }
    }
}

using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests.CreateContest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed partial class CreateContestTests
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_create_and_return_Liverpool_format_contest_scenario_1(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_contest_for_my_countries(
            contestFormat: "Liverpool",
            contestYear: 2023,
            cityName: "Liverpool",
            group0CountryCode: "XX",
            group1Participants: """
                                | CountryCode | ActName       | SongTitle              |
                                |-------------|---------------|------------------------|
                                | AT          | Teya & Salena | Who The Hell Is Edgar? |
                                | BE          | Gustaph       | Because Of You         |
                                | CZ          | Vesna         | My Sister's Crown      |
                                """,
            group2Participants: """
                                | CountryCode | ActName | SongTitle         |
                                |-------------|---------|-------------------|
                                | DK          | Reiley  | Breaking My Heart |
                                | EE          | Alika   | Bridges           |
                                | FI          | Käärijä | Cha Cha Cha       |
                                """);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_created_contest_should_match(
            contestFormat: "Liverpool",
            contestYear: 2023,
            cityName: "Liverpool",
            completed: false,
            participants: """
                          | Group | CountryCode | ActName       | SongTitle              |
                          |-------|-------------|---------------|------------------------|
                          | 0     | XX          |               |                        |
                          | 1     | AT          | Teya & Salena | Who The Hell Is Edgar? |
                          | 1     | BE          | Gustaph       | Because Of You         |
                          | 1     | CZ          | Vesna         | My Sister's Crown      |
                          | 2     | DK          | Reiley        | Breaking My Heart      |
                          | 2     | EE          | Alika         | Bridges                |
                          | 2     | FI          | Käärijä       | Cha Cha Cha            |
                          """);
        await admin.Then_the_created_contest_should_have_no_child_broadcasts();
        await admin.Then_the_created_contest_should_be_the_only_existing_contest_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_create_and_return_Liverpool_format_contest_scenario_2(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CH", "DK", "EE", "FI", "GB", "IT", "XX");

        admin.Given_I_want_to_create_a_contest_for_my_countries(contestFormat: "Liverpool",
            contestYear: 2025,
            cityName: "Basel",
            group0CountryCode: "XX",
            group1Participants: """
                                | CountryCode | ActName       | SongTitle             |
                                |-------------|---------------|-----------------------|
                                | BE          | Red Sebastian | Strobe Lights         |
                                | CH          | Zoë Më        | Voyage                |
                                | EE          | Tommy Cash    | Espresso Macchiato    |
                                | IT          | Lucio Corsi   | Volevo Essere Un Duro |
                                """,
            group2Participants: """
                                | CountryCode | ActName         | SongTitle                    |
                                |-------------|-----------------|------------------------------|
                                | AT          | JJ              | Wasted Love                  |
                                | DK          | Sissal          | Hallucination                |
                                | GB          | Remember Monday | What The Hell Just Happened? |
                                """);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_created_contest_should_match(
            contestFormat: "Liverpool",
            contestYear: 2025,
            cityName: "Basel",
            completed: false,
            participants: """
                          | Group | CountryCode | ActName         | SongTitle                    |
                          |-------|-------------|-----------------|------------------------------|
                          | 0     | XX          |                 |                              |
                          | 1     | BE          | Red Sebastian   | Strobe Lights                |
                          | 1     | CH          | Zoë Më          | Voyage                       |
                          | 1     | EE          | Tommy Cash      | Espresso Macchiato           |
                          | 1     | IT          | Lucio Corsi     | Volevo Essere Un Duro        |
                          | 2     | AT          | JJ              | Wasted Love                  |
                          | 2     | DK          | Sissal          | Hallucination                |
                          | 2     | GB          | Remember Monday | What The Hell Just Happened? |
                          """);
        await admin.Then_the_created_contest_should_have_no_child_broadcasts();
        await admin.Then_the_created_contest_should_be_the_only_existing_contest_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_non_existent_country_as_group_0_participant(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "XX");
        await admin.Given_I_have_deleted_my_country("GB");

        await admin.Given_I_want_to_create_a_Liverpool_format_contest_with_my_deleted_country_as_the_group_0_participant();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_404_NotFound();
        await admin.Then_the_response_problem_details_should_match(status: 404,
            title: "Country not found",
            detail: "No country exists with the provided country ID.");
        await admin.Then_the_response_problem_details_extensions_should_include_my_deleted_country_ID();
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_non_existent_country_as_group_1_participant(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "XX");
        await admin.Given_I_have_deleted_my_country("GB");

        await admin.Given_I_want_to_create_a_Liverpool_format_contest_with_my_deleted_country_as_a_group_1_participant();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_404_NotFound();
        await admin.Then_the_response_problem_details_should_match(status: 404,
            title: "Country not found",
            detail: "No country exists with the provided country ID.");
        await admin.Then_the_response_problem_details_extensions_should_include_my_deleted_country_ID();
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_non_existent_country_as_group_2_participant(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "XX");
        await admin.Given_I_have_deleted_my_country("GB");

        await admin.Given_I_want_to_create_a_Liverpool_format_contest_with_my_deleted_country_as_a_group_2_participant();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_404_NotFound();
        await admin.Then_the_response_problem_details_should_match(status: 404,
            title: "Country not found",
            detail: "No country exists with the provided country ID.");
        await admin.Then_the_response_problem_details_extensions_should_include_my_deleted_country_ID();
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_non_unique_contest_year(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(
            contestYear: 2023,
            cityName: "Liverpool",
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_contest_year(2023);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Contest year conflict",
            detail: "A contest already exists with the provided contest year.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_contest_year(2023);
        await admin.Then_my_given_contest_should_be_the_only_existing_contest_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_illegal_contest_year_value(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_contest_year(2015);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal contest year value",
            detail: "Contest year value must be an integer between 2016 and 2050.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_contest_year(2015);
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_illegal_city_name_value(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_city_name(" ");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal city name value",
            detail: "City name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_city_name(" ");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_illegal_group_1_participant_act_name_value(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_1_participant_act_name(" ");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal act name value",
            detail: "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_act_name(" ");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_illegal_group_2_participant_act_name_value(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_2_participant_act_name(" ");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal act name value",
            detail: "Act name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_act_name(" ");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_illegal_group_1_participant_song_title_value(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_1_participant_song_title(" ");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal song title value",
            detail: "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_song_title(" ");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_illegal_group_2_participant_song_title_value(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_2_participant_song_title(" ");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal song title value",
            detail: "Song title value must be a non-empty, non-whitespace string of no more than 200 characters.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_song_title(" ");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_duplicate_group_0_and_1_participating_country_IDs(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ", "XX"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Duplicate participating country IDs",
            detail: "Every participant in a contest must have a different participating country ID.");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_duplicate_group_0_and_2_participating_country_IDs(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI", "XX"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Duplicate participating country IDs",
            detail: "Every participant in a contest must have a different participating country ID.");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_duplicate_group_1_and_2_participating_country_IDs(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI", "AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Duplicate participating country IDs",
            detail: "Every participant in a contest must have a different participating country ID.");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_duplicate_group_1_participating_country_IDs(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ", "AT"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Duplicate participating country IDs",
            detail: "Every participant in a contest must have a different participating country ID.");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_duplicate_group_2_participating_country_IDs(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI", "DK"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Duplicate participating country IDs",
            detail: "Every participant in a contest must have a different participating country ID.");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_no_group_0_participant(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "DK", "EE", "FI");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group1CountryCodes: ["AT", "BE"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal Liverpool format participant groups",
            detail: "A Liverpool format contest must have one group 0 participant, " +
                    "at least three group 1 participants, and at least three group 2 participants.");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_fewer_than_three_group_1_participants(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal Liverpool format participant groups",
            detail: "A Liverpool format contest must have one group 0 participant, " +
                    "at least three group 1 participants, and at least three group 2 participants.");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Liverpool_format_contest_with_fewer_than_three_group_2_participants(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "XX");

        admin.Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal Liverpool format participant groups",
            detail: "A Liverpool format contest must have one group 0 participant, " +
                    "at least three group 1 participants, and at least three group 2 participants.");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    private sealed partial class AdminActor
    {
        public async Task Given_I_have_created_a_Liverpool_format_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string group0CountryCode = "",
            string cityName = "",
            int contestYear = 0)
        {
            Guid[] group1CountryIds = group1CountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();
            Guid[] group2CountryIds = group2CountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();

            MyExistingContest = await ApiDriver.CreateSingleLiverpoolFormatContestAsync(contestYear: contestYear,
                cityName: cityName,
                group0CountryId: MyCountryCodesAndIds[group0CountryCode],
                group1CountryIds: group1CountryIds,
                group2CountryIds: group2CountryIds);
        }

        public async Task Given_I_want_to_create_a_Liverpool_format_contest_with_my_deleted_country_as_the_group_0_participant()
        {
            Guid myDeletedCountryId = await Assert.That(MyDeletedCountryId).IsNotNull();

            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest() with
            {
                Group0ParticipatingCountryId = myDeletedCountryId
            };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public async Task Given_I_want_to_create_a_Liverpool_format_contest_with_my_deleted_country_as_a_group_1_participant()
        {
            Guid myDeletedCountryId = await Assert.That(MyDeletedCountryId).IsNotNull();

            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest();

            requestBody.Group1ParticipantData[0] = requestBody.Group1ParticipantData[0] with
            {
                ParticipatingCountryId = myDeletedCountryId
            };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public async Task Given_I_want_to_create_a_Liverpool_format_contest_with_my_deleted_country_as_a_group_2_participant()
        {
            Guid myDeletedCountryId = await Assert.That(MyDeletedCountryId).IsNotNull();

            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest();

            requestBody.Group2ParticipantData[0] = requestBody.Group2ParticipantData[0] with
            {
                ParticipatingCountryId = myDeletedCountryId
            };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_contest_year(int contestYear)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest() with { ContestYear = contestYear };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_city_name(string cityName)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest() with { CityName = cityName };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_1_participant_act_name(string actName)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest();

            requestBody.Group1ParticipantData[0] = requestBody.Group1ParticipantData[0] with { ActName = actName };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_2_participant_act_name(string actName)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest();

            requestBody.Group2ParticipantData[0] = requestBody.Group2ParticipantData[0] with { ActName = actName };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_1_participant_song_title(string songTitle)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest();

            requestBody.Group1ParticipantData[0] = requestBody.Group1ParticipantData[0] with { SongTitle = songTitle };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_a_group_2_participant_song_title(string songTitle)
        {
            CreateContestRequest requestBody = CreateDefaultLiverpoolFormatContest();

            requestBody.Group2ParticipantData[0] = requestBody.Group2ParticipantData[0] with { SongTitle = songTitle };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Liverpool_format_contest_with_participating_countries(
            string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string? group0CountryCode = null)
        {
            CreateContestRequest requestBody = new()
            {
                ContestFormat = ContestFormat.Liverpool,
                ContestYear = TestDefaults.ContestYear,
                CityName = TestDefaults.CityName,
                Group0ParticipatingCountryId = group0CountryCode is not null
                    ? MyCountryCodesAndIds[group0CountryCode]
                    : null,
                Group1ParticipantData = group1CountryCodes.Select(c => MyCountryCodesAndIds[c])
                    .Select(TestDefaults.ParticipantDatum)
                    .ToArray(),
                Group2ParticipantData = group2CountryCodes.Select(c => MyCountryCodesAndIds[c])
                    .Select(TestDefaults.ParticipantDatum)
                    .ToArray()
            };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        private CreateContestRequest CreateDefaultLiverpoolFormatContest()
        {
            Guid[] myCountryIds = MyCountryCodesAndIds.Values.ToArray();

            return new CreateContestRequest
            {
                ContestFormat = ContestFormat.Liverpool,
                ContestYear = TestDefaults.ContestYear,
                CityName = TestDefaults.CityName,
                Group0ParticipatingCountryId = myCountryIds[0],
                Group1ParticipantData = myCountryIds.Skip(1).Take(3).Select(TestDefaults.ParticipantDatum).ToArray(),
                Group2ParticipantData = myCountryIds.Skip(4).Take(3).Select(TestDefaults.ParticipantDatum).ToArray()
            };
        }
    }
}

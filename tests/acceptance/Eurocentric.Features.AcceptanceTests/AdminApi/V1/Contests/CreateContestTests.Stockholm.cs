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
    public async Task Endpoint_should_create_and_return_Stockholm_format_contest_scenario_1(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_contest_for_my_countries(
            contestFormat: "Stockholm",
            contestYear: 2016,
            cityName: "Stockholm",
            group1Participants: """
                                | CountryCode | ActName            | SongTitle            |
                                |-------------|--------------------|----------------------|
                                | AT          | ZOË                | Loin d'ici           |
                                | BE          | Laura Tesoro       | What's The Pressure? |
                                | CZ          | Gabriela Gunčíková | I Stand              |
                                """,
            group2Participants: """
                                | CountryCode | ActName        | SongTitle        |
                                |-------------|----------------|------------------|
                                | DK          | Lighthouse X   | Soldiers Of Love |
                                | EE          | Jüri Pootsmann | Play             |
                                | FI          | Sandhja        | Sing It Away     |
                                """);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_created_contest_should_match(
            contestFormat: "Stockholm",
            contestYear: 2016,
            cityName: "Stockholm",
            completed: false,
            participants: """
                          | Group | CountryCode | ActName            | SongTitle            |
                          |-------|-------------|--------------------|----------------------|
                          | 1     | AT          | ZOË                | Loin d'ici           |
                          | 1     | BE          | Laura Tesoro       | What's The Pressure? |
                          | 1     | CZ          | Gabriela Gunčíková | I Stand              |
                          | 2     | DK          | Lighthouse X       | Soldiers Of Love     |
                          | 2     | EE          | Jüri Pootsmann     | Play                 |
                          | 2     | FI          | Sandhja            | Sing It Away         |
                          """);
        await admin.Then_the_created_contest_should_have_no_child_broadcasts();
        await admin.Then_the_created_contest_should_be_the_only_existing_contest_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_create_and_return_Stockholm_format_contest_scenario_2(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "GB", "NO", "RS", "UA");

        admin.Given_I_want_to_create_a_contest_for_my_countries(
            contestFormat: "Stockholm",
            contestYear: 2022,
            cityName: "Turin",
            group1Participants: """
                                | CountryCode | ActName               | SongTitle |
                                |-------------|-----------------------|-----------|
                                | AT          | LUM!X feat. Pia Maria | Halo      |
                                | GB          | Sam Ryder             | SPACE MAN |
                                | UA          | Kalush Orchestra      | Stefania  |
                                """,
            group2Participants: """
                                | CountryCode | ActName         | SongTitle               |
                                |-------------|-----------------|-------------------------|
                                | BE          | Jérémie Makiese | Miss You                |
                                | CZ          | We Are Domi     | Lights Off              |
                                | NO          | Subwoolfer      | Give That Wolf A Banana |
                                | RS          | Konstrakta      | In Corpore Sano         |
                                """);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_created_contest_should_match(
            contestFormat: "Stockholm",
            contestYear: 2022,
            cityName: "Turin",
            completed: false,
            participants: """
                          | Group | CountryCode | ActName               | SongTitle               |
                          |-------|-------------|-----------------------|-------------------------|
                          | 1     | AT          | LUM!X feat. Pia Maria | Halo                    |
                          | 1     | GB          | Sam Ryder             | SPACE MAN               |
                          | 1     | UA          | Kalush Orchestra      | Stefania                |
                          | 2     | BE          | Jérémie Makiese       | Miss You                |
                          | 2     | CZ          | We Are Domi           | Lights Off              |
                          | 2     | NO          | Subwoolfer            | Give That Wolf A Banana |
                          | 2     | RS          | Konstrakta            | In Corpore Sano         |
                          """);
        await admin.Then_the_created_contest_should_have_no_child_broadcasts();
        await admin.Then_the_created_contest_should_be_the_only_existing_contest_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_non_existent_country_as_group_1_participant(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB");
        await admin.Given_I_have_deleted_my_country("GB");

        await admin.Given_I_want_to_create_a_Stockholm_format_contest_with_my_deleted_country_as_a_group_1_participant();

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
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_non_existent_country_as_group_2_participant(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB");
        await admin.Given_I_have_deleted_my_country("GB");

        await admin.Given_I_want_to_create_a_Stockholm_format_contest_with_my_deleted_country_as_a_group_2_participant();

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
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_non_unique_contest_year(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(
            contestYear: 2016,
            cityName: "Stockholm",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_contest_year(2016);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Contest year conflict",
            detail: "A contest already exists with the provided contest year.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_contest_year(2016);
        await admin.Then_my_given_contest_should_be_the_only_existing_contest_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_illegal_contest_year_value(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_contest_year(2015);

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
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_illegal_city_name_value(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_city_name(" ");

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
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_illegal_group_1_participant_act_name_value(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_a_group_1_participant_act_name(" ");

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
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_illegal_group_2_participant_act_name_value(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_a_group_2_participant_act_name(" ");

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
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_illegal_group_1_participant_song_title_value(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_a_group_1_participant_song_title(" ");

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
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_illegal_group_2_participant_song_title_value(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_a_group_2_participant_song_title(" ");

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
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_duplicate_group_1_and_2_participating_country_IDs(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
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
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_duplicate_group_1_participating_country_IDs(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
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
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_duplicate_group_2_participating_country_IDs(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
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
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_a_group_0_participant(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal Stockholm format participant groups",
            detail: "A Stockholm format contest must have no group 0 participants, " +
                    "at least three group 1 participants, and at least three group 2 participants.");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_fewer_than_three_group_1_participants(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "DK", "EE", "FI");

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            group1CountryCodes: ["AT", "BE"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal Stockholm format participant groups",
            detail: "A Stockholm format contest must have no group 0 participants, " +
                    "at least three group 1 participants, and at least three group 2 participants.");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_with_fewer_than_three_group_2_participants(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE");

        admin.Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal Stockholm format participant groups",
            detail: "A Stockholm format contest must have no group 0 participants, " +
                    "at least three group 1 participants, and at least three group 2 participants.");
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    private sealed partial class AdminActor
    {
        public async Task Given_I_have_created_a_Stockholm_format_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string cityName = "",
            int contestYear = 0)
        {
            Guid[] group1CountryIds = group1CountryCodes.Select(CountryIds.GetSingle).ToArray();
            Guid[] group2CountryIds = group2CountryCodes.Select(CountryIds.GetSingle).ToArray();

            Contest = await ApiDriver.CreateSingleStockholmFormatContestAsync(contestYear: contestYear,
                cityName: cityName,
                group1CountryIds: group1CountryIds,
                group2CountryIds: group2CountryIds);
        }

        public async Task Given_I_want_to_create_a_Stockholm_format_contest_with_my_deleted_country_as_a_group_1_participant()
        {
            Guid myDeletedCountryId = await Assert.That(DeletedCountryId).IsNotNull();

            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest();

            requestBody.Group1ParticipantData[0] = requestBody.Group1ParticipantData[0] with
            {
                ParticipatingCountryId = myDeletedCountryId
            };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public async Task Given_I_want_to_create_a_Stockholm_format_contest_with_my_deleted_country_as_a_group_2_participant()
        {
            Guid myDeletedCountryId = await Assert.That(DeletedCountryId).IsNotNull();

            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest();

            requestBody.Group2ParticipantData[0] = requestBody.Group2ParticipantData[0] with
            {
                ParticipatingCountryId = myDeletedCountryId
            };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Stockholm_format_contest_with_contest_year(int contestYear)
        {
            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest() with { ContestYear = contestYear };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Stockholm_format_contest_with_city_name(string cityName)
        {
            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest() with { CityName = cityName };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Stockholm_format_contest_with_a_group_1_participant_act_name(string actName)
        {
            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest();

            requestBody.Group1ParticipantData[0] = requestBody.Group1ParticipantData[0] with { ActName = actName };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Stockholm_format_contest_with_a_group_2_participant_act_name(string actName)
        {
            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest();

            requestBody.Group2ParticipantData[0] = requestBody.Group2ParticipantData[0] with { ActName = actName };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Stockholm_format_contest_with_a_group_1_participant_song_title(string songTitle)
        {
            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest();

            requestBody.Group1ParticipantData[0] = requestBody.Group1ParticipantData[0] with { SongTitle = songTitle };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Stockholm_format_contest_with_a_group_2_participant_song_title(string songTitle)
        {
            CreateContestRequest requestBody = CreateDefaultStockholmFormatContest();

            requestBody.Group2ParticipantData[0] = requestBody.Group2ParticipantData[0] with { SongTitle = songTitle };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Given_I_want_to_create_a_Stockholm_format_contest_with_participating_countries(
            string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string? group0CountryCode = null)
        {
            CreateContestRequest requestBody = new()
            {
                ContestFormat = ContestFormat.Stockholm,
                ContestYear = TestDefaults.ContestYear,
                CityName = TestDefaults.CityName,
                Group0ParticipatingCountryId = group0CountryCode is not null
                    ? CountryIds.GetSingle(group0CountryCode)
                    : null,
                Group1ParticipantData = group1CountryCodes.Select(CountryIds.GetSingle)
                    .Select(TestDefaults.ParticipantDatum)
                    .ToArray(),
                Group2ParticipantData = group2CountryCodes.Select(CountryIds.GetSingle)
                    .Select(TestDefaults.ParticipantDatum)
                    .ToArray()
            };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        private CreateContestRequest CreateDefaultStockholmFormatContest()
        {
            Guid[] myCountryIds = CountryIds.GetAll();

            return new CreateContestRequest
            {
                ContestFormat = ContestFormat.Stockholm,
                ContestYear = TestDefaults.ContestYear,
                CityName = TestDefaults.CityName,
                Group0ParticipatingCountryId = null,
                Group1ParticipantData = myCountryIds.Take(3).Select(TestDefaults.ParticipantDatum).ToArray(),
                Group2ParticipantData = myCountryIds.Skip(3).Take(3).Select(TestDefaults.ParticipantDatum).ToArray()
            };
        }
    }
}

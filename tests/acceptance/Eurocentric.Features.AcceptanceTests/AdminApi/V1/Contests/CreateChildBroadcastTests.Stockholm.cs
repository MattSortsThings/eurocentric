using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed partial class CreateChildBroadcastTests
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_create_and_return_Stockholm_format_contest_SemiFinal1_broadcast_and_update_contest(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
            broadcastDate: "2016-05-01",
            competingCountryCodes: ["CZ", "AT"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_created_broadcast_should_match(contestStage: "SemiFinal1",
            broadcastDate: "2016-05-01",
            completed: false,
            competitors: """
                         | RunningOrder | CountryCode | Finish |
                         |--------------|-------------|--------|
                         | 1            | CZ          | 1      |
                         | 2            | AT          | 2      |
                         """,
            juries: """
                    | CountryCode | PointsAwarded |
                    |-------------|---------------|
                    | AT          | false         |
                    | BE          | false         |
                    | CZ          | false         |
                    """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       |-------------|---------------|
                       | AT          | false         |
                       | BE          | false         |
                       | CZ          | false         |
                       """);
        await admin.Then_the_created_broadcast_should_reference_my_contest_as_its_parent_contest();
        await admin.Then_the_created_broadcast_should_be_the_only_existing_broadcast_in_the_system();
        await admin.Then_my_contest_should_now_contain_a_single_child_broadcast_referencing_the_created_broadcast();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_create_and_return_Stockholm_format_contest_SemiFinal2_broadcast_and_update_contest(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
            broadcastDate: "2016-05-02",
            competingCountryCodes: ["FI", "DK"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_created_broadcast_should_match(contestStage: "SemiFinal2",
            broadcastDate: "2016-05-02",
            completed: false,
            competitors: """
                         | RunningOrder | CountryCode | Finish |
                         |--------------|-------------|--------|
                         | 1            | FI          | 1      |
                         | 2            | DK          | 2      |
                         """,
            juries: """
                    | CountryCode | PointsAwarded |
                    |-------------|---------------|
                    | DK          | false         |
                    | EE          | false         |
                    | FI          | false         |
                    """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       |-------------|---------------|
                       | DK          | false         |
                       | EE          | false         |
                       | FI          | false         |
                       """);
        await admin.Then_the_created_broadcast_should_reference_my_contest_as_its_parent_contest();
        await admin.Then_the_created_broadcast_should_be_the_only_existing_broadcast_in_the_system();
        await admin.Then_my_contest_should_now_contain_a_single_child_broadcast_referencing_the_created_broadcast();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_create_and_return_Stockholm_format_contest_GrandFinal_broadcast_and_update_contest(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
            broadcastDate: "2016-05-03",
            competingCountryCodes: ["CZ", "DK"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_201_Created();
        await admin.Then_the_created_broadcast_should_match(contestStage: "GrandFinal",
            broadcastDate: "2016-05-03",
            completed: false,
            competitors: """
                         | RunningOrder | CountryCode | Finish |
                         |--------------|-------------|--------|
                         | 1            | CZ          | 1      |
                         | 2            | DK          | 2      |
                         """,
            juries: """
                    | CountryCode | PointsAwarded |
                    |-------------|---------------|
                    | AT          | false         |
                    | BE          | false         |
                    | CZ          | false         |
                    | DK          | false         |
                    | EE          | false         |
                    | FI          | false         |
                    """,
            televotes: """
                       | CountryCode | PointsAwarded |
                       |-------------|---------------|
                       | AT          | false         |
                       | BE          | false         |
                       | CZ          | false         |
                       | DK          | false         |
                       | EE          | false         |
                       | FI          | false         |
                       """);
        await admin.Then_the_created_broadcast_should_reference_my_contest_as_its_parent_contest();
        await admin.Then_the_created_broadcast_should_be_the_only_existing_broadcast_in_the_system();
        await admin.Then_my_contest_should_now_contain_a_single_child_broadcast_referencing_the_created_broadcast();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_SemiFinal1_child_broadcast_when_SemiFinal1_exists(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_SemiFinal1_broadcast_for_my_contest_with_broadcast_date("2016-05-31");

        await admin.Given_I_want_to_create_a_SemiFinal1_child_broadcast_for_my_contest_with_broadcast_date("2016-05-01");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Child broadcast contest stage conflict",
            detail: "The contest already has a child broadcast with the provided contest stage.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_contest_stage("SemiFinal1");
        await admin.Then_my_contest_should_be_unchanged();
        await admin.Then_my_given_broadcast_should_be_the_only_existing_broadcast_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_SemiFinal2_child_broadcast_when_SemiFinal2_exists(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_SemiFinal2_broadcast_for_my_contest_with_broadcast_date("2016-05-31");

        await admin.Given_I_want_to_create_a_SemiFinal2_child_broadcast_for_my_contest_with_broadcast_date("2016-05-02");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Child broadcast contest stage conflict",
            detail: "The contest already has a child broadcast with the provided contest stage.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_contest_stage("SemiFinal2");
        await admin.Then_my_contest_should_be_unchanged();
        await admin.Then_my_given_broadcast_should_be_the_only_existing_broadcast_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_GrandFinal_child_broadcast_when_GrandFinal_exists(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_GrandFinal_broadcast_for_my_contest_with_broadcast_date("2016-05-31");

        await admin.Given_I_want_to_create_a_GrandFinal_child_broadcast_for_my_contest_with_broadcast_date("2016-05-03");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Child broadcast contest stage conflict",
            detail: "The contest already has a child broadcast with the provided contest stage.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_contest_stage("GrandFinal");
        await admin.Then_my_contest_should_be_unchanged();
        await admin.Then_my_given_broadcast_should_be_the_only_existing_broadcast_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_child_broadcast_with_non_unique_broadcast_date(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_SemiFinal1_broadcast_for_my_contest_with_broadcast_date("2016-05-17");

        await admin.Given_I_want_to_create_a_GrandFinal_child_broadcast_for_my_contest_with_broadcast_date("2016-05-17");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Broadcast date conflict",
            detail: "A broadcast already exists with the provided broadcast date.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_broadcast_date("2016-05-17");
        await admin.Then_my_contest_should_be_unchanged();
        await admin.Then_my_given_broadcast_should_be_the_only_existing_broadcast_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_child_broadcast_with_illegal_broadcast_date_value(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_want_to_create_a_SemiFinal1_child_broadcast_for_my_contest_with_broadcast_date("1066-10-14");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal broadcast date value",
            detail: "Broadcast date value must have a year between 2016 and 2050.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_broadcast_date("1066-10-14");
        await admin.Then_my_contest_should_be_unchanged();
        await admin.Then_no_broadcasts_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_child_broadcast_with_broadcast_date_out_of_range(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_want_to_create_a_SemiFinal1_child_broadcast_for_my_contest_with_broadcast_date("2017-01-01");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Child broadcast date out of range",
            detail: "Child broadcast date must match parent contest year.");
        await admin.Then_the_response_problem_details_extensions_should_include_the_broadcast_date("2017-01-01");
        await admin.Then_my_contest_should_be_unchanged();
        await admin.Then_no_broadcasts_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_child_broadcast_with_orphan_competing_country_ID(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_want_to_create_a_SemiFinal1_child_broadcast_for_my_contest_with_competing_countries(
            "AT", "BE", "GB");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Child broadcast competing country IDs mismatch",
            detail: "Every competitor in a child broadcast must have a competing country ID matching a participant in " +
                    "the parent contest that is eligible to compete in the broadcast's contest stage.");
        await admin.Then_my_contest_should_be_unchanged();
        await admin.Then_no_broadcasts_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_SemiFinal1_child_broadcast_with_group_2_competitor(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_want_to_create_a_SemiFinal1_child_broadcast_for_my_contest_with_competing_countries(
            "AT", "BE", "FI");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Child broadcast competing country IDs mismatch",
            detail: "Every competitor in a child broadcast must have a competing country ID matching a participant in " +
                    "the parent contest that is eligible to compete in the broadcast's contest stage.");
        await admin.Then_my_contest_should_be_unchanged();
        await admin.Then_no_broadcasts_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_SemiFinal2_child_broadcast_with_group_1_competitor(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_want_to_create_a_SemiFinal2_child_broadcast_for_my_contest_with_competing_countries(
            "DK", "EE", "AT");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Child broadcast competing country IDs mismatch",
            detail: "Every competitor in a child broadcast must have a competing country ID matching a participant in " +
                    "the parent contest that is eligible to compete in the broadcast's contest stage.");
        await admin.Then_my_contest_should_be_unchanged();
        await admin.Then_no_broadcasts_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_child_broadcast_with_duplicate_competing_country_IDs(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_want_to_create_a_SemiFinal1_child_broadcast_for_my_contest_with_competing_countries(
            "AT", "BE", "AT");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Duplicate competing country IDs",
            detail: "Every competitor in a broadcast must have a different competing country ID.");
        await admin.Then_my_contest_should_be_unchanged();
        await admin.Then_no_broadcasts_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_Stockholm_format_contest_child_broadcast_with_fewer_than_two_competitors(
        string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_want_to_create_a_SemiFinal1_child_broadcast_for_my_contest_with_competing_countries("AT");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity();
        await admin.Then_the_response_problem_details_should_match(status: 422,
            title: "Illegal competitor count",
            detail: "A broadcast must have at least 2 competitors.");
        await admin.Then_my_contest_should_be_unchanged();
        await admin.Then_no_broadcasts_should_exist_in_the_system();
    }

    private sealed partial class AdminActor
    {
        public async Task Given_I_have_created_a_Stockholm_format_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            int contestYear = 0)
        {
            Guid[] group1CountryIds = CountryIds.GetMultiple(group1CountryCodes);
            Guid[] group2CountryIds = CountryIds.GetMultiple(group2CountryCodes);

            Contest = await ApiDriver.CreateSingleStockholmFormatContestAsync(contestYear: contestYear,
                cityName: TestDefaults.CityName,
                group1CountryIds: group1CountryIds,
                group2CountryIds: group2CountryIds);
        }
    }
}

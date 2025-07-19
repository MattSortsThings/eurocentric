using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Responses;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public static partial class CreateChildBroadcastTests
{
    public sealed partial class Endpoint
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_create_and_return_SemiFinal1_child_broadcast_for_Stockholm_format_contest_and_update_contest(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2025,
                cityName: "Basel",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
                broadcastDate: "2025-05-01",
                competingCountryCodes: ["BE", "AT"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(201);
            admin.Then_the_created_broadcast_should_match(contestStage: "SemiFinal1",
                broadcastDate: "2025-05-01",
                completed: false,
                competitors: """
                             | Finishing | CountryCode | RunningOrder |
                             |----------:|:------------|-------------:|
                             |         1 | BE          |            1 |
                             |         2 | AT          |            2 |
                             """,
                juries: """
                        | CountryCode | PointsAwarded |
                        |:------------|:--------------|
                        | AT          | false         |
                        | BE          | false         |
                        | CZ          | false         |
                        """,
                televotes: """
                           | CountryCode | PointsAwarded |
                           |:------------|:--------------|
                           | AT          | false         |
                           | BE          | false         |
                           | CZ          | false         |
                           """);
            admin.Then_the_created_broadcast_should_reference_my_contest_as_its_parent_contest();
            await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
            await admin.Then_my_contest_should_now_reference_the_created_broadcast_as_one_of_its_child_broadcasts();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_create_and_return_SemiFinal2_child_broadcast_for_Stockholm_format_contest_and_update_contest(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2025,
                cityName: "Basel",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal2",
                broadcastDate: "2025-05-02",
                competingCountryCodes: ["FI", "DK"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(201);
            admin.Then_the_created_broadcast_should_match(contestStage: "SemiFinal2",
                broadcastDate: "2025-05-02",
                completed: false,
                competitors: """
                             | Finishing | CountryCode | RunningOrder |
                             |----------:|:------------|-------------:|
                             |         1 | FI          |            1 |
                             |         2 | DK          |            2 |
                             """,
                juries: """
                        | CountryCode | PointsAwarded |
                        |:------------|:--------------|
                        | DK          | false         |
                        | EE          | false         |
                        | FI          | false         |
                        """,
                televotes: """
                           | CountryCode | PointsAwarded |
                           |:------------|:--------------|
                           | DK          | false         |
                           | EE          | false         |
                           | FI          | false         |
                           """);
            admin.Then_the_created_broadcast_should_reference_my_contest_as_its_parent_contest();
            await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
            await admin.Then_my_contest_should_now_reference_the_created_broadcast_as_one_of_its_child_broadcasts();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_create_and_return_GrandFinal_child_broadcast_for_Stockholm_format_contest_and_update_contest(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2025,
                cityName: "Basel",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
                broadcastDate: "2025-05-03",
                competingCountryCodes: ["FI", "AT"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(201);
            admin.Then_the_created_broadcast_should_match(contestStage: "GrandFinal",
                broadcastDate: "2025-05-03",
                completed: false,
                competitors: """
                             | Finishing | CountryCode | RunningOrder |
                             |----------:|:------------|-------------:|
                             |         1 | FI          |            1 |
                             |         2 | AT          |            2 |
                             """,
                juries: """
                        | CountryCode | PointsAwarded |
                        |:------------|:--------------|
                        | AT          | false         |
                        | BE          | false         |
                        | CZ          | false         |
                        | DK          | false         |
                        | EE          | false         |
                        | FI          | false         |
                        """,
                televotes: """
                           | CountryCode | PointsAwarded |
                           |:------------|:--------------|
                           | AT          | false         |
                           | BE          | false         |
                           | CZ          | false         |
                           | DK          | false         |
                           | EE          | false         |
                           | FI          | false         |
                           """);
            admin.Then_the_created_broadcast_should_reference_my_contest_as_its_parent_contest();
            await admin.Then_the_created_broadcast_should_be_retrievable_by_its_ID();
            await admin.Then_my_contest_should_now_reference_the_created_broadcast_as_one_of_its_child_broadcasts();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Stockholm_format_contest_child_broadcast_with_non_unique_contest_stage(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2025,
                cityName: "Basel",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            await admin.Given_I_have_created_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
                broadcastDate: "2025-05-31",
                competingCountryCodes: ["AT", "DK"]);
            admin.Given_I_want_to_create_a_child_broadcast_for_my_contest_with_contest_stage("GrandFinal");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(409);
            admin.Then_the_response_problem_details_should_match(status: 409,
                title: "Child broadcast contest stage conflict",
                detail: "A child broadcast already exists for the contest with the provided contest stage.");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "contestStage", value: "GrandFinal");
            await admin.Then_my_given_broadcast_should_be_the_only_existing_contest();
            await admin.Then_my_contest_should_be_unchanged();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Stockholm_format_contest_child_broadcast_with_broadcast_date_out_of_range(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2025,
                cityName: "Basel",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_create_a_child_broadcast_for_my_contest_with_broadcast_date("2024-12-31");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(409);
            admin.Then_the_response_problem_details_should_match(status: 409,
                title: "Child broadcast date out of range",
                detail: "A broadcast's date must be in the same year as its parent contest.");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "broadcastDate", value: "2024-12-31");
            await admin.Then_no_broadcasts_should_exist();
            await admin.Then_my_contest_should_be_unchanged();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Stockholm_format_contest_child_broadcast_with_orphan_competing_country_ID(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2025,
                cityName: "Basel",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_create_a_GrandFinal_child_broadcast_for_my_contest_with_competing_countries("AT", "GB");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(409);
            admin.Then_the_response_problem_details_should_match(status: 409,
                title: "Orphan competing country ID",
                detail: "Competing country ID has no matching participant in parent contest.");
            admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country_with_country_code("GB");
            await admin.Then_no_broadcasts_should_exist();
            await admin.Then_my_contest_should_be_unchanged();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Stockholm_format_contest_SemiFinal1_child_broadcast_with_group_2_competitor(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2025,
                cityName: "Basel",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_create_a_SemiFinal1_child_broadcast_for_my_contest_with_competing_countries("AT", "FI");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(409);
            admin.Then_the_response_problem_details_should_match(status: 409,
                title: "Ineligible competing country ID",
                detail: "Competing country ID matches ineligible participant in parent contest given contest stage.");
            admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country_with_country_code("FI");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "contestStage", value: "SemiFinal1");
            await admin.Then_no_broadcasts_should_exist();
            await admin.Then_my_contest_should_be_unchanged();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Stockholm_format_contest_SemiFinal2_child_broadcast_with_group_1_competitor(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2025,
                cityName: "Basel",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_create_a_SemiFinal2_child_broadcast_for_my_contest_with_competing_countries("DK", "AT");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(409);
            admin.Then_the_response_problem_details_should_match(status: 409,
                title: "Ineligible competing country ID",
                detail: "Competing country ID matches ineligible participant in parent contest given contest stage.");
            admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country_with_country_code("AT");
            admin.Then_the_response_problem_details_extensions_should_contain(key: "contestStage", value: "SemiFinal2");
            await admin.Then_no_broadcasts_should_exist();
            await admin.Then_my_contest_should_be_unchanged();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Stockholm_format_contest_child_broadcast_with_duplicate_competing_country_IDs(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2025,
                cityName: "Basel",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_create_a_GrandFinal_child_broadcast_for_my_contest_with_competing_countries("DK", "AT", "DK");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Duplicate competing country IDs",
                detail: "Each competitor in a broadcast must have a competing country ID referencing a different country.");
            await admin.Then_no_broadcasts_should_exist();
            await admin.Then_my_contest_should_be_unchanged();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_Stockholm_format_contest_child_broadcast_with_fewer_than_two_competitors(
            string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2025,
                cityName: "Basel",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_create_a_GrandFinal_child_broadcast_for_my_contest_with_competing_countries("DK");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(422);
            admin.Then_the_response_problem_details_should_match(status: 422,
                title: "Illegal broadcast size",
                detail: "A broadcast must have at least 2 competitors.");
            await admin.Then_no_broadcasts_should_exist();
            await admin.Then_my_contest_should_be_unchanged();
        }
    }
}

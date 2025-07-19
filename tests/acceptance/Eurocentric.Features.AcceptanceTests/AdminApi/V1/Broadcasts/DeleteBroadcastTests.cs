using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Responses;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public static class DeleteBroadcastTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_delete_requested_broadcast(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2016,
                cityName: "Stockholm",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            await admin.Given_I_have_created_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
                broadcastDate: "2016-05-01",
                competingCountryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI"]);
            admin.Given_I_want_to_delete_my_broadcast();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(204);
            await admin.Then_no_broadcasts_should_exist();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_non_existent_broadcast_requested(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2016,
                cityName: "Stockholm",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            await admin.Given_I_have_created_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
                broadcastDate: "2016-05-01",
                competingCountryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI"]);
            await admin.Given_I_have_deleted_my_broadcast();
            admin.Given_I_want_to_delete_my_broadcast();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(404);
            admin.Then_the_response_problem_details_should_match(status: 404,
                title: "Broadcast not found",
                detail: "No broadcast exists with the provided broadcast ID.");
            admin.Then_the_response_problem_details_extensions_should_include_my_broadcast_ID();
        }
    }

    private sealed class Admin : AdminActorWithoutResponse
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, IRequestFactory requestFactory) :
            base(restClient, backDoor, requestFactory)
        {
        }

        public void Given_I_want_to_delete_my_broadcast()
        {
            Broadcast myBroadcast = GivenBroadcasts.GetSingle();

            Request = RequestFactory.Broadcasts.DeleteBroadcast(myBroadcast.Id);
        }
    }
}

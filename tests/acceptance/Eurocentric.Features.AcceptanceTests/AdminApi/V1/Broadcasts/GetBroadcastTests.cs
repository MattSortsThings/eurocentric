using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public static class GetBroadcastTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_retrieve_requested_broadcast(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(
                contestYear: 2022,
                cityName: "Turin",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
                contestStage: "GrandFinal",
                broadcastDate: "2022-05-14",
                competingCountryCodes: ["AT", "BE", "DK", "FI"]);
            admin.Given_I_want_to_retrieve_my_broadcast_by_its_ID();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_broadcast_should_be_my_broadcast();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_non_existent_broadcast_requested(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(
                contestYear: 2022,
                cityName: "Turin",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
                contestStage: "GrandFinal",
                broadcastDate: "2022-05-14",
                competingCountryCodes: ["AT", "BE", "DK", "FI"]);
            await admin.Given_I_have_deleted_every_broadcast_I_have_created();
            admin.Given_I_want_to_retrieve_my_broadcast_by_its_ID();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(404);
            admin.Then_the_response_problem_details_should_match(status: 404,
                title: "Broadcast not found",
                detail: "No broadcast exists with the provided broadcast ID.");
            admin.Then_the_response_problem_details_extensions_should_contain_my_broadcast_ID();
        }
    }

    private sealed class Admin : AdminActor<GetBroadcastResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_my_broadcast_by_its_ID()
        {
            Broadcast myBroadcast = Assert.Single(GivenBroadcasts);

            Request = RequestFactory.Broadcasts.GetBroadcast(myBroadcast.Id);
        }

        public void Then_the_retrieved_broadcast_should_be_my_broadcast()
        {
            Assert.NotNull(ResponseObject);

            Broadcast myBroadcast = Assert.Single(GivenBroadcasts);
            Broadcast retrievedBroadcast = ResponseObject.Broadcast;

            Assert.Equal(myBroadcast, retrievedBroadcast, BroadcastEquality.Compare);
        }

        public void Then_the_response_problem_details_extensions_should_contain_my_broadcast_ID()
        {
            Assert.NotNull(ResponseProblemDetails);
            Broadcast myBroadcast = Assert.Single(GivenBroadcasts);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "broadcastId", Value: JsonElement je }
                                                                      && je.GetGuid() == myBroadcast.Id);
        }
    }
}

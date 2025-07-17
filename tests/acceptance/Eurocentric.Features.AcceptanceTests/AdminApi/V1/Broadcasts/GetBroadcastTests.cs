using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Responses;
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
        public async Task Should_retrieve_requested_contest(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            await admin.Given_I_have_created_a_Liverpool_format_contest(contestYear: 2025,
                cityName: "Basel",
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            await admin.Given_I_have_created_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
                broadcastDate: "2025-05-03",
                competingCountryCodes: ["AT", "DK"]);
            admin.Given_I_want_to_retrieve_my_broadcast();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_broadcast_should_be_my_broadcast();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_non_existent_country_requested(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            await admin.Given_I_have_created_a_Liverpool_format_contest(contestYear: 2025,
                cityName: "Basel",
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            await admin.Given_I_have_created_a_child_broadcast_for_my_contest(contestStage: "GrandFinal",
                broadcastDate: "2025-05-03",
                competingCountryCodes: ["AT", "DK"]);
            await admin.Given_I_have_deleted_my_broadcast();
            admin.Given_I_want_to_retrieve_my_broadcast();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(404);
            admin.Then_the_response_problem_details_should_match(status: 404,
                title: "Broadcast not found",
                detail: "No broadcast exists with the provided broadcast ID.");
            admin.Then_the_response_problem_details_should_have_a_broadcastId_extension_with_my_broadcast_ID();
        }
    }

    private sealed class Admin : AdminActorWithResponse<GetBroadcastResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, IRequestFactory requestFactory) :
            base(restClient, backDoor, requestFactory)
        {
        }

        public void Given_I_want_to_retrieve_my_broadcast()
        {
            Broadcast broadcast = GivenBroadcasts.GetSingle();

            Request = RequestFactory.Broadcasts.GetBroadcast(broadcast.Id);
        }

        public void Then_the_retrieved_broadcast_should_be_my_broadcast()
        {
            Assert.NotNull(ResponseObject);

            Broadcast expectedBroadcast = GivenBroadcasts.GetSingle();
            Broadcast retrievedBroadcast = ResponseObject.Broadcast;

            Assert.Equal(expectedBroadcast, retrievedBroadcast, new BroadcastEqualityComparer());
        }

        public void Then_the_response_problem_details_should_have_a_broadcastId_extension_with_my_broadcast_ID()
        {
            Assert.NotNull(ResponseProblemDetails);

            Guid expectedBroadcastId = GivenBroadcasts.GetSingle().Id;

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "broadcastId", Value: JsonElement je }
                                                                      && je.GetGuid() == expectedBroadcastId);
        }
    }
}

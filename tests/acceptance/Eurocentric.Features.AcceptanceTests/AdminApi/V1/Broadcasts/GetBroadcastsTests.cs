using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public static class GetBroadcastsTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_retrieve_all_existing_broadcasts_in_broadcast_date_order(string apiVersion)
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
            await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
                contestStage: "SemiFinal1",
                broadcastDate: "2022-05-10",
                competingCountryCodes: ["AT", "BE", "CZ"]);
            await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
                contestStage: "SemiFinal2",
                broadcastDate: "2022-05-12",
                competingCountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_retrieve_all_existing_broadcasts();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_broadcasts_should_be_my_broadcasts_in_broadcast_date_order();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_retrieve_empty_list_when_no_broadcasts_exist(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            admin.Given_I_want_to_retrieve_all_existing_broadcasts();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_broadcasts_should_be_an_empty_list();
        }
    }

    private sealed class Admin : AdminActor<GetBroadcastsResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_all_existing_broadcasts() => Request = RequestFactory.Broadcasts.GetBroadcasts();

        public void Then_the_retrieved_broadcasts_should_be_my_broadcasts_in_broadcast_date_order()
        {
            Assert.NotNull(ResponseObject);

            IOrderedEnumerable<Broadcast> expectedBroadcasts = GivenBroadcasts.OrderBy(broadcast => broadcast.BroadcastDate);
            Broadcast[] actualBroadcasts = ResponseObject.Broadcasts;

            Assert.Equal(expectedBroadcasts, actualBroadcasts, new BroadcastEqualityComparer());
        }

        public void Then_the_retrieved_broadcasts_should_be_an_empty_list()
        {
            Assert.NotNull(ResponseObject);

            Assert.Empty(ResponseObject.Broadcasts);
        }
    }
}

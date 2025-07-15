using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
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
        public async Task Should_retrieve_dummy_broadcast(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            admin.Given_I_want_to_retrieve_the_broadcast_with_ID("e1450140-83d8-46d8-ae19-96a9a4856200");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_broadcast_should_have_ID("e1450140-83d8-46d8-ae19-96a9a4856200");
        }
    }

    private sealed class Admin : AdminActor<GetBroadcastResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_the_broadcast_with_ID(string broadcastId) =>
            Request = RequestFactory.Broadcasts.GetBroadcast(Guid.Parse(broadcastId));

        public void Then_the_retrieved_broadcast_should_have_ID(string broadcastId)
        {
            Assert.NotNull(ResponseObject);

            Broadcast retrievedBroadcast = ResponseObject.Broadcast;

            Assert.Equal(Guid.Parse(broadcastId), retrievedBroadcast.Id);
        }
    }
}

using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Broadcasts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastTest(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_dummy_broadcast_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_broadcast_with_ID("a58e0def-e65a-4916-be16-c464022de733");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_broadcast_ID_should_be("a58e0def-e65a-4916-be16-c464022de733");
    }

    private sealed class AdminActor : ActorWithResponse<GetBroadcastResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        public void Given_I_want_to_retrieve_the_broadcast_with_ID(string broadcastId)
        {
            Guid targetId = Guid.Parse(broadcastId);

            SendMyRequest = apiDriver => apiDriver.Broadcasts.GetBroadcast(targetId, TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_broadcast_ID_should_be(string broadcastId)
        {
            Assert.NotNull(ResponseObject);

            Guid expectedBroadcastId = Guid.Parse(broadcastId);

            Assert.Equal(expectedBroadcastId, ResponseObject.Broadcast.Id);
        }
    }
}

using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AdminApi.V1.Broadcasts.GetBroadcast;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_dummy_broadcast(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_broadcast_with_ID("4fcc3fcb-05eb-4574-a25b-d8c72f8972b0");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_broadcast_should_have_the_ID("4fcc3fcb-05eb-4574-a25b-d8c72f8972b0");
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<GetBroadcastResponse>(apiDriver)
    {
        public void Given_I_want_to_retrieve_the_broadcast_with_ID(string broadcastId)
        {
            Guid targetBroadcastId = Guid.Parse(broadcastId);

            Request = ApiDriver.RequestFactory.Broadcasts.GetBroadcast(targetBroadcastId);
        }

        public async Task Then_the_retrieved_broadcast_should_have_the_ID(string broadcastId)
        {
            Guid expectedBroadcastId = Guid.Parse(broadcastId);

            GetBroadcastResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.Broadcast)
                .HasMember(broadcast => broadcast.Id).EqualTo(expectedBroadcastId);
        }
    }
}

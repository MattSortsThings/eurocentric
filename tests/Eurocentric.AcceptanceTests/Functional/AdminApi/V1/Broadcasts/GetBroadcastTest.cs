using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Features.Broadcasts;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastTest : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_dummy_broadcast(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_broadcast_with_ID("d5582129-3090-4ec6-a69e-e900a228d2b3");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_broadcast_should_have_ID("d5582129-3090-4ec6-a69e-e900a228d2b3");
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetBroadcastResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_the_broadcast_with_ID(string broadcastId)
        {
            Guid id = Guid.Parse(broadcastId);

            Request = Kernel.Requests.Broadcasts.GetBroadcast(id);
        }

        public async Task Then_the_retrieved_broadcast_should_have_ID(string broadcastId)
        {
            Guid expectedId = Guid.Parse(broadcastId);

            await Assert.That(SuccessResponse?.Data?.Broadcast.Id).IsEqualTo(expectedId);
        }
    }
}

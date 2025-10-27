using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Features.Broadcasts;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastsTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_dummy_broadcasts(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_broadcasts();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_broadcasts_should_be_in_broadcast_date_order();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetBroadcastsResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_all_existing_broadcasts() =>
            Request = Kernel.Requests.Broadcasts.GetBroadcasts();

        public async Task Then_the_retrieved_broadcasts_should_be_in_broadcast_date_order()
        {
            await Assert
                .That(SuccessResponse?.Data?.Broadcasts)
                .IsNotEmpty()
                .And.IsOrderedBy(broadcast => broadcast.BroadcastDate);
        }
    }
}

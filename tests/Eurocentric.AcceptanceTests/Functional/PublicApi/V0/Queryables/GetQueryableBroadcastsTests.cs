using Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V0.Features.Queryables;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V0.Queryables;

[Category("public-api")]
public sealed class GetQueryableBroadcastsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_retrieve_all_the_queryable_broadcasts_in_broadcast_date_order(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_the_queryable_broadcasts();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_queryable_broadcasts_should_have_count(6);
        await euroFan.Then_the_retrieved_queryable_broadcasts_should_be_in_broadcast_date_order();
    }

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetQueryableBroadcastsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_all_the_queryable_broadcasts() =>
            Request = Kernel.Requests.Queryables.GetQueryableBroadcasts();

        public async Task Then_the_retrieved_queryable_broadcasts_should_have_count(int count)
        {
            GetQueryableBroadcastsResponse response = await Assert.That(SuccessResponse?.Data).IsNotNull();

            await Assert.That(response.QueryableBroadcasts).HasCount(count);
        }

        public async Task Then_the_retrieved_queryable_broadcasts_should_be_in_broadcast_date_order()
        {
            GetQueryableBroadcastsResponse response = await Assert.That(SuccessResponse?.Data).IsNotNull();

            await Assert.That(response.QueryableBroadcasts).IsOrderedBy(broadcast => broadcast.BroadcastDate);
        }
    }
}

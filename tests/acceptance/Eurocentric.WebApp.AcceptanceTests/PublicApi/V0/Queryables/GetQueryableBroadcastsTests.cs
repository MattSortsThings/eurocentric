using Eurocentric.Apis.Public.V0.Dtos.Queryables;
using Eurocentric.Apis.Public.V0.Features.Queryables;
using Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Utils.Attributes;
using Eurocentric.WebApp.AcceptanceTests.Utils;

namespace Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Queryables;

public sealed class GetQueryableBroadcastsTests : SeededParallelAcceptanceTest
{
    [Test]
    [V0Point1AndUp]
    public async Task Should_retrieve_all_queryable_broadcasts_in_broadcast_date_order(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_the_queryable_broadcasts();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_retrieved_queryable_broadcasts_should_have_count(6);
        await euroFan.Then_the_retrieved_queryable_broadcasts_should_be_in_broadcast_date_order();
    }

    private sealed class EuroFanActor : EuroFanActorWithResponse<GetQueryableBroadcasts.Response>
    {
        public EuroFanActor(IApiDriver apiDriver) : base(apiDriver) { }

        public void Given_I_want_to_retrieve_all_the_queryable_broadcasts() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableBroadcasts();

        public async Task Then_the_retrieved_queryable_broadcasts_should_have_count(int count)
        {
            GetQueryableBroadcasts.Response responseObject = await Assert.That(ResponseObject).IsNotNull();

            await Assert.That(responseObject.QueryableBroadcasts).HasCount(count);
        }

        public async Task Then_the_retrieved_queryable_broadcasts_should_be_in_broadcast_date_order()
        {
            GetQueryableBroadcasts.Response responseObject = await Assert.That(ResponseObject).IsNotNull();

            await Assert.That(responseObject.QueryableBroadcasts).IsInOrder(new BroadcastDateComparer());
        }
    }

    private sealed class BroadcastDateComparer : IComparer<QueryableBroadcast>
    {
        public int Compare(QueryableBroadcast? x, QueryableBroadcast? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (y is null)
            {
                return 1;
            }

            if (x is null)
            {
                return -1;
            }

            return x.BroadcastDate.CompareTo(y.BroadcastDate);
        }
    }
}

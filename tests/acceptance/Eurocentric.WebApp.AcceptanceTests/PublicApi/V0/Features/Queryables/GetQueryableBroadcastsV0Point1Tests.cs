using Eurocentric.Apis.Public.V0.Dtos.Queryables;
using Eurocentric.Apis.Public.V0.Features.Queryables;
using Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.WebApp.AcceptanceTests.Utils;

namespace Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Features.Queryables;

public sealed class GetQueryableBroadcastsV0Point1Tests : SeededParallelAcceptanceTest
{
    [Test]
    public async Task Request_should_retrieve_all_queryable_broadcasts_in_broadcast_date_order()
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, "v0.1"));

        // Given
        euroFan.Given_I_want_to_retrieve_all_queryable_broadcasts();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_retrieved_broadcasts_should_have_count(6);
        await euroFan.Then_the_retrieved_broadcasts_should_be_in_date_order();
    }

    private sealed class EuroFanActor : EuroFanActorWithResponse<GetQueryableBroadcastsV0Point1.Response>
    {
        public EuroFanActor(IApiDriver apiDriver) : base(apiDriver)
        {
        }

        public void Given_I_want_to_retrieve_all_queryable_broadcasts() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableBroadcasts();

        public async Task Then_the_retrieved_broadcasts_should_have_count(int count)
        {
            GetQueryableBroadcastsV0Point1.Response responseObject = await Assert.That(ResponseObject).IsNotNull();

            await Assert.That(responseObject.QueryableBroadcasts).HasCount(count);
        }

        public async Task Then_the_retrieved_broadcasts_should_be_in_date_order()
        {
            GetQueryableBroadcastsV0Point1.Response responseObject = await Assert.That(ResponseObject).IsNotNull();

            await Assert.That(responseObject.QueryableBroadcasts).IsInOrder(new QueryableBroadcastBroadcastDateComparer());
        }
    }

    private sealed class QueryableBroadcastBroadcastDateComparer : IComparer<QueryableBroadcast>
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

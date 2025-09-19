using Eurocentric.Apis.Public.V0.Features.Queryables;
using Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.WebApp.AcceptanceTests.Utils;

namespace Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Features.Queryables;

public sealed class GetQueryableBroadcastsV0Point2Tests : SeededParallelAcceptanceTest
{
    [Test]
    public async Task Request_should_retrieve_all_queryable_broadcasts_in_broadcast_date_order()
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, "v0.2"));

        // Given
        euroFan.Given_I_want_to_retrieve_all_queryable_broadcasts();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
    }

    private sealed class EuroFanActor : EuroFanActorWithResponse<GetQueryableBroadcastsV0Point1.Response>
    {
        public EuroFanActor(IApiDriver apiDriver) : base(apiDriver)
        {
        }

        public void Given_I_want_to_retrieve_all_queryable_broadcasts() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableBroadcasts();
    }
}

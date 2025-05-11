using System.Net;
using Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.PublicApi.V0.Filters;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Filters;

public abstract class GetAvailableVotingMethodsTests : AcceptanceTestBase
{
    protected GetAvailableVotingMethodsTests(WebAppFixture webAppFixture) : base(webAppFixture) { }

    [Fact]
    public async Task Should_be_able_to_retrieve_all_available_voting_methods()
    {
        EuroFanActor euroFan = new(PublicApiV0Driver.Create(Client, MajorApiVersion, MinorApiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_available_voting_methods();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        euroFan.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
    }

    public sealed class V0Point1 : GetAvailableVotingMethodsTests
    {
        public V0Point1(WebAppFixture webAppFixture) : base(webAppFixture) { }

        private protected override int MajorApiVersion => 0;

        private protected override int MinorApiVersion => 1;
    }

    public sealed class V0Point2 : GetAvailableVotingMethodsTests
    {
        public V0Point2(WebAppFixture webAppFixture) : base(webAppFixture) { }

        private protected override int MajorApiVersion => 0;

        private protected override int MinorApiVersion => 2;
    }

    private sealed class EuroFanActor : ActorBase<GetAvailableVotingMethods.Response>
    {
        private readonly PublicApiV0Driver _driver;

        public EuroFanActor(PublicApiV0Driver driver)
        {
            _driver = driver;
        }

        private protected override Func<Task<ResponseOrProblem<GetAvailableVotingMethods.Response>>>
            SendMyRequest { get; set; } = null!;

        public void Given_I_want_to_retrieve_all_available_voting_methods() => SendMyRequest = () =>
            _driver.GetAvailableVotingMethodsAsync(TestContext.Current.CancellationToken);
    }
}

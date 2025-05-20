using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Broadcasts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastTests : AcceptanceTestBase
{
    public GetBroadcastTests(WebAppFixture webAppFixture) : base(webAppFixture)
    {
    }

    private protected override int ApiMajorVersion => 1;

    private protected override int ApiMinorVersion => 0;

    [Fact]
    public async Task Should_be_able_to_retrieve_dummy_broadcast()
    {
        AdminActor admin = AdminActor.WithDriver(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_broadcast_with_the_ID("a5a5e60e-15f8-4cab-afd9-84a8ad81e158");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_broadcast_should_have_the_target_broadcast_ID();
    }

    private sealed class AdminActor : ActorWithResponse<GetBroadcastResponse>
    {
        private readonly AdminApiV1Driver _driver;

        private AdminActor(AdminApiV1Driver driver)
        {
            _driver = driver;
        }

        private Guid TargetBroadcastId { get; set; }

        private protected override Func<Task<ResponseOrProblem<GetBroadcastResponse>>> SendMyRequest { get; set; } = null!;

        public static AdminActor WithDriver(AdminApiV1Driver driver) => new(driver);

        public void Given_I_want_to_retrieve_the_broadcast_with_the_ID(string broadcastId)
        {
            TargetBroadcastId = Guid.Parse(broadcastId);

            SendMyRequest = () => _driver.GetBroadcastAsync(TargetBroadcastId, TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_broadcast_should_have_the_target_broadcast_ID()
        {
            Assert.NotNull(Response);

            Assert.Equal(TargetBroadcastId, Response.Broadcast.Id);
        }
    }
}
